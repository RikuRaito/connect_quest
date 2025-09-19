using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Data.Common;

public class FacilityManager : MonoBehaviour
{
    
    [System.Serializable]
    public class FacilityLevel
    {
        public GameObject prefab;
        public Vector3 scale;
        public Vector3 position;
    }

    [System.Serializable]
    public class FacilityPrefabSet
    {
        public string name;
        public FacilityLevel[] levels;
    }

    public List<FacilityPrefabSet> facilityPrefabs;

    private Dictionary<string, GameObject> currentFacilityInstances = new Dictionary<string, GameObject>();

    [System.Serializable]
    public class FacilityData
    {
        public int training;
        public int school;
        public int restaurant;
        public int inn;
        public int gym;
        public int farm;
        public int blacksmith;
    }

    private FacilityData currentData = new FacilityData();

    public void UpgradeFacility(string facilityName)
    {
        Debug.Log("Before upgrade blacksmith lv: " + currentData.blacksmith);
        switch (facilityName)
        {
            case "training":
                currentData.training++;
                SetFacilityLevel("training", currentData.training);
                break;
            case "school":
                currentData.school++;
                SetFacilityLevel("school", currentData.school);
                break;
            case "restaurant":
                currentData.restaurant++;
                SetFacilityLevel("restaurant", currentData.restaurant);
                break;
            case "inn":
                currentData.inn++;
                SetFacilityLevel("inn", currentData.inn);
                break;
            case "gym":
                currentData.gym++;
                SetFacilityLevel("gym", currentData.gym);
                break;
            case "farm":
                currentData.farm++;
                SetFacilityLevel("farm", currentData.farm);
                break;
            case "blacksmith":
                currentData.blacksmith++;
                Debug.Log("blacksmith lv: " + currentData.blacksmith);
                SetFacilityLevel("blacksmith", currentData.blacksmith);
                break;
            default:
                Debug.LogWarning("Unknown Facility" + facilityName);
                break;
        }

    }

    public void ReceiveFacilityData(string json)
    {
        var data = JsonUtility.FromJson<FacilityData>(json);

        //swiftから渡ってきたデータをcurrentDataにコピーする
        currentData.training = data.training;
        currentData.school = data.school;
        currentData.restaurant = data.restaurant;
        currentData.inn = data.inn;
        currentData.gym = data.gym;
        currentData.farm = data.farm;
        currentData.blacksmith = data.blacksmith;

        Debug.Log("training: " + currentData.training);
        Debug.Log("school: " + currentData.school);
        Debug.Log("restaurant: " + currentData.restaurant);
        Debug.Log("inn: " + currentData.inn);
        Debug.Log("gym: " + currentData.gym);
        Debug.Log("farm: " + currentData.farm);
        Debug.Log("blacksmith: " + currentData.blacksmith);

        SetFacilityLevel("training", currentData.training);
        SetFacilityLevel("school", currentData.school);
        SetFacilityLevel("restaurant", currentData.restaurant);
        SetFacilityLevel("inn", currentData.inn);
        SetFacilityLevel("gym", currentData.gym);
        SetFacilityLevel("farm", currentData.farm);
        SetFacilityLevel("blacksmith", currentData.blacksmith);
    }

    void SetFacilityLevel(string baseName, int level)
    {
        if (currentFacilityInstances.ContainsKey(baseName))
        {
            if (currentFacilityInstances[baseName] != null)
            {
                Destroy(currentFacilityInstances[baseName]);
            }
            currentFacilityInstances.Remove(baseName);
        }

        FacilityPrefabSet prefabSet = facilityPrefabs.Find(f => f.name == baseName);
        if (prefabSet == null)
        {
            Debug.LogWarning("FacilityPrefabSet not found for: " + baseName);
            return;
        }

        if(level < 0 || level >= prefabSet.levels.Length)
        {
            Debug.LogWarning("Invalid level " + level + " for facility " + baseName);
            return;
        }

        FacilityLevel facilityLevel = prefabSet.levels[level];
        if (facilityLevel.prefab == null)
        {
            Debug.LogWarning("Prefab is null for " + baseName + " level " + level);
            return;
        }

        GameObject instance = Instantiate(
            facilityLevel.prefab,
            facilityLevel.position,
            Quaternion.identity
        );
        instance.transform.localScale = facilityLevel.scale;
        SpriteRenderer spriteRenderer = instance.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Foreground";
        }
        currentFacilityInstances[baseName] = instance;
    }
    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
