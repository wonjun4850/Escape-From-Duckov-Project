using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class SceneTransitionUI : MonoBehaviour
{
    #region 인스펙터
    [Header("씬 전환 연출 UI")]
    [SerializeField] private CanvasGroup _sceneChangeUI;
    [SerializeField] private RectTransform _changeCircle;
    [SerializeField] private float _maxScale = 30f;
    [SerializeField] private float _midScale = 10f;
    [SerializeField] private float _minScale = 0f;
    [SerializeField] private float _duration = 2f;

    [Header("로딩중 UI")]
    [SerializeField] private CanvasGroup _loadingUI;
    [SerializeField] private float _loadingFadeDuration = 0.5f;

    [Header("클릭 UI")]
    [SerializeField] private CanvasGroup _clickUI;
    [SerializeField] private float _clickFadeDuration = 0.5f;
    [SerializeField] private RectTransform _circleAll; // 스케일값 연출용
    [SerializeField] private CanvasGroup _circleAllCanvas; // 알파값 연출용
    [SerializeField] private Image _edgeCircle;
    [SerializeField] private CanvasGroup _fillCircle;
    [SerializeField] private TextMeshProUGUI _clickText;

    [Header("씬 별 연출")]
    [SerializeField] private CanvasGroup _specificLoading;
    [SerializeField] private float _specificLoadingFadeDuration = 1f;
    [SerializeField] private GameObject _start;
    [SerializeField] private GameObject _escape;
    [SerializeField] private GameObject _dead;

    [Header("결과창 UI")]
    [SerializeField] private float _resultFadeDuration = 1f;
    [SerializeField] private CanvasGroup _resultGroup;
    [SerializeField] private Image _expBar;
    [SerializeField] private Button _escapeContinueButton;
    [SerializeField] private Button _deadContinueButton;
    [SerializeField] private RectTransform _buttons;
    [SerializeField] private TextMeshProUGUI _currentLevelText;
    [SerializeField] private TextMeshProUGUI _currentExpText;
    [SerializeField] private TextMeshProUGUI _currentMaxExpText;
    [SerializeField] private TextMeshProUGUI _mainText;
    #endregion

    #region 외부 호출 함수
    public void Init()
    {
        _sceneChangeUI.alpha = 0f;
        _loadingUI.alpha = 0f;
        _clickUI.alpha = 0f;
        _specificLoading.alpha = 0f;
        _changeCircle.localScale = Vector3.one * _maxScale;

        _sceneChangeUI.gameObject.SetActive(false);
        _loadingUI.gameObject.SetActive(false);
        _clickUI.gameObject.SetActive(false);
        _specificLoading.gameObject.SetActive(false);
    }

    public void ResultInit()
    {
        _resultGroup.alpha = 0f;
        _mainText.text = "";
        _buttons.gameObject.SetActive(false);
        _escapeContinueButton.gameObject.SetActive(false);
        _deadContinueButton.gameObject.SetActive(false);
        _resultGroup.gameObject.SetActive(false);
    }

    public Tween CircleIn()
    {
        _sceneChangeUI.gameObject.SetActive(true);
        _sceneChangeUI.alpha = 1f;

        Sequence sq = DOTween.Sequence().SetUpdate(true);
        sq.Append(_changeCircle.DOScale(_midScale, _duration * 0.6f).SetEase(Ease.OutBack));
        sq.Append(_changeCircle.DOScale(_minScale, _duration * 0.3f).SetEase(Ease.InExpo));
        return sq;
    }

    public Tween CircleOut()
    {
        Sequence sq = DOTween.Sequence().SetUpdate(true);
        sq.Append(_changeCircle.DOScale(_midScale, _duration * 0.6f).SetEase(Ease.OutBack));
        sq.Append(_changeCircle.DOScale(_maxScale, _duration * 0.3f).SetEase(Ease.InExpo));
        sq.OnComplete(() => _sceneChangeUI.gameObject.SetActive(false));
        return sq;
    }

    public void SetLoadingTextUI(bool isActive)
    {
        if (!_loadingUI.gameObject.activeSelf)
        {
            _loadingUI.gameObject.SetActive(true);
        }

        if (isActive)
        {
            _loadingUI.DOFade(1f, _loadingFadeDuration).SetUpdate(true);
        }

        else
        {
            _loadingUI.DOFade(0f, _loadingFadeDuration).SetUpdate(true).OnComplete(() => _loadingUI.gameObject.SetActive(false));
        }
    }

    public void SetClickUI(bool isActive)
    {
        if (!_clickUI.gameObject.activeSelf)
        {
            _clickUI.gameObject.SetActive(true);
        }

        if (isActive)
        {
            _fillCircle.alpha = 1f;

            _clickUI.DOFade(1f, _clickFadeDuration).SetUpdate(true).OnComplete(() =>
            {
                _circleAll.DOScale(1.35f, 1.25f).SetLoops(-1);
                _circleAllCanvas.DOFade(0f, 1.25f).SetLoops(-1);
            });
        }

        else
        {
            _clickText.transform.DOPunchScale(new Vector3(0.15f, 0.15f, 0.15f), 0.3f, 5, 1f).SetUpdate(true);

            _clickUI.DOFade(0f, _clickFadeDuration).SetUpdate(true).OnComplete(() =>
            {
                _circleAll.DOKill();
                _circleAllCanvas.DOKill();
                _fillCircle.alpha = 0f;
                _clickUI.gameObject.SetActive(false);
            });
        }
    }

    public void SetStartLoadingUI(bool isActive)
    {
        if (isActive)
        {
            _start.SetActive(true);

            _specificLoading.DOFade(1f, _specificLoadingFadeDuration).SetUpdate(true);
        }

        else
        {
            _specificLoading.DOFade(0f, _specificLoadingFadeDuration).SetUpdate(true).OnComplete(() => _start.SetActive(false));
        }
    }

    public void SetEscapeLoadingUI(bool isActive)
    {
        if (isActive)
        {
            _escape.SetActive(true);

            _specificLoading.DOFade(1f, _specificLoadingFadeDuration).SetUpdate(true);
        }

        else
        {
            _specificLoading.DOFade(0f, _specificLoadingFadeDuration).SetUpdate(true).OnComplete(() => _escape.SetActive(false));
        }
    }

    public void SetDeadLoadingUI(bool isActive)
    {
        if (isActive)
        {
            _dead.SetActive(true);

            _specificLoading.DOFade(1f, _specificLoadingFadeDuration).SetUpdate(true);
        }

        else
        {
            _specificLoading.DOFade(0f, _specificLoadingFadeDuration).SetUpdate(true).OnComplete(() => _dead.SetActive(false));
        }
    }

    public void ResultUI(PlayerDataSO player, int gainExp, bool isEscape)
    {
        _sceneChangeUI.gameObject.SetActive(true);
        _sceneChangeUI.alpha = 1f;

        if (isEscape)
        {
            _mainText.text = "철수 완료";
            Color c = new Color(1f, 1f, 1f);
            _mainText.color = c;
        }

        else
        {
            _mainText.text = "YOU DIED";
            Color c = new Color(1f, 85f / 255f, 100f / 255f);
            _mainText.color = c;
        }

        _currentLevelText.text = $"{player.Level}";
        _currentExpText.text = $"{player.CurrentExp}";
        _currentMaxExpText.text = $"{player.MaxExp}";
        _expBar.fillAmount = (float)player.CurrentExp / player.MaxExp;

        Sequence sq = DOTween.Sequence().SetUpdate(true);
        sq.Append(_changeCircle.DOScale(_minScale, _duration * 1.5f).SetEase(Ease.InExpo));
        sq.AppendInterval(0.1f);
        sq.AppendCallback(() => _resultGroup.gameObject.SetActive(true));
        sq.Append(_resultGroup.DOFade(1f, _resultFadeDuration));
        sq.AppendInterval(0.1f);

        if (gainExp > 0)
        {
            sq.AppendCallback(() => ExpEffect(player, gainExp, 2f, isEscape));
        }

        else
        {
            sq.AppendCallback(() => ShowContinueButton(isEscape));
        }
    }
    #endregion

    private void ShowContinueButton(bool isEscape)
    {
        _buttons.gameObject.SetActive(true);

        if (isEscape)
        {
            _escapeContinueButton.gameObject.SetActive(true);            
        }

        else
        {
            _deadContinueButton.gameObject.SetActive(true);
        }

        _buttons.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
    }

    private void ExpEffect(PlayerDataSO player, int gainExp, float duration, bool isEscape)
    {
        if (gainExp <= 0)
        {
            SoundManager.Instance.PlaySFX("Exp_Up");
            ShowContinueButton(isEscape);
            return;
        }

        int maxExp = player.MaxExp;
        int currentExp = player.CurrentExp;
        int needExp = maxExp - currentExp;
        int add = Mathf.Min(gainExp, needExp);
        float targetFill = (float)(currentExp + add) / maxExp;
        float sfxTick = _expBar.fillAmount;

        _expBar.DOFillAmount(targetFill, duration)
            .SetEase(Ease.OutCubic)
            .OnUpdate(() =>
            {
                _currentExpText.text = Mathf.RoundToInt(_expBar.fillAmount * maxExp).ToString();

                if (_expBar.fillAmount >= sfxTick + 0.1f)
                {
                    SoundManager.Instance.PlaySFX("Exp_Up");
                    sfxTick = _expBar.fillAmount;
                }
            })
            .OnComplete(() =>
            {
                if (add >= needExp)
                {
                    SoundManager.Instance.PlaySFX("Exp_Up");

                    player.LevelUp();
                    player.AddExp(-player.CurrentExp);

                    _expBar.fillAmount = 0f;
                    _currentLevelText.text = player.Level.ToString();
                    _currentMaxExpText.text = player.MaxExp.ToString();
                    _currentExpText.text = Mathf.RoundToInt(_expBar.fillAmount * maxExp).ToString();
                    ExpEffect(player, gainExp - add, duration, isEscape);
                }

                else
                {
                    player.AddExp(add);
                    ShowContinueButton(isEscape);
                    SoundManager.Instance.PlaySFX("Exp_Up");
                }
            });
    }

    #region 버튼 함수
    public void OnClickEscapeContinue()
    {
        _escapeContinueButton.interactable = false;
        SoundManager.Instance.PlaySFX("Confirm");
        SceneLoader.Instance.LoadScene("Base1", "Ingame");
    }

    public void OnClickDeadContinue()
    {
        _deadContinueButton.interactable = false;
        SoundManager.Instance.PlaySFX("Confirm");
        SceneLoader.Instance.LoadScene("Base1", "Ingame", true);
    }
    #endregion
}