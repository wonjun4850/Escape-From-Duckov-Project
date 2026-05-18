using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPortal : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private string _sceneId = "";
    [SerializeField] private string _nextActionMap = "";
    [SerializeField] private bool _clickDirection = false;
    [SerializeField] private bool _onlyOnce = true;

    [SerializeField] private PlayerDataSO _playerDataSO;
    #endregion

    #region 내부 변수
    private bool _isActive = false;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (_onlyOnce && _isActive)
        {
            return;
        }

        if (other.CompareTag(_playerTag))
        {
            _isActive = true;

            if (_sceneId == "Base1")
            {
                SceneLoader.Instance.ShowResultUI(_playerDataSO, 3333, true);
            }

            else
            {
                SceneLoader.Instance.LoadScene(_sceneId, _nextActionMap, _clickDirection);
            }
        }
    }
}
