using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class FacilityManager : MonoBehaviour
{
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
        int maxLevel = 3;
        for (int i = 0; i <= maxLevel; i++)
        {
            string objName;
            if (i == 0)
            {
                objName = baseName + "UnderBuilding";
            }
            else
            {
                objName = baseName + "Lv" + i;
            }

            GameObject obj = GameObject.Find(objName);
            if (obj != null)
            {
                obj.SetActive(i == level);
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
