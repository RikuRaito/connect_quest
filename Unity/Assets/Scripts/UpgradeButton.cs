using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public FacilityManager facilityManager;  // インスペクターでアタッチ
    public string facilityName;              // このボタンが担当する施設名

    // ボタンの OnClick で呼ばれる
    public void OnClickUpgrade()
    {
        // facilityManager.UpgradeFacility(facilityName);
    }
}
