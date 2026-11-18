using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainer : MonoBehaviour, IInteractable
{
    #region 인터페이스 구현
    public void Interact()
    {
        if (LootManager.Instance != null)
        {
            LootManager.Instance.OpenContainer(this);
        }
    }

    public string ObjectName => _objectName;
    #endregion

    public enum EObjectType
    {
        Normal,
        Corpse
    }

    #region 인스펙터
    [Header("오브젝트 설정")]
    [SerializeField] private string _objectName = "";
    [SerializeField] private EObjectType _type = EObjectType.Normal;
    [SerializeField] private int _minRandomLootCount = 2;
    [SerializeField] private int _maxRandomLootCount = 6;

    [Header("참조 SO")]
    [SerializeField] private LootTableSO _lootTableSO;

    [Header("보유 아이템")]
    [SerializeField] private List<GeneratedLoot> _generatedItem = new List<GeneratedLoot>();
    #endregion

    private void Awake()
    {
        ContainerSetting();
    }

    private void ContainerSetting()
    {
        switch (_type)
        {
            case EObjectType.Normal:
                GenerateRandomItem();
                break;

            case EObjectType.Corpse:
                GenerateRandomItem();
                GenerateFixedItem();
                break;

            default:
                Debug.LogError("컨테이너 타입 확인 필요");
                break;
        }
    }

    private void GenerateRandomItem()
    {
        int randomKind = Random.Range(_minRandomLootCount, _maxRandomLootCount + 1);

        for (int i = 0; i < randomKind; i++)
        {
            if (!LootGenerator.TryGenerateRandomLoot(_lootTableSO, out GeneratedLoot randomLoot))
            {
                continue;
            }

            _generatedItem.Add(randomLoot);
        }
    }

    private void GenerateFixedItem()
    {
        if (!LootGenerator.TryGenerateFixedLoot(_lootTableSO, out List<GeneratedLoot> fixedLootList))
        {
            return;
        }

        _generatedItem.AddRange(fixedLootList);
    }
}