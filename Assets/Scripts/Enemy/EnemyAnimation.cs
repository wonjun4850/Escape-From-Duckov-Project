using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private string _paramSpeed = "fSpeed";
    [SerializeField] private string _paramMoveX = "fMoveX";
    [SerializeField] private string _paramMoveY = "fMoveY";
    [SerializeField] private string _paramDodge = "tDodge";
    [SerializeField] private string _paramAttack = "tAttack";

    [SerializeField] private float _stepCoolDown = 0.2f;
    [SerializeField] private bool _isBoss = false;
    #endregion

    #region 내부 변수
    private Animator _anim;

    private int _hashSpeed;
    private int _hashMoveX;
    private int _hashMoveY;
    private int _hashDodge;
    private int _hashAttack;

    private float _lastEventTime;
    #endregion

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("EnemyAnimation 애니메이터 컴포넌트 없음");
            return;
        }

        _hashSpeed = Animator.StringToHash(_paramSpeed);
        _hashMoveX = Animator.StringToHash(_paramMoveX);
        _hashMoveY = Animator.StringToHash(_paramMoveY);
        _hashDodge = Animator.StringToHash(_paramDodge);
        _hashAttack = Animator.StringToHash(_paramAttack);
    }

    #region 외부 호출 함수
    public void Move(float x, float y, float speed)
    {
        _anim.SetFloat(_hashMoveX, x, 0.1f, Time.deltaTime);
        _anim.SetFloat(_hashMoveY, y, 0.1f, Time.deltaTime);
        _anim.SetFloat(_hashSpeed, speed);
    }

    public void Dodge()
    {
        if (!_isBoss)
        {
            return;
        }

        _anim.SetTrigger(_hashDodge);
    }

    public void Attack()
    {
        _anim.SetTrigger(_hashAttack);
    }
    #endregion

    #region 애니메이션 이벤트 함수
    public void PlayBossDodgeSound()
    {
        // 보스 구르기 소리 재생(3D)
    }

    public void PlayEnemyFootStepSound()
    {
        if (Time.time - _lastEventTime < _stepCoolDown) return;

        float moveX = _anim.GetFloat(_hashMoveX);
        float moveY = _anim.GetFloat(_hashMoveY);

        if (Mathf.Abs(moveX) < 0.1f && Mathf.Abs(moveY) < 0.1f) return;

        // 일반 적 발소리 재생(3D)

        _lastEventTime = Time.time;
    }

    public void PlayBossFootStepSound()
    {
        if (Time.time - _lastEventTime < _stepCoolDown) return;

        float moveX = _anim.GetFloat(_hashMoveX);
        float moveY = _anim.GetFloat(_hashMoveY);

        if (Mathf.Abs(moveX) < 0.1f && Mathf.Abs(moveY) < 0.1f) return;

        // 보스 발소리 재생(3D)

        _lastEventTime = Time.time;
    }
    #endregion
}
