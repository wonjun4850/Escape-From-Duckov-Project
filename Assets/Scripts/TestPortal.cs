using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPortal : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private bool _onlyOnce = true;

    [SerializeField] private string _nextSceneID = "";
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

            GameManager.Instance.MoveNextScene(_nextSceneID, "Ingame");
        }
    }
}
