using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameCamera : MonoBehaviour
{
    #region 인스펙터
    [Header("카메라 설정")]
    [SerializeField] private Vector3 _positionOffset = new Vector3(6, 20, -10);
    [SerializeField] private float _mouseOffset = 5f;
    [SerializeField] private float _smoothSpeed = 5f;
    #endregion

    #region 내부 변수
    private Transform _playerTr;
    #endregion

    private void LateUpdate()
    {
        if (_playerTr == null) return;

        float mouseX = (Input.mousePosition.x / Screen.width) * 2f - 1f;
        float mouseY = (Input.mousePosition.y / Screen.height) * 2f - 1f;

        Vector3 camF = Vector3.ProjectOnPlane(this.transform.forward, Vector3.up).normalized;
        Vector3 camR = Vector3.ProjectOnPlane(this.transform.right, Vector3.up).normalized;

        Vector3 mouseMove = (camR * mouseX + camF * mouseY) * _mouseOffset;

        Vector3 targetPos = _playerTr.position + _positionOffset + mouseMove;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _smoothSpeed);
    }

    #region 외부 호출 함수
    public void SetTarget(Transform target)
    {
        _playerTr = target;
        transform.position = _playerTr.position + _positionOffset;
        transform.LookAt(_playerTr.position);
    }
    #endregion
}