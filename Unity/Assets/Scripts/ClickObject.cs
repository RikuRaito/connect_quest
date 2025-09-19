using UnityEngine;

public class ClickObject : MonoBehaviour
{
    public string facilityName;

    void OnMouseDown()
    {
        Debug.Log("Tapped: " + facilityName);
        PopupManager.Instance.ShowPopup(
            facilityName,
            $"Upgrade {facilityName}?"
        );
    }
}