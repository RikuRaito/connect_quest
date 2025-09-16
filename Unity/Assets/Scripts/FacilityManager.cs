using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class FacilityManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void sendFacilityData();
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



    public void ReceiveFacilityData(string json)
    {
        FacilityData data = JsonUtility.FromJson<FacilityData>(json);

        Debug.Log("training: " + data.training);
        Debug.Log("school: " + data.school);
        Debug.Log("restaurant: " + data.restaurant);
        Debug.Log("inn: " + data.inn);
        Debug.Log("gym: " + data.gym);
        Debug.Log("farm: " + data.farm);
        Debug.Log("blacksmith: " + data.blacksmith);

        SetFacilityLevel("training", data.training);
        SetFacilityLevel("school", data.school);
        SetFacilityLevel("restaurant", data.restaurant);
        SetFacilityLevel("inn", data.inn);
        SetFacilityLevel("gym", data.gym);
        SetFacilityLevel("farm", data.farm);
        SetFacilityLevel("blacksmith", data.blacksmith);
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
        sendFacilityData();
    }


    void Update()
    {
        
    }
}
