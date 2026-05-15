using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region 인스펙터
    [Header("회전 설정")]
    [SerializeField] private float _rotateSharpness = 15f;
    #endregion

    #region 내부 변수
    private PlayerAnimation _playerAnimation;
    private StaminaSystem _staminaSystem;

    private Rigidbody _rb;
    private Vector2 _moveInput;

    private float _moveSpeed;
    private float _runMultiplier;
    private float _dodgeForce;
    private float _dodgeDuration;

    private bool _isInit = false;
    private bool _requestRun = false;
    private bool _requestDodge = false;
    private bool _isRunning = false;
    private bool _isDodging = false;
    #endregion

    private void Awake()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        _staminaSystem = GetComponent<StaminaSystem>();
        _rb = GetComponent<Rigidbody>();

        if (_playerAnimation == null || _staminaSystem == null)
        {
            Debug.LogError("PlayerMovement 겟컴포넌트 오류 : 인스펙터 확인");
            return;
        }

        if (_rb == null)
        {
            Debug.LogError("플레이어 리지드바디 없음");
            return;
        }
    }

    void Start()
    {
        InputCommandHub.Instance.RegisterValueCommands<Vector2>("Player.Move", new ValueCommand<Vector2>(OnMove));
        InputCommandHub.Instance.RegisterValueCommands<bool>("Player.Run", new ValueCommand<bool>(OnRun));
        InputCommandHub.Instance.RegisterCommands("Player.Dodge", new SimpleCommand(OnDodge));
    }

    private void FixedUpdate()
    {
        if (!_isInit)
        {
            return;
        }

        Move();
        TryDodge();

        if (_isRunning)
        {
            _staminaSystem.ConsumeStaminaByRun();
        }
    }

    private void OnDisable()
    {
        _isDodging = false;
    }

    private void OnMove(Vector2 v)
    {
        _moveInput = v;
    }

    private void OnRun(bool isPress)
    {
        _requestRun = isPress;
    }

    private void OnDodge()
    {
        if (_isDodging)
        {
            return;
        }

        _requestDodge = true;
    }

    private void Move()
    {
        if (_isDodging)
        {
            return;
        }

        Vector3 camF = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 camR = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;

        Vector3 direction = (camR * _moveInput.x + camF * _moveInput.y).normalized;

        if (direction.sqrMagnitude < 0.001f)
        {
            direction = Vector3.zero;
        }

        UpdateRunningState(direction);
 
        Vector3 localDir = transform.InverseTransformDirection(direction);

        float directionMultiplier = 1.0f;

        if (localDir.z >= 0)
        {
            directionMultiplier = Mathf.Lerp(0.9f, 1.0f, localDir.z);
        }
        else
        {
            directionMultiplier = Mathf.Lerp(0.9f, 0.7f, Mathf.Abs(localDir.z));
        }

        float speed = _isRunning ? _moveSpeed * _runMultiplier : _moveSpeed;
        speed *= directionMultiplier;

        Vector3 velocity = direction * speed;
        _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);

        ApplyMoveAnimation(direction);

        if (_isRunning && direction.sqrMagnitude > 0.001f)
        {
            TickRotate(direction);
        }

        else
        {
            RotateToMouse();
        }
    }

    private void TickRotate(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion target = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.Slerp
            (
            transform.rotation,
            target,
            1.0f - Mathf.Exp(-_rotateSharpness / 4f * Time.fixedDeltaTime)
            );
    }

    private void RotateToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float hitDistance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(hitDistance);

            Vector3 lookDir = (mouseWorldPos - transform.position);
            lookDir.y = 0;

            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    1.0f - Mathf.Exp(-_rotateSharpness * Time.fixedDeltaTime)
                );
            }
        }
    }

    private void ApplyMoveAnimation(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
        {
            _playerAnimation.Move(0f, 0f, 1.0f);
            return;
        }

        Vector3 localDir = transform.InverseTransformDirection(direction);

        float animSpeed = _isRunning ? _runMultiplier : 1.0f;
        _playerAnimation.Move(localDir.x, localDir.z, animSpeed);
    }

    private void TryDodge()
    {
        if (!_requestDodge || _isDodging || !_staminaSystem.CanDodge())
        {
            return;
        }

        _staminaSystem.ConsumeStaminaByDodge();
        StartCoroutine(CoDodgeRoutine());
    }

    private IEnumerator CoDodgeRoutine()
    {
        _isDodging = true;
        _requestDodge = false;

        Vector3 camF = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 camR = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;

        Vector3 inputDir = (camR * _moveInput.x + camF * _moveInput.y).normalized;
        Vector3 dodgeDir = inputDir.sqrMagnitude > 0.001f ? inputDir : transform.forward;

        transform.rotation = Quaternion.LookRotation(dodgeDir);

        _playerAnimation.Dodge();

        float timer = 0f;

        while (timer < _dodgeDuration)
        {
            timer += Time.fixedDeltaTime;

            float process = timer / _dodgeDuration;

            float curve = (1f - process); // * (1f - process)

            Vector3 velocity = dodgeDir * (_dodgeForce * curve);
            _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);

            yield return new WaitForFixedUpdate();
        }

        yield return null;

        _rb.velocity = new Vector3(0, _rb.velocity.y, 0);

        _isDodging = false;
    }

    private void UpdateRunningState(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
        {
            _isRunning = false;
            return;
        }

        if (_requestRun)
        {
            if (!_isRunning)
            {
                if (_staminaSystem.CanRunStart())
                {
                    _isRunning = true;
                }
            }

            else
            {
                if (!_staminaSystem.CanRun())
                {
                    _isRunning = false;
                }
            }
        }

        else
        {
            _isRunning = false;
        }
    }

    #region 외부 호출 함수
    public void Init(float basespeed, float runMult, float dodgeForce, float dodgeDuration)
    {
        _moveSpeed = basespeed;
        _runMultiplier = runMult;
        _dodgeForce = dodgeForce;
        _dodgeDuration = dodgeDuration;
        _isInit = true;
    }
    #endregion
}