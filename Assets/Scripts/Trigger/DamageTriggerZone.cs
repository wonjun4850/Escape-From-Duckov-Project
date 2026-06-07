using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTriggerZone : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private float _damageAmount = 3f;
    [SerializeField] private float _damageInterval = 0.5f;
    #endregion

    #region 내부 변수
    private float _nextTick = 0f;
    #endregion

    private void OnTriggerStay(Collider other)
    {
        if (Time.time >= _nextTick)
        {
            if (other.TryGetComponent<HpSystem>(out HpSystem hp))
            {
                hp.TakeDamage(_damageAmount);

                _nextTick = Time.time + _damageInterval;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _nextTick = 0f;
    }
}
