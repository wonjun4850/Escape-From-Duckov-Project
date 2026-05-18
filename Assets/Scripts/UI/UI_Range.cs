using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 추후에 장착하고 있는 총의 유효사거리에 따른 색변화 추가할 예정?
public class UI_Range : MonoBehaviour
{
    #region 인스펙터
    [Header("숫자 변경 텍스트")]
    [SerializeField] private TextMeshProUGUI _rangeText;
    #endregion

    #region 내부 변수
    private Transform _playerTr;
    private Camera _camera;
    private int _lastDistance = -1;
    #endregion

    void Update()
    {
        if (_playerTr == null)
        {
            return;
        }

        if (_camera == null)
        {
            _camera = Camera.main;
        }

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        Plane plane = new Plane(Vector3.up, _playerTr.position);

        if (plane.Raycast(ray, out float rayDistance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(rayDistance);

            float fDistance = Vector3.Distance(_playerTr.position, mouseWorldPos);

            int Idistance = Mathf.RoundToInt(fDistance);

            if (_lastDistance != Idistance)
            {
                _lastDistance = Idistance;
                _rangeText.text = Idistance.ToString("00");
            }
        }
    }

    #region 외부 호출 함수
    public void SetUp(Transform playerTr)
    {
        _playerTr = playerTr;
        _camera = Camera.main;

        if (_playerTr == null)
        {
            Debug.LogError("_playerTr = null 확인 필요");
        }
    }
    #endregion
}