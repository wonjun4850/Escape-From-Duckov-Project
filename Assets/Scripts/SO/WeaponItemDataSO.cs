using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/WeaponItemDataSO", fileName = "WeaponItemDataSO")]
public class WeaponItemDataSO : EquipmentItemDataSO
{
    public enum EWeaponType
    {
        None,
        Pistol,
        Rifle,
        Shotgun,
        SMG,
        Melee
    }

    #region 인스펙터
    [Header("무기 세부 분류")]
    [SerializeField] private EWeaponType _weaponType = EWeaponType.None;

    [Header("총기 성능 설정")]
    [SerializeField] private bool _isAutomatic = false;
    [SerializeField] private float _damage = 0f;
    [SerializeField] private float _effectiveRange = 0f; // 유효 사거리
    [SerializeField] private float _damageOutsideEffectiveRange = 0.0f; // 유효 사거리 외 대미지 계수
    [SerializeField] private float _fireRate = 0f;
    [SerializeField] private int _magazineSize = 0;
    [SerializeField] private float _reloadTime = 0f;
    [SerializeField] private float _recoilX = 0f;
    [SerializeField] private float _recoilY = 0f;
    [SerializeField] private float _spread = 0f; // 탄퍼짐 (조준 상태에서는 값을 낮춰주면 될듯?)
    [SerializeField] private float _soundRange = 0f;
    [SerializeField] private float _headshotMultiplier = 0f;
    [SerializeField] private float _moveSpeedModifier = 1.0f;

    [Header("투사체 설정 (근접 무기는 사용 안함)")]
    [SerializeField] private float _projectileSpeed = 0f;
    [SerializeField] private float _projectileLifetime = 0f;
    [SerializeField] private GameObject _projectilePrefab = null;
    #endregion

    #region 프로퍼티
    public EWeaponType WeaponType => _weaponType;
    public bool IsAutomatic => _isAutomatic;
    public float Damage => _damage;
    public float EffectiveRange => _effectiveRange;
    public float DamageOutsideEffectiveRange => _damageOutsideEffectiveRange;
    public float FireRate => _fireRate;
    public int MagazineSize => _magazineSize;
    public float ReloadTime => _reloadTime;
    public float RecoilX => _recoilX;
    public float RecoilY => _recoilY;
    public float Spread => _spread;
    public float SoundRange => _soundRange;
    public float HeadshotMultiplier => _headshotMultiplier;
    public float MoveSpeedModifier => _moveSpeedModifier;
    public float ProjectileSpeed => _projectileSpeed;
    public float ProjectileLifetime => _projectileLifetime;
    public GameObject ProjectilePrefab => _projectilePrefab;
    #endregion
}