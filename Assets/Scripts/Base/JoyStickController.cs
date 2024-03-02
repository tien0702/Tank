using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickController : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public Transform handle;

    public void OnBeginDrag(PointerEventData eventData)
    {
        handle.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        handle.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        handle.transform.position = eventData.position;
    }
}
