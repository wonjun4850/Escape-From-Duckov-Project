using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    #region 인스펙터
    [Header("설정")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _smoothSpeed = 1f;
    #endregion

    #region 내부 변수
    private Quaternion _originRotation;
    #endregion

    void Start()
    {
        _originRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        float mouseX = (Input.mousePosition.x / Screen.width) * 2f - 1f;
        float mouseY = (Input.mousePosition.y / Screen.height) * 2f - 1f;

        float targetX = mouseY * _moveSpeed;
        float targetY = -mouseX * _moveSpeed;

        Quaternion targetRotation = _originRotation * Quaternion.Euler(targetX, targetY, 0f);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * _smoothSpeed);
    }
}
