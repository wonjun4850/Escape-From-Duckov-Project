using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

[System.Serializable]
public struct SheetData
{
    public string SheetName;
    public string Gid;

    public SheetData(string sheetName, string gid)
    {
        SheetName = sheetName;
        Gid = gid;
    }
}

public class GoogleSheetImport : EditorWindow
{
    private const string BASE_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQ9_X0YopR5WResbvdjqJLv_btWfhNwAps5XHfFI5MNJgYm6AWa_R5qMWZeGrjVOpTPa6r4LxydSnJq/pub?";
    private const string FOLDER_PATH = "Assets/Resources/ItemData";

    private SheetData[] _sheets = new SheetData[]
    {
        new SheetData ("BaseItem",       "0"),
        new SheetData ("WeaponItem",     "1063210392"),
        new SheetData ("ArmorItem",      "1570652292"),
        new SheetData ("TotemItem",      "2021464201"),
        new SheetData ("AmmoItem",       "1315433096"),
        new SheetData ("ConsumableItem", "705429197"),
        new SheetData ("MaterialItem",   "1969809158")
    };


    [MenuItem("Tools/Google Sheet Import")]
    public static void ShowWindow()
    {
        GetWindow<GoogleSheetImport>("Google SheetImport");
    }

    private void OnGUI()
    {
        GUILayout.Label("구글 시트 임포트", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("시트 전체 SO 생성", GUILayout.Height(30)))
        {
            ImportAllAsync();
        }
    }

    private async void ImportAllAsync()
    {
        try
        {
            foreach (var sheet in _sheets)
            {
                string url = $"{BASE_URL}gid={sheet.Gid}&single=true&output=tsv";

                await ParseAsync(sheet.SheetName, url);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("아이템 데이터 파싱 완료");
        }

        catch (Exception ex)
        {
            Debug.LogError($"오류 발생: {ex}");
        }
    }

    private async UniTask ParseAsync(string sheetName, string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"[{sheetName}] 데이터 다운로드 실패: {www.error}");
            }

            ParseTsv(sheetName, www.downloadHandler.text);
        }
    }

    private void ParseTsv(string sheetName, string tsv)
    {
        string[] lines = tsv.Split('\n');

        if (lines.Length <= 1)
        {
            return;
        }

        if (!Directory.Exists(FOLDER_PATH))
        {
            Directory.CreateDirectory(FOLDER_PATH);
        }

        switch (sheetName)
        {
            case "BaseItem":
                ParseBaseItem(lines);
                break;

            case "WeaponItem":
                ParseWeaponItem(lines);
                break;

            case "ArmorItem":
                ParseArmorItem(lines);
                break;

            case "TotemItem":
                ParseTotemItem(lines);
                break;

            case "AmmoItem":
                ParseAmmoItem(lines);
                break;

            case "ConsumableItem":
                ParseConsumableItem(lines);
                break;

            case "MaterialItem":
                ParseMaterialItem(lines);
                break;

            default:
                Debug.LogWarning($"알 수 없는 시트 이름: {sheetName}");
                return;
        }
    }

    private void ParseBaseItem(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split('\t');

            if (string.IsNullOrWhiteSpace(fields[0]))
            {
                continue;
            }

            string itemType = fields[2].Trim();
            BaseItemDataSO itemSO = null;

            switch (itemType)
            {
                case "Weapon":
                    itemSO = ScriptableObject.CreateInstance<WeaponItemDataSO>();
                    break;

                case "Armor":
                    itemSO = ScriptableObject.CreateInstance<ArmorItemDataSO>();
                    break;

                case "Totem":
                    itemSO = ScriptableObject.CreateInstance<TotemItemDataSO>();
                    break;

                case "Ammo":
                    itemSO = ScriptableObject.CreateInstance<AmmoItemDataSO>();
                    break;

                case "Consumable":
                    itemSO = ScriptableObject.CreateInstance<ConsumableItemDataSO>();
                    break;

                case "Material":
                    itemSO = ScriptableObject.CreateInstance<MaterialItemDataSO>();
                    break;

                default:
                    Debug.LogWarning($"알 수 없는 아이템 타입: {itemType}");
                    break;
            }

            if (itemSO != null)
            {
                itemSO.BaseInit
                    (
                    fields[0].Trim(),
                    fields[1].Trim(),
                    Enum.TryParse(fields[2].Trim(), out BaseItemDataSO.EType parsedType) ? parsedType : BaseItemDataSO.EType.None,
                    fields[3].Trim(),
                    int.TryParse(fields[4].Trim(), out int price) ? price : 0,
                    float.TryParse(fields[5].Trim(), out float weight) ? weight : 0f,
                    int.TryParse(fields[6].Trim(), out int maxStackSize) ? maxStackSize : 1,
                    null,
                    null
                    //AssetDatabase.LoadAssetAtPath<Sprite>(fields[7].Trim()), 이미지
                    //AssetDatabase.LoadAssetAtPath<GameObject>(fields[8].Trim()) 프리팹
                    );

                string assetPath = $"{FOLDER_PATH}/{fields[0].Trim()}.asset";
                AssetDatabase.CreateAsset(itemSO, assetPath);
            }
        }
    }

    private void ParseWeaponItem(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split('\t');

            if (string.IsNullOrWhiteSpace(fields[0]))
            {
                continue;
            }

            string itemId = fields[0].Trim();

            WeaponItemDataSO weaponSO = AssetDatabase.LoadAssetAtPath<WeaponItemDataSO>($"{FOLDER_PATH}/{itemId}.asset");

            if (weaponSO != null)
            {
                weaponSO.Init
                    (
                    Enum.TryParse(fields[1].Trim(), out EquipmentItemDataSO.EEquipmentSlot equipmentSlot) ? equipmentSlot : EquipmentItemDataSO.EEquipmentSlot.None,
                    Enum.TryParse(fields[2].Trim(), out WeaponItemDataSO.EWeaponType weaponType) ? weaponType : WeaponItemDataSO.EWeaponType.None,
                    bool.TryParse(fields[3].Trim(), out bool isAutomatic) ? isAutomatic : false,
                    float.TryParse(fields[4].Trim(), out float damage) ? damage : 0f,
                    float.TryParse(fields[5].Trim(), out float effectiveRange) ? effectiveRange : 0f,
                    float.TryParse(fields[6].Trim(), out float outsideEffectiveRange) ? outsideEffectiveRange : 0f,
                    float.TryParse(fields[7].Trim(), out float fireRate) ? fireRate : 0f,
                    int.TryParse(fields[8].Trim(), out int magazinSize) ? magazinSize : 0,
                    float.TryParse(fields[9].Trim(), out float reloadTime) ? reloadTime : 0f,
                    float.TryParse(fields[10].Trim(), out float recoilX) ? recoilX : 0f,
                    float.TryParse(fields[11].Trim(), out float recoilY) ? recoilY : 0f,
                    float.TryParse(fields[12].Trim(), out float spread) ? spread : 0f,
                    float.TryParse(fields[13].Trim(), out float headshotMultiplier) ? headshotMultiplier : 1f,
                    float.TryParse(fields[14].Trim(), out float moveSpeedModifier) ? moveSpeedModifier : 1f,
                    float.TryParse(fields[15].Trim(), out float projectileSpeed) ? projectileSpeed : 0f,
                    float.TryParse(fields[16].Trim(), out float projectileLifetime) ? projectileLifetime : 0f,
                    int.TryParse(fields[17].Trim(), out int perShot) ? perShot : 0,
                    null
                    //AssetDatabase.LoadAssetAtPath<GameObject>(fields[18].Trim()) 총알 프리팹
                    );

                EditorUtility.SetDirty(weaponSO);
            }

            else
            {
                Debug.LogWarning($"{itemId} -> WeaponItemDataSO를 찾을 수 없습니다");
            }
        }
    }

    private void ParseArmorItem(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split('\t');

            if (string.IsNullOrWhiteSpace(fields[0]))
            {
                continue;
            }

            string itemId = fields[0].Trim();

            ArmorItemDataSO armorSO = AssetDatabase.LoadAssetAtPath<ArmorItemDataSO>($"{FOLDER_PATH}/{itemId}.asset");

            if (armorSO != null)
            {
                armorSO.Init
                    (
                    Enum.TryParse(fields[1].Trim(), out EquipmentItemDataSO.EEquipmentSlot equipmentSlot) ? equipmentSlot : EquipmentItemDataSO.EEquipmentSlot.None,
                    float.TryParse(fields[2].Trim(), out float armorValue) ? armorValue : 0f,
                    int.TryParse(fields[3].Trim(), out int extraInventorySlot) ? extraInventorySlot : 0,
                    float.TryParse(fields[4].Trim(), out float extraCarryWeight) ? extraCarryWeight : 0f
                    );

                EditorUtility.SetDirty(armorSO);
            }

            else
            {
                Debug.LogWarning($"{itemId} -> ArmorItemDataSO를 찾을 수 없습니다");
            }
        }
    }

    private void ParseTotemItem(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split('\t');

            if (string.IsNullOrWhiteSpace(fields[0]))
            {
                continue;
            }

            string itemId = fields[0].Trim();

            TotemItemDataSO totemSO = AssetDatabase.LoadAssetAtPath<TotemItemDataSO>($"{FOLDER_PATH}/{itemId}.asset");

            if (totemSO != null)
            {
                totemSO.Init
                    (
                    Enum.TryParse(fields[1].Trim(), out EquipmentItemDataSO.EEquipmentSlot equipmentSlot) ? equipmentSlot : EquipmentItemDataSO.EEquipmentSlot.None,
                    Enum.TryParse(fields[2].Trim(), out TotemItemDataSO.ETotemType totemType) ? totemType : TotemItemDataSO.ETotemType.None,
                    int.TryParse(fields[3].Trim(), out int totemLevel) ? totemLevel : 0,
                    int.TryParse(fields[4].Trim(), out int bonusValue) ? bonusValue : 0
                    );

                EditorUtility.SetDirty(totemSO);
            }
            else
            {
                Debug.LogWarning($"{itemId} -> TotemItemDataSO를 찾을 수 없습니다");
            }
        }
    }

    private void ParseAmmoItem(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split('\t');

            if (string.IsNullOrWhiteSpace(fields[0]))
            {
                continue;
            }

            string itemId = fields[0].Trim();

            AmmoItemDataSO ammoSO = AssetDatabase.LoadAssetAtPath<AmmoItemDataSO>($"{FOLDER_PATH}/{itemId}.asset");

            if (ammoSO != null)
            {
                ammoSO.Init
                    (
                    Enum.TryParse(fields[1].Trim(), out AmmoItemDataSO.EAmmoType ammoType) ? ammoType : AmmoItemDataSO.EAmmoType.None
                    );

                EditorUtility.SetDirty(ammoSO);
            }

            else
            {
                Debug.LogWarning($"{itemId} -> AmmoItemDataSO를 찾을 수 없습니다");
            }
        }
    }

    private void ParseConsumableItem(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split('\t');

            if (string.IsNullOrWhiteSpace(fields[0]))
            {
                continue;
            }

            string itemId = fields[0].Trim();

            ConsumableItemDataSO consumableSO = AssetDatabase.LoadAssetAtPath<ConsumableItemDataSO>($"{FOLDER_PATH}/{itemId}.asset");

            if (consumableSO != null)
            {
                consumableSO.Init
                    (
                    float.TryParse(fields[1].Trim(), out float healthRestore) ? healthRestore : 0f,
                    float.TryParse(fields[2].Trim(), out float energyRestore) ? energyRestore : 0f,
                    float.TryParse(fields[3].Trim(), out float hydrationRestore) ? hydrationRestore : 0f,
                    float.TryParse(fields[4].Trim(), out float useTime) ? useTime : 0f,
                    int.TryParse(fields[5].Trim(), out int reusableCount) ? reusableCount : 0
                    );

                EditorUtility.SetDirty(consumableSO);
            }
            else
            {
                Debug.LogWarning($"{itemId} -> ConsumableItemDataSO를 찾을 수 없습니다");
            }
        }
    }

    private void ParseMaterialItem(string[] lines)
    {
        for (int i = 1; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split('\t');

            if (string.IsNullOrWhiteSpace(fields[0]))
            {
                continue;
            }

            string itemId = fields[0].Trim();

            MaterialItemDataSO materialSO = AssetDatabase.LoadAssetAtPath<MaterialItemDataSO>($"{FOLDER_PATH}/{itemId}.asset");

            if (materialSO != null)
            {
                materialSO.Init
                    (
                    Enum.TryParse(fields[1].Trim(), out MaterialItemDataSO.EMaterialType materialType) ? materialType : MaterialItemDataSO.EMaterialType.None
                    );

                EditorUtility.SetDirty(materialSO);
            }

            else
            {
                Debug.LogWarning($"{itemId} -> MaterialItemDataSO를 찾을 수 없습니다");
            }
        }
    }
}