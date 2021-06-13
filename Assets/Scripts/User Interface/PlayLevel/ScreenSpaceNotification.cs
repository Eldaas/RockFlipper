using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceNotification : Notification
{
    [HideInInspector]
    public Transform trackedTransform;

    public override void Activate()
    {
        base.Activate();
        StartCoroutine(UpdateNotification());
    }

    public void UpdatePosition()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(trackedTransform.position);
        transform.position = screenPos;

        if(screenPos.y < 0f)
        {
            DestroyImmediate(gameObject);
        }
    }

    private IEnumerator UpdateNotification()
    {
        while (isActive)
        {
            UpdatePosition();
            yield return new WaitForEndOfFrame();
        }
    }


}
