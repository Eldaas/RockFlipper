using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonDoubleClick : MonoBehaviour, IPointerClickHandler
{
    int clickCount;

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCount = eventData.clickCount;

        if (clickCount == 2)
        {
            // do something
        }

    }
}