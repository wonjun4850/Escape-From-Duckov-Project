using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameCamera : MonoBehaviour
{
    #region 인스펙터
    [Header("카메라 설정")]
    [SerializeField] private Vector3 _positionOffset = new Vector3(6, 20, -10);
    [SerializeField] private float _mouseOffset = 5f;
    [SerializeField] private float _smoothSpeed = 5f;

    [Header("반동 설정")]
    [SerializeField] private float _duration = 0.05f;
    [SerializeField] private float _recoilOffset = 0.15f;

    [Header("조준 설정")]
    [SerializeField] private float _normalFOV = 40f;
    [SerializeField] private float _aimFOV = 35f;
    [SerializeField] private float _zoomSpeed = 5f;
    #endregion

    #region 내부 변수
    private Transform _playerTr;
    private Weapon _activeWeapon;
    private Camera _camera;

    private Vector3 _shakeOffset = Vector3.zero;
    private float _targetFOV;
    #endregion

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        if (_camera != null)
        {
            _camera.fieldOfView = _normalFOV;
            _targetFOV = _normalFOV;
        }
    }

    private void LateUpdate()
    {
        if (_playerTr == null) return;

        if (_camera != null)
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFOV, Time.deltaTime * _zoomSpeed);
        }

        Vector2 recoilOffset = Vector2.zero;

        if (_activeWeapon != null && _activeWeapon.Recoil != null)
        {
            recoilOffset = _activeWeapon.Recoil.CurrentRecoilOffset;
        }

        float mouseX = ((Input.mousePosition.x + recoilOffset.x) / Screen.width) * 2f - 1f;
        float mouseY = ((Input.mousePosition.y + recoilOffset.y) / Screen.height) * 2f - 1f;

        Vector3 camF = Vector3.ProjectOnPlane(this.transform.forward, Vector3.up).normalized;
        Vector3 camR = Vector3.ProjectOnPlane(this.transform.right, Vector3.up).normalized;

        Vector3 mouseMove = (camR * mouseX + camF * mouseY) * _mouseOffset;

        Vector3 targetPos = _playerTr.position + _positionOffset + mouseMove + _shakeOffset;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _smoothSpeed);
    }

    private IEnumerator CoShake()
    {
        float timer = 0f;

        while (timer < _duration)
        {
            timer += Time.deltaTime;

            float moveX = Random.Range(-1f, 1f) * _recoilOffset;
            float moveZ = Random.Range(-1f, 1f) * _recoilOffset;

            _shakeOffset = new Vector3(moveX, 0, moveZ);

            yield return null;
        }

        _shakeOffset = Vector3.zero;
    }

    #region 외부 호출 함수
    public void SetTarget(Transform target)
    {
        _playerTr = target;
        transform.position = _playerTr.position + _positionOffset;
        transform.LookAt(_playerTr.position);
    }

    public void ActiveWeapon(Weapon weapon)
    {
        _activeWeapon = weapon;
    }

    public void CameraShake()
    {
        StopAllCoroutines();
        StartCoroutine(CoShake());
    }

    public void SetAimToFOV(bool isAiming)
    {
        _targetFOV = isAiming ? _aimFOV : _normalFOV;
    }
    #endregion
}