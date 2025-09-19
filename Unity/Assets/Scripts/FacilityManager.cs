using UnityEngine;
using System.Collections.Generic;

public class FacilityManager : MonoBehaviour
{
    // シングルトン
    public static FacilityManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ----------------------------
    // データ定義
    // ----------------------------

    [System.Serializable]
    public class FacilityLevel
    {
        public GameObject prefab;
        public Vector3 scale = Vector3.one;
        public Vector3 position = Vector3.zero;
    }

    [System.Serializable]
    public class FacilityPrefabSet
    {
        public string name;              // training / school / restaurant / ...
        public FacilityLevel[] levels;   // [0] = レベル0, [1] = レベル1, ...
    }

    public List<FacilityPrefabSet> facilityPrefabs; // Inspectorで設定

    // 施設の現在レベル
    private Dictionary<string, int> currentLevels = new Dictionary<string, int>();

    // シーン上に存在する施設オブジェクト
    private Dictionary<string, GameObject> currentInstances = new Dictionary<string, GameObject>();

    // ----------------------------
    // 初期化
    // ----------------------------

    void Start()
    {
        // 全施設をレベル0で生成
        foreach (var prefabSet in facilityPrefabs)
        {
            currentLevels[prefabSet.name] = 0;
            SetFacilityLevel(prefabSet.name, 0);
        }

#if UNITY_EDITOR
        Debug.Log("FacilityManager: 全施設をレベル0から配置しました。");
#endif
    }

    // ----------------------------
    // アップグレード処理
    // ----------------------------

    public void UpgradeFacility(string facilityName)
    {
        if (!currentLevels.ContainsKey(facilityName))
        {
            Debug.LogWarning($"Facility {facilityName} が存在しません");
            return;
        }

        int nextLevel = currentLevels[facilityName] + 1;

        FacilityPrefabSet prefabSet = facilityPrefabs.Find(f => f.name == facilityName);
        if (prefabSet == null)
        {
            Debug.LogWarning($"PrefabSet が見つかりません: {facilityName}");
            return;
        }

        if (nextLevel >= prefabSet.levels.Length)
        {
            Debug.LogWarning($"{facilityName} は最大レベルに到達しています");
            return;
        }

        currentLevels[facilityName] = nextLevel;
        SetFacilityLevel(facilityName, nextLevel);

        Debug.Log($"{facilityName} を Lv.{nextLevel} にアップグレードしました");
    }

    // ----------------------------
    // レベル設定（生成 & 置き換え）
    // ----------------------------

    private void SetFacilityLevel(string baseName, int level)
    {
        // 古いオブジェクトがあれば削除
        if (currentInstances.ContainsKey(baseName))
        {
            if (currentInstances[baseName] != null)
            {
                Destroy(currentInstances[baseName]);
            }
            currentInstances.Remove(baseName);
        }

        FacilityPrefabSet prefabSet = facilityPrefabs.Find(f => f.name == baseName);
        if (prefabSet == null || level < 0 || level >= prefabSet.levels.Length)
        {
            Debug.LogWarning($"Facility {baseName} のレベル {level} は不正です");
            return;
        }

        FacilityLevel facilityLevel = prefabSet.levels[level];
        if (facilityLevel.prefab == null)
        {
            Debug.LogWarning($"{baseName} Lv.{level} のPrefabが未設定です");
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

        currentInstances[baseName] = instance;
    }

    // ----------------------------
    // 現在レベルの取得
    // ----------------------------

    public int GetFacilityLevel(string facilityName)
    {
        if (currentLevels.ContainsKey(facilityName))
            return currentLevels[facilityName];
        return -1;
    }
}