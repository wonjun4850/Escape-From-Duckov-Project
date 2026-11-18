using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InteractableMarker : MonoBehaviour
{
    #region 인스펙터
    [Header("마커 프리팹")]
    [SerializeField] private GameObject _markerPrefab;

    [Header("부모 오브젝트")]
    [SerializeField] private Transform _parent;
    #endregion

    #region 내부 변수
    private Interaction _interaction;
    private Dictionary<IInteractable, InteractableMarker> _markers = new Dictionary<IInteractable, InteractableMarker>();
    #endregion

    private void OnDestroy()
    {
        UnBind();
    }

    private void UnBind()
    {
        if (_interaction != null)
        {
            _interaction.OnInteractableEnter -= OnInteractableEnter;
            _interaction.OnInteractableExit -= OnInteractableExit;
        }
    }

    private void OnInteractableEnter(IInteractable interactable)
    {
        if (_markers.ContainsKey(interactable))
        {
            return;
        }

        if (interactable is MonoBehaviour targetMono)
        {
            GameObject markerObj = Instantiate(_markerPrefab, _parent);

            InteractableMarker marker = markerObj.GetComponent<InteractableMarker>();

            if (marker != null)
            {
                marker.Init(targetMono.transform, out bool isSuccess);

                if (isSuccess)
                {
                    marker.PlayMarkerSequence();
                    _markers.Add(interactable, marker);
                }

                else
                {
                    Destroy(markerObj);
                }
            }

            else
            {
                Destroy(markerObj);
            }
        }
    }

    private void OnInteractableExit(IInteractable interactable)
    {
        if (_markers.TryGetValue(interactable, out InteractableMarker marker))
        {
            if (marker != null)
            {
                marker.ExitMarkerSequence();
                _markers.Remove(interactable);
            }
        }
    }

    #region 외부 호출 함수
    public void SetUp(Interaction interaction)
    {
        UnBind();

        _interaction = interaction;

        if (_interaction != null)
        {
            _interaction.OnInteractableEnter += OnInteractableEnter;
            _interaction.OnInteractableExit += OnInteractableExit;
        }

        else
        {
            Debug.LogError("UI_InteractableMarker Interaction = null");
        }
    }
    #endregion
}