using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CrosshairControl : MonoBehaviour
{
    #region 인스펙터
    [Header("크로스헤어 UI 요소")]
    [SerializeField] private CanvasGroup _center;
    [SerializeField] private RectTransform _up;
    [SerializeField] private RectTransform _down;
    [SerializeField] private RectTransform _left;
    [SerializeField] private RectTransform _right;

    [Header("점 페이드 인아웃 시간설정")]
    [SerializeField] private float _centerFadeInOutDuration = 1f;

    [Header("Aim 설정")]
    [SerializeField] private float _aimSpeed = 10f;
    [SerializeField] private float _minSizeMult = 0.5f;

    [Header("연출 튜닝 수치")]
    [SerializeField] private float _spreadMultiplier = 15f;
    [SerializeField] private float _shakeAngle = 5f;
    #endregion

    #region 내부 변수
    private Weapon _activeWeapon;
    private bool _isAiming = false;

    private float _dynamicSpread = 0f;
    private float _currentRotZ = 0f;
    private float _currentAimMultiplier = 1f;

    private RectTransform _parentRectTransform;
    #endregion

    private void Awake()
    {
        if (_center == null || _up == null || _down == null || _left == null || _right == null)
        {
            Debug.LogError("CrosshairControl UI 요소 없음 : 인스펙터 확인");
            return;
        }

        _center.alpha = 0f;
        _parentRectTransform = transform.parent as RectTransform;
    }

    void Update()
    {
        if (_activeWeapon == null || _activeWeapon.WeaponData == null)
        {
            return;
        }

        _dynamicSpread = Mathf.Lerp(_dynamicSpread, 0f, Time.deltaTime * _activeWeapon.Recoil.GetRecoverySpeed());
        _currentRotZ = Mathf.Lerp(_currentRotZ, 0f, Time.deltaTime * _activeWeapon.Recoil.GetRecoverySpeed());

        float targetAim = _isAiming ? _minSizeMult : 1f;

        _currentAimMultiplier = Mathf.Lerp(_currentAimMultiplier, targetAim, Time.deltaTime * _aimSpeed);

        float baseSpread = _activeWeapon.WeaponData.Spread * _spreadMultiplier * _currentAimMultiplier;           

        float finalSpread = baseSpread + _dynamicSpread;

        _up.anchoredPosition = new Vector2 (0f, finalSpread);
        _down.anchoredPosition = new Vector2 (0f, -finalSpread);
        _left.anchoredPosition= new Vector2 (-finalSpread, 0f);
        _right.anchoredPosition = new Vector2 (finalSpread, 0f);

        transform.localRotation = Quaternion.Euler(0f, 0f, _currentRotZ);

        if (_parentRectTransform != null)
        {
            Vector2 finalCursorPos = Input.mousePosition;

            if (_activeWeapon.Recoil != null)
            {
                finalCursorPos += _activeWeapon.Recoil.CurrentRecoilOffset;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentRectTransform,
                finalCursorPos,
                null,
                out Vector2 localPoint
            );
            (transform as RectTransform).anchoredPosition = localPoint;
        }
    }

    #region 외부 호출 함수
    public void OnWeaponFire()
    {
        _dynamicSpread += _activeWeapon.WeaponData.Spread * _spreadMultiplier;

        _currentRotZ += Random.Range(-_shakeAngle, _shakeAngle);
    }

    public void SetActiveWeapon(Weapon weapon)
    {
        _activeWeapon = weapon;
    }

    public void SetAimState(bool isAiming)
    {
        if (_isAiming == isAiming)
        {
            return;
        }

        _isAiming = isAiming;

        _center.DOKill();

        if (_isAiming)
        {
            _center.DOFade(1f, _centerFadeInOutDuration).SetEase(Ease.OutQuad);
        }

        else
        {
            _center.DOFade(0f, _centerFadeInOutDuration).SetEase(Ease.InQuad);
        }
    }
    #endregion
}