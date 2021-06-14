using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notification : MonoBehaviour
{
    [SerializeField]
    protected float activeTime;
    [SerializeField]
    protected float fadeTime;
    [SerializeField]
    public bool isActive;
    [SerializeField]
    protected TextMeshProUGUI elementText;
    protected CanvasGroup cg;

    public virtual void Activate()
    {
        cg = GetComponent<CanvasGroup>();
        isActive = true;
        StartCoroutine(FadeIn());
    }

    public virtual void Deactivate()
    {
        isActive = false;
        StartCoroutine(FadeOut());
    }

    public void SetMessage(string message, Color color)
    {
        elementText.text = message;
        elementText.color = color;
    }

    public bool CompareMessage(string message)
    {
        if(string.Compare(elementText.text, message) == 0)
        {
            return true;
        }

        return false;
    }

    protected virtual void BeginActiveCountdown()
    {
        StartCoroutine(Countdown());
    }

    public virtual void DeactivateImmediate()
    {
        isActive = false;
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator Countdown()
    {
        float time = activeTime - fadeTime * 2;

        while (time > Mathf.Epsilon)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        Deactivate();
    }

    private IEnumerator FadeIn()
    {
        float time = fadeTime;

        while (time > Mathf.Epsilon)
        {
            time -= Time.deltaTime;
            cg.alpha = Mathf.SmoothStep(0f, 1f, 1f - (time / fadeTime));

            yield return null;
        }

        BeginActiveCountdown();
    }

    private IEnumerator FadeOut()
    {
        float time = fadeTime;

        while (time > Mathf.Epsilon)
        {
            time -= Time.deltaTime;
            cg.alpha = Mathf.SmoothStep(1f, 0f, 1f - (time / fadeTime));

            yield return null;
        }

        Destroy(gameObject);
    }
}
