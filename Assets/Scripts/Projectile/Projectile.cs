using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private bool _onlyOnce = true;
    #endregion

    #region 내부 변수
    private Rigidbody _rb;
    private Coroutine _returnRoutine;
    private bool _isDamaged = false;
    private bool _isSetup = false;

    private float _baseDamage;
    private float _lifeTime;
    private float _effectiveRange;
    private float _damageOutsideRangeModifier;
    private Vector3 _startPosition;
    private bool _isHeadShot;
    private float _headshotMultiplier;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Reset()
    {
        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isSetup)
        {
            return;
        }

        if (_onlyOnce && _isDamaged)
        {
            return;
        }

        if ((_targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            _isDamaged = true;

            float distance = Vector3.Distance(_startPosition, transform.position);
            float finalDamage = _baseDamage;

            if (distance > _effectiveRange)
            {
                finalDamage = _baseDamage * _damageOutsideRangeModifier;
            }

            if (_isHeadShot)
            {
                finalDamage *= _headshotMultiplier;
            }

            other.GetComponent<HpSystem>()?.TakeDamage(finalDamage);

            // 적에게 맞았을 때의 효과 (파티클, 사운드 등) 추가
            ReturnToPool();
        }

        else
        {
            // 벽, 장애물 등에 맞았을 때의 효과 (파티클, 사운드 등) 추가
            ReturnToPool();
        }


    }

    private IEnumerator CoReturnToPool()
    {
        yield return new WaitForSeconds(_lifeTime);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        _isSetup = false;

        if (_returnRoutine != null)
        {
            StopCoroutine(_returnRoutine);
            _returnRoutine = null;
        }

        ProjectileManager.Instance.Despawn(this.gameObject);
    }

    #region 외부 호출 함수
    public void Setup
        (float damage, float lifeTime, float effectiveRange, float damageOutsideRangeModifier, float speed, Vector3 startPosition, bool isHeadShot, float headshotMultiplier)
    {
        _baseDamage = damage;
        _lifeTime = lifeTime;
        _effectiveRange = effectiveRange;
        _damageOutsideRangeModifier = damageOutsideRangeModifier;
        _startPosition = startPosition;
        _isHeadShot = isHeadShot;
        _headshotMultiplier = headshotMultiplier;
        _isDamaged = false;
        _isSetup = true;

        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;

            _rb.velocity = transform.forward * speed;
        }

        if (_returnRoutine != null)
        {
            StopCoroutine(_returnRoutine);
        }

        _returnRoutine = StartCoroutine(CoReturnToPool());
    }
    #endregion
}