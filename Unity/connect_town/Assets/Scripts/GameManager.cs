using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text isSuccess;

    public void OnHealthCheckResult(string message)
    {
        Debug.Log("サーバー応答" + message);
        isSuccess.text = "ヘルスチェック成功" + message;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
