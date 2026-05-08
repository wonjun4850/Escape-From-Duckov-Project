using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data (SO)/CursorDataSO", fileName = "CursorDataSO")]
public class CursorDataSO : ScriptableObject
{
    #region 인스펙터
    [Header("커서 이미지")]
    [SerializeField] private Texture2D _arrowTexture;
    [SerializeField] private Texture2D _aimTexture;

    [Header("인게임 에임 세팅")]
    [SerializeField] private float _recoil = 0f;
    [SerializeField] private float _maxSpread = 0f;
    #endregion

    #region 프로퍼티
    public Texture2D ArrowTexture => _arrowTexture;
    public Texture2D AimTexture => _aimTexture;
    public float Recoil => _recoil;
    public float MaxSpread => _maxSpread;
    #endregion
}
