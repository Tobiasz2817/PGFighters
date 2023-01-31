using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CustomizeRotation : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 15;
    private bool isHandle = false;
    
    private void Update()
    {
        if (!isHandle) return;
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        var mousePos = Mouse.current.position.ReadValue();
        var position = new Vector3(mousePos.x, mousePos.y, 10);
        var onWorldPoint = Camera.main.ScreenToWorldPoint(position) * rotateSpeed * Time.deltaTime;
        onWorldPoint.y = 0;
        onWorldPoint.z = 0;
        transform.Rotate(new Vector3(0,onWorldPoint.x,0));
    }
    public void OnDragEnter(BaseEventData eventData)
    {
        isHandle = true;
    }

    public void OnDragExit(BaseEventData eventData)
    {
        isHandle = false;
    }
}
