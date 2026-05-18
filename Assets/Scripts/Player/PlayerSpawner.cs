using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private IngameCamera _camera;
    #endregion

    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        if (_playerData == null || _playerData.PlayerPrefab == null)
        {
            Debug.LogError("PlayerSpawner null 확인 필요");
            return;
        }

        GameObject playerObj = Instantiate(_playerData.PlayerPrefab, transform.position, transform.rotation);

        Player player = playerObj.GetComponent<Player>();

        if (player != null)
        {
            player.Init();

            if (_camera != null)
            {
                _camera.SetTarget(player.transform);
            }
        }

        else
        {
            Debug.LogError("player = null 확인 필요");
        }
    }
}