using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private string _paramSpeed = "fSpeed";
    [SerializeField] private string _paramMoveX = "fMoveX";
    [SerializeField] private string _paramMoveY = "fMoveY";
    [SerializeField] private string _paramDodge = "tDodge";

    [SerializeField] private float _stepCoolDown = 0.2f;
    #endregion

    #region 내부 변수
    private Animator _anim;

    private int _hashSpeed;
    private int _hashMoveX;
    private int _hashMoveY;
    private int _hashDodge;

    private float _lastEventTime;
    #endregion

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("PlayerAnimation 애니메이터 컴포넌트 없음");
            return;
        }

        _hashSpeed = Animator.StringToHash(_paramSpeed);
        _hashMoveX = Animator.StringToHash(_paramMoveX);
        _hashMoveY = Animator.StringToHash(_paramMoveY);
        _hashDodge = Animator.StringToHash(_paramDodge);
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
        _anim.SetTrigger(_hashDodge);
    }
    #endregion

    #region 애니메이션 이벤트 함수
    public void PlayDodgeSound()
    {
        SoundManager.Instance.PlayRandomSFX("Dodge", 3);
    }
    
    public void PlayFootStepSound()
    {
        if (Time.time - _lastEventTime < _stepCoolDown) return;

        float moveX = _anim.GetFloat(_hashMoveX);
        float moveY = _anim.GetFloat(_hashMoveY);

        if (Mathf.Abs(moveX) < 0.1f && Mathf.Abs(moveY) < 0.1f) return;

        SoundManager.Instance.PlayRandomSFX("Player_FootStep", 5);

        _lastEventTime = Time.time;
    }
    #endregion


    /// <summary>
    /// 아래는 테스트용도
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeWeaponMode(true);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeWeaponMode(false);
    }
    private void ChangeWeaponMode(bool b)
    {
        if (b)
        {
            _anim.SetLayerWeight(1, 0f);
        }

        else
        {
            _anim.SetLayerWeight(1, 1f);
        }
    }
}