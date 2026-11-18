using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    #region 내부 변수
    public static LootManager Instance {  get; private set; }
    private LootContainer _currentContainer = null;
    private Player _player = null;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        if (_currentContainer == null || _player == null)
        {
            return;
        }

        if (!_player.Interaction.IsWithinInteractionRange(_currentContainer.transform))
        {
            CloseContainer();
        }
    }

    #region 외부 호출 함수
    public void OpenContainer(LootContainer container)
    {
        if (container == null)
        {
            return;
        }

        if (_currentContainer != null)
        {
            return;
        }

        _currentContainer = container;

        // UI 에게 요청하기
    }

    public void CloseContainer()
    {
        if (_currentContainer == null)
        {
            return;
        }

        _currentContainer = null;

        // UI 에게 요청하기
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }
    #endregion
}