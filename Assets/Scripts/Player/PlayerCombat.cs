using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Weapon _currentWeapon;
    #endregion

    #region 내부 변수
    //private Weapon _currentWeapon;
    private PlayerMovement _playerMovement;
    private IngameCamera _camera;
    private bool _isAiming = false;
    private bool _isFiring = false;
    #endregion

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();

        if (_playerMovement == null)
        {
            Debug.LogError("PlayerCombat 겟컴포넌트 오류 : PlayerMovement 컴포넌트 없음");
            return;
        }
    }

    void Start()
    {
        InputCommandHub.Instance.RegisterValueCommands<bool>("Player.Fire", new ValueCommand<bool>(OnFire));
        InputCommandHub.Instance.RegisterValueCommands<bool>("Player.Aim", new ValueCommand<bool>(OnAim));
        InputCommandHub.Instance.RegisterCommands("Player.Reload", new SimpleCommand(OnReload));

        if (Camera.main != null)
        {
            _camera = Camera.main.GetComponent<IngameCamera>();
        }

        if (_currentWeapon != null)
        {
            _currentWeapon.Init(_currentWeapon.WeaponData, this.gameObject);

            if (CursorManager.Instance != null)
            {
                CursorManager.Instance.SetActiveWeaponToCrosshair(_currentWeapon);
            }
        }
    }

    void Update()
    {
        if (_isFiring)
        {
            if (_playerMovement != null && (_playerMovement.IsDodging || _playerMovement.IsRunning))
            {
                return;
            }

            bool isHeadShot;

            Vector3 targetPos = GetMouseWorldPosition(out isHeadShot);

            if (_currentWeapon.WeaponData.IsAutomatic)
            {
                _currentWeapon.TryFire(targetPos, isHeadShot);
            }

            else
            {
                _isFiring = false;

                _currentWeapon.TryFire(targetPos, isHeadShot);
            }
        }
    }

    private void OnFire(bool isPress)
    {
        if (_currentWeapon == null)
        {
            return;
        }

        _isFiring = isPress;
    }

    private void OnAim(bool isPress)
    {
        if (_currentWeapon == null)
        {
            return;
        }

        _isAiming = isPress;

        _currentWeapon.SetAimState(_isAiming);

        if (_camera != null)
        {
            _camera.SetAimToFOV(_isAiming);
        }

        if (CursorManager.Instance != null)
        {
            CursorManager.Instance.SetAimStateToCrosshair(_isAiming);
        }
    }

    private Vector3 GetMouseWorldPosition(out bool isHeadShot)
    {
        isHeadShot = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (hit.collider.CompareTag("EnemyHead"))
            {
                isHeadShot = true;
            }

            return hit.point;
        }

        return transform.position + transform.forward * 10f;
    }

    private void OnReload()
    { 
        _currentWeapon.TryReload();
    }

    #region 외부 호출 함수
    public void ChangeWeapon(Weapon newWeapon)
    {
        if (CursorManager.Instance != null)
        {
            CursorManager.Instance.SetActiveWeaponToCrosshair(_currentWeapon);
        }

        _currentWeapon = newWeapon;

        if (_currentWeapon != null)
        {
            _currentWeapon.SetAimState(_isAiming);
        }
    }
    #endregion
}