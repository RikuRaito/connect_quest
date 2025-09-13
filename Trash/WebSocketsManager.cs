using UnityEngine;
using UnityEngine.InputSystem;
using WebSocketSharp;

public class WebSocketManager : MonoBehaviour
{
    private WebSocket ws;

    void Start()
    {
        // 接続先を指定（必要に応じて ws://localhost:8765 を変更）
        ws = new WebSocket("ws://localhost:8765");

        // 接続イベント
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("✅ Connected to server");
        };

        // メッセージ受信イベント
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("📩 Received from server: " + e.Data);
        };

        // エラーイベント
        ws.OnError += (sender, e) =>
        {
            Debug.LogError("⚠️ WebSocket Error: " + e.Message);
        };

        // 接続終了イベント
        ws.OnClose += (sender, e) =>
        {
            Debug.Log("🔌 Disconnected from server");
        };

        // 接続開始
        ws.Connect();
    }

    void Update()
    {
        // Sキーを押したらメッセージ送信
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            if (ws != null && ws.IsAlive)
            {
                string msg = "Hello Server! from Unity";
                ws.Send(msg);
                Debug.Log("📤 Sent: " + msg);
            }
            Debug.Log("Detected Pressing S key");
        }
    }

    void OnDestroy()
    {
        // 終了時に切断
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }
}