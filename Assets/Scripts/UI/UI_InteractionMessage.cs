using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InteractionMessage : MonoBehaviour
{
    #region 인스펙터

    #endregion

    #region 내부 변수
    private Interaction _interaction;
    #endregion

    private void OnDestroy()
    {
        UnBind();
    }

    private void UnBind()
    {
        if (_interaction != null)
        {
            _interaction.OnCurrentTargetChanged -= OnCurrentTargetChanged;
        }
    }

    private void OnCurrentTargetChanged(IInteractable interactable)
    {
        
    }

    #region 외부 호출 함수
    public void SetUp(Interaction interaction)
    {
        UnBind();

        _interaction = interaction;

        if (_interaction != null)
        {
            _interaction.OnCurrentTargetChanged += OnCurrentTargetChanged;
        }

        else
        {
            Debug.LogError("UI_InteractionMessage Interaction = null");
        }
    }
    #endregion
}
