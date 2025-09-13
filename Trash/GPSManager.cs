using UnityEngine;
using System.Collections;

public class GPSManager : MonoBehaviour
{
    public static GPSManager Instance;
    [SerializeField] public HTTPManager httpManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//シーンを跨いで保持
            if (httpManager == null)
            {
                httpManager = Object.FindFirstObjectByType<HTTPManager>();
                if (httpManager == null)
                {
                    Debug.LogWarning("HTTPManager was not found in the scene. Please add one and assign it in GPSManager");
                }
            }
        }
        else
        {
            Destroy(gameObject);//既に存在しているなら新しいものは消す
        }
    }

    IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("GPS is not enabled by the user");
            yield break;
        }

        //サービス開始（精度：10m, 更新間隔：１秒）
        Input.location.Start(10f, 1f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }

        StartCoroutine(PollLocation());
    }

    IEnumerator PollLocation()
    {
        while (true)
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                float latitude = Input.location.lastData.latitude;
                float longitude = Input.location.lastData.longitude;

                if (httpManager != null)
                {
                    StartCoroutine(httpManager.PostLocation(latitude, longitude));
                }
            }
            yield return new WaitForSeconds(5f);
        }
    }
}