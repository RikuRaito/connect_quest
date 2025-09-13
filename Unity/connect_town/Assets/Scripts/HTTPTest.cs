using UnityEngine;
using UnityEngine.UI;

public class HTTPTest : MonoBehaviour
{
    public Button testButton;

    void Start()
    {
        testButton.onClick.AddListener(OnTestButtonClicked);
    }

    void OnTestButtonClicked()
    {
        Debug.Log("Unity側からHTTP通信リクエストを送信しました");

        // Swift 側の関数を呼ぶ
        CallSwiftHealthCheck();
    }

    // Swift関数をブリッジ経由で呼び出す
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void HealthCheckFromUnity();

    private void CallSwiftHealthCheck()
    {
#if UNITY_IOS && !UNITY_EDITOR
        HealthCheckFromUnity();
#endif
    }

    // Swift 側から呼ばれるメソッド（UnitySendMessage）
    public void OnHealthCheckResult(string result)
    {
        Debug.Log("サーバー応答: " + result);
    }
}