using System.Collections;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.InputSystem;

public class HTTPManager : MonoBehaviour
{
    private string portNumber = "http://localhost:8765";
    private string dataApiUrl;
    private string GPSApiUrl;
    private string healthApiUrl;

    void Awake()
    {
        dataApiUrl = portNumber + "/api/data";
        GPSApiUrl = portNumber + "/api/location";
        healthApiUrl = portNumber + "/api/health";
    }

    void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            StartCoroutine(HealthCheck());
        }
    }

    public IEnumerator HealthCheck() {
        using (UnityWebRequest www = UnityWebRequest.Get(healthApiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Response: " + www.downloadHandler.text);
            }
        }
    }

    // GET リクエスト
    public IEnumerator GetData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(dataApiUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Response: " + www.downloadHandler.text);
            }
        }
    }

    // POST リクエスト
    public IEnumerator PostData(string json)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = new UnityWebRequest(dataApiUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);
        }
    }

    public IEnumerator PostLocation(float latitude, float longitude)
    {
        string json = JsonUtility.ToJson(new LocationData { latitude = latitude, longitude = longitude });
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = new UnityWebRequest(GPSApiUrl, "POST");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            Debug.Log("Response: " + www.downloadHandler.text);
        }
    }

    [System.Serializable]
    private class LocationData
    {
        public float latitude;
        public float longitude;
    }
}