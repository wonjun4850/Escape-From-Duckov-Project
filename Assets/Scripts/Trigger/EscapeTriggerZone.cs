using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTriggerZone : MonoBehaviour // UI랑 나중에 연결해주자
{
    #region 인스펙터
    [SerializeField] private float _escapeTime = 10f;
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
        if (_onlyOnce && _isEscaped)
        {
            return;
        }

        if (other.CompareTag(_playerTag))
        {
            // UI 켜주는 코드 추가하면 될듯??
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_onlyOnce && _isEscaped)
        {
            return;
        }

        if (other.CompareTag(_playerTag))
        {
            // 켜둔 UI 시간 갱신 시켜야할듯?

            _timer += Time.deltaTime;

            if (_timer >= _escapeTime)
            {
                _isEscaped = true;
                Debug.Log("탈출 성공!");
                // 결과씬으로 넘어가는 코드 추가해야함
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_onlyOnce && _isEscaped)
        {
            return;
        }

        if (other.CompareTag(_playerTag))
        {
            _timer = 0f;

            // UI 꺼주는 코드 추가하면 될듯??
        }
    }
}