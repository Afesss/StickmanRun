using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event System.Action<Vector2> OnPointerUpWithDragVector;

    private Vector2 touchPoint;


    [Inject] private PlayerSettings playerSettings;

    public void OnPointerDown(PointerEventData eventData)
    {
        touchPoint = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 dragVector = eventData.position - touchPoint;

        OnPointerUpWithDragVector?.Invoke(dragVector);

        touchPoint = Vector2.zero;
    }
}
