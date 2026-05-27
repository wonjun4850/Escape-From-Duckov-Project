using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    #region 인스펙터
    [Header("커서 SO")]
    [SerializeField] private CursorDataSO _cursorDataSO;

    [Header("크로스헤어")]
    [SerializeField] private RectTransform _cursorAnchor;
    #endregion

    #region 내부 변수
    public static CursorManager Instance { get; private set; }
    private CrosshairControl _crosshair;
    #endregion

    #region 프로퍼티
    public CursorDataSO CursorDataSO => _cursorDataSO;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _crosshair = GetComponentInChildren<CrosshairControl>();

        if (_cursorDataSO == null || _cursorAnchor == null || _crosshair == null)
        {
            Debug.LogError("CursorManager 확인 필요");
            return;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Update()
    {
        if (_cursorAnchor != null && _cursorAnchor.gameObject.activeInHierarchy)
        {
            Vector2 localMousePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                _cursorAnchor.parent as RectTransform,
                Input.mousePosition,
                null,
                out localMousePos
                );

            _cursorAnchor.anchoredPosition = localMousePos;
        }
    }

    #region 외부 호출 함수
    public void SetCursorByScene(string actionMapName = "")
    {
        if (actionMapName == "Ingame")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            if (_cursorAnchor != null)
            {
                _cursorAnchor.gameObject.SetActive(true);
            }
        }

        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Cursor.SetCursor(_cursorDataSO.ArrowTexture, Vector2.zero, CursorMode.Auto);

            if (_cursorAnchor != null)
            {
                _cursorAnchor.gameObject.SetActive(false);
            }
        }
    }

    public void SetActiveWeaponToCrosshair(Weapon weapon)
    {
        if (_crosshair != null)
        {
            _crosshair.SetActiveWeapon(weapon);
        }
    }

    public void SetAimStateToCrosshair(bool isAiming)
    {
        if (_crosshair != null)
        {
            _crosshair.SetAimState(isAiming);
        }
    }

    public void FireCrosshairEffect()
    {
        if (_crosshair != null)
        {
            _crosshair.OnWeaponFire();
        }
    }
    #endregion
}