using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTriggerZone : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private float _escapeTime = 6f;
    [SerializeField] private string _playerTag = "Player";
    [SerializeField] private bool _onlyOnce = true;
    #endregion

    #region 내부 변수
    private bool _isEscaped = false;
    private float _timer = 0f;
    #endregion

    private void Reset()
    {
        Collider _col = GetComponent<Collider>();

        if (_col != null)
        {
            _col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_playerTag))
        {
            return;
        }

        if (_onlyOnce && _isEscaped)
        {
            return;
        }

        _timer = _escapeTime;

        IngameUIManager.Instance.StartEscapeTimer(_escapeTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(_playerTag))
        {
            return;
        }

        if (_onlyOnce && _isEscaped)
        {
            return;
        }

        _timer -= Time.deltaTime;

        IngameUIManager.Instance.UpdateEscapeTimer(_timer);

        if (_timer <= 0f)
        {
            _isEscaped = true;

            IngameUIManager.Instance.StopEscapeTimer();
            GameManager.Instance.OnPlayerEscape();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(_playerTag))
        {
            return;
        }

        if (_onlyOnce && _isEscaped)
        {
            return;
        }

        _timer = _escapeTime;

        IngameUIManager.Instance.StopEscapeTimer();
    }
}