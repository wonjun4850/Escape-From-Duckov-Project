using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    #region 인스펙터
    [Header("커서 SO")]
    [SerializeField] private CursorDataSO _cursorDataSO;
    #endregion

    #region 내부 변수
    public static CursorManager Instance { get; private set; }
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

        if (_cursorDataSO == null)
        {
            Debug.LogError("CursorManager : 커서 SO 데이터 없음");
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

    #region 외부 호출 함수
    public void SetCursorByScene(string actionMapName = "")
    {
        if (actionMapName == "Ingame")
        {
            Vector2 center = new Vector2(_cursorDataSO.AimTexture.width / 2, _cursorDataSO.AimTexture.height / 2);

            Cursor.SetCursor(_cursorDataSO.AimTexture, center, CursorMode.Auto);
        }

        else
        {
            Cursor.SetCursor(_cursorDataSO.ArrowTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    public void ApplyCursorRecoil(Vector2 recoilAmount)
    {
        // 추후 구현 예정
    }
    #endregion
}