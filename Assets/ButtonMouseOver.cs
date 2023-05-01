using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonMouseOver : MonoBehaviour, IPointerEnterHandler
{
    public UnityEvent OnMouseOver;
    public void OnPointerEnter(PointerEventData eventData)
    {

        OnMouseOver.Invoke();
    }
}
