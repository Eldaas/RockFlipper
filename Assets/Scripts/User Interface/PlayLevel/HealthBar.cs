using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Image fillBar;
    [SerializeField]
    private CanvasGroup canvasGroup;

    public void Activate()
    {
        canvasGroup.alpha = 1f;
    }

    public void Deactivate()
    {
        canvasGroup.alpha = 0f;
    }

    public void UpdateHpBar(float percentage)
    {
        fillBar.fillAmount = percentage;
    }

    public void UpdatePosition(Transform trans)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(trans.position);
        transform.position = screenPos;
    }
}
