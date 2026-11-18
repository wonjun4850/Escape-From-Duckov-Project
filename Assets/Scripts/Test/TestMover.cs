using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMover : MonoBehaviour
{
    #region 인스펙터
    [Header("속도 설정")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("회전 속도 설정")]
    [SerializeField] private float _rotSpeed = 30f;
    #endregion

    #region 내부 변수
    private float _moveX;
    private float _moveZ;
    private Rigidbody _rb;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            Debug.LogError("플레이어 리지드바디 컴포넌트 없음.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        _moveX = Input.GetAxisRaw("Horizontal");
        _moveZ = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (_rb == null)
        {
            return;
        }

        Vector3 dir = new Vector3(_moveX, 0, _moveZ).normalized;

        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, _rotSpeed * Time.fixedDeltaTime);
        }

        float x = dir.x * _moveSpeed;
        float z = dir.z * _moveSpeed;

        Vector3 vel = _rb.velocity;

        vel.x = x;
        vel.z = z;

        _rb.velocity = vel;
    }
}
