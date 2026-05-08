using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.05f, 0.1f).SetEase(Ease.OutQuad);
        SoundManager.Instance.PlaySFX("Lobby_Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuad);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(0.95f, 0.1f).SetEase(Ease.OutQuad);
        SoundManager.Instance.PlaySFX("Lobby_Click");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuad);
    }
}