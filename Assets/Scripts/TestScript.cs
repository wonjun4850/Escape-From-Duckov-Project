using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private PlayerDataSO _testPlayerData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneLoader.Instance.ShowResultUI(_testPlayerData, 2222, true);
        }
    }
}