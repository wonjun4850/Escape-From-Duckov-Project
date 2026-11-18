using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    #region 인스펙터
    [Header("테스트 용")]
    [SerializeField] private bool _isTest = true;
    [SerializeField] private UI_InteractableMarker _markerUI;

    [Header("감지 범위 콜라이더 참조")]
    [SerializeField] private SphereCollider _detectionCollider;

    [Header("범위 설정")]
    [SerializeField] private float _detectionRange = 6f;
    [SerializeField] private float _interactionRange = 2f;
    #endregion

    #region 내부 변수
    private List<IInteractable> _nearbyTargets = new List<IInteractable>();
    private IInteractable _currentTarget = null;

    public event System.Action<IInteractable> OnCurrentTargetChanged;
    public event System.Action<IInteractable> OnInteractableEnter;
    public event System.Action<IInteractable> OnInteractableExit;
    #endregion

    private void Awake()
    {
        if (_detectionCollider == null)
        {
            Debug.LogError("Detection Collider가 설정되지 않았습니다.");
            enabled = false;
            return;
        }

        _detectionCollider.radius = _detectionRange;
    }

    private void Update()
    {
        UpdateCurrentInteractableTarget();

        if (!_isTest)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _markerUI.SetUp(this);
        }
        
    }

    private void UpdateCurrentInteractableTarget()
    {
        if (_nearbyTargets.Count <= 0)
        {
            if (_currentTarget != null)
            {
                _currentTarget = null;
                OnCurrentTargetChanged?.Invoke(_currentTarget);
            }

            return;
        }

        float nearestDistance = float.MaxValue;
        IInteractable nearestTarget = null;
        Vector3 playerPos = this.transform.position;

        foreach (var target in _nearbyTargets)
        {
            if (target is MonoBehaviour targetMono)
            {
                float distance = Vector3.Distance(playerPos, targetMono.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = target;
                }
            }
        }

        IInteractable newTarget = null;

        if (nearestDistance <= _interactionRange)
        {
            newTarget = nearestTarget;
        }

        if (_currentTarget == newTarget)
        {
            return;
        }

        _currentTarget = newTarget;
        OnCurrentTargetChanged?.Invoke(_currentTarget);
    }

    #region 외부 호출 함수
    public void Interact()
    {
        if (_currentTarget == null)
        {
            return;
        }

        _currentTarget.Interact();
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IInteractable>(out var interactable))
        {
            return;
        }

        if (_nearbyTargets.Contains(interactable))
        {
            return;
        }

        _nearbyTargets.Add(interactable);
        OnInteractableEnter?.Invoke(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<IInteractable>(out var interactable))
        {
            return;
        }

        if (!_nearbyTargets.Contains(interactable))
        {
            return;
        }

        _nearbyTargets.Remove(interactable);
        OnInteractableExit?.Invoke(interactable);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _interactionRange);
    }
}