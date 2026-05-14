using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCamera : MonoBehaviour
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

        float moveX = mouseY * _moveSpeed;
        float moveY = -mouseX * _moveSpeed;

        Quaternion Rotation = _originRotation * Quaternion.Euler(moveX, moveY, 0f);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Rotation, Time.deltaTime * _smoothSpeed);
    }
}
