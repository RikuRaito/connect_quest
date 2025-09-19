using UnityEngine;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [Header("Popup UI")]
    public GameObject popupRoot;            // PopupPanel
    public TextMeshProUGUI messageText;     // MessageText (TMP)
    public UpgradeButton upgradeButton;     // UpgradeButton コンポーネント

    void Awake()
    {
        Debug.Log("PopupManager Awake called on: " + gameObject.name);

        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Destroying duplicate PopupManager on: " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Debug.Log("popupRoot assigned? " + (popupRoot != null));
        Debug.Log("messageText assigned? " + (messageText != null));
        Debug.Log("upgradeButton assigned? " + (upgradeButton != null));

        if (popupRoot != null) popupRoot.SetActive(false);
    }

    // オブジェクトから呼び出し（対象名と表示文言を受け取る）
    public void ShowPopup(string facilityName, string message)
    {
        Debug.Log("ShowPopup called on: " + gameObject.name);

        if (this == null) Debug.LogError("PopupManager THIS is null!");
        if (popupRoot == null) Debug.LogError("popupRoot is NULL in ShowPopup!");
        if (messageText == null) Debug.LogError("messageText is NULL in ShowPopup!");
        if (upgradeButton == null) Debug.LogError("upgradeButton is NULL in ShowPopup!");

        if (upgradeButton != null) upgradeButton.facilityName = facilityName;
        if (messageText != null) messageText.text = message;
        if (popupRoot != null) popupRoot.SetActive(true);
    }

    public void ClosePopup()
    {
        popupRoot.SetActive(false);
    }
}