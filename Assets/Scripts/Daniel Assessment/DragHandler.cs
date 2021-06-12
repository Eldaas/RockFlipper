using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private RectTransform rect;
    private Image itemBackground;
    private Transform parent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        itemBackground = GetComponent<Image>();
        parent = transform.parent;
        canvas = HangarController.instance.hangarUi.canvas;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("OnPointerDown");
    }   

    public void OnBeginDrag(PointerEventData data)
    {
        Debug.Log("OnBeginDrag");
        itemBackground.enabled = false;
        rect.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData data)
    {
        Debug.Log("OnDrag");

        rect.anchoredPosition += data.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData data)
    {
        Debug.Log("OnEndDrag");
        itemBackground.enabled = true;
        rect.SetParent(parent);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
}
