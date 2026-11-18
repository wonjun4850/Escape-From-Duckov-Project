using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestScript : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("아이템 상호작용 완료");
    }
}