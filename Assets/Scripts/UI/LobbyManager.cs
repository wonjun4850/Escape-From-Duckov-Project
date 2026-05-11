using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    #region 인스펙터
    [Header("게임 시작 시 UI 연출")]
    [SerializeField] private GameObject _duckovLogo;
    [SerializeField] private TextMeshProUGUI _pressAnyKeyText;

    [Header("메인 메뉴 UI")]
    [SerializeField] private GameObject _mainmenu;
    #endregion

    private void Awake()
    {
        if (_duckovLogo == null || _pressAnyKeyText == null || _mainmenu == null)
        {
            Debug.LogError("LobbyManager: UI 요소가 할당되지 않았습니다.");
            return;
        }
    }

    void Start()
    {
        CursorManager.Instance.SetCursorByScene();
        InputDispatcher.Instance.ChangeActionMap("Lobby");

        _duckovLogo.SetActive(false);
        _duckovLogo.transform.localScale = Vector3.zero;

        _pressAnyKeyText.gameObject.SetActive(false);
        _pressAnyKeyText.alpha = 0f;

        _mainmenu.SetActive(false);

        StartLobbySequence();
    }

    private void StartLobbySequence()
    {
        Sequence start = DOTween.Sequence();

        start.AppendInterval(1f);
        // 로고 연출
        start.AppendCallback(() => _duckovLogo.SetActive(true));
        start.Append(_duckovLogo.transform.DOScale(1.1f, 0.5f));

        start.AppendInterval(1f);
        // 텍스트 연출
        start.AppendCallback(() => _pressAnyKeyText.gameObject.SetActive(true));
        start.Append(_pressAnyKeyText.DOFade(1f, 1f));
        start.OnComplete(() =>
        {
            InputCommandHub.Instance.RegisterCommands("Lobby.PressAnyKey", new SimpleCommand(OnPressAnyKey));
            _pressAnyKeyText.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);
        });
    }

    private void OnPressAnyKey()
    {
        InputCommandHub.Instance.RegisterCommands("Lobby.PressAnyKey", null);
        _pressAnyKeyText.DOKill();
        _duckovLogo.transform.DOKill();
        _pressAnyKeyText.alpha = 1f;

        Sequence end = DOTween.Sequence();

        end.Join(_duckovLogo.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 10, 1f));
        end.Join(_pressAnyKeyText.transform.DOScale(1.2f, 0.2f).SetEase(Ease.OutQuad));
        SoundManager.Instance.PlaySFX("Lobby_Coin");

        end.AppendInterval(0.1f);
        end.Append(_duckovLogo.transform.DOScale(0f, 0.4f).SetEase(Ease.InBack));
        end.Join(_pressAnyKeyText.DOFade(0f, 0.4f));

        end.OnComplete(() =>
        {
            SoundManager.Instance.PlayBGM("Lobby_BGM", 1f);
            _duckovLogo.SetActive(false);
            _pressAnyKeyText.gameObject.SetActive(false);
            _pressAnyKeyText.DOKill();
            _duckovLogo.transform.DOKill();
        });

        end.AppendInterval(0.5f);
        end.AppendCallback(() => _mainmenu.SetActive(true));
    }
}