using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameCamera : MonoBehaviour
{
    #region 인스펙터
    [Header("플레이어 참조")]
    [SerializeField] private Transform _playerTr = null;

    [Header("카메라 설정")]
    [SerializeField] private Vector3 _positionOffset = new Vector3(6, 20, -10);
    [SerializeField] private float _mouseOffset = 5f;
    [SerializeField] private float _smoothSpeed = 5f;
    #endregion

    void Start()
    {
        if (_playerTr == null)
        {
            return;
        }

        transform.position = _playerTr.position + _positionOffset;
        transform.LookAt(_playerTr.position);
    }

    private void LateUpdate()
    {
        if (_playerTr == null) return;

        float mouseX = (Input.mousePosition.x / Screen.width) * 2f - 1f;
        float mouseY = (Input.mousePosition.y / Screen.height) * 2f - 1f;

        Vector3 camRight = transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 camForward = transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 mouseMove = (camRight * mouseX + camForward * mouseY) * _mouseOffset;

        Vector3 targetPos = _playerTr.position + _positionOffset + mouseMove;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _smoothSpeed);
    }
}