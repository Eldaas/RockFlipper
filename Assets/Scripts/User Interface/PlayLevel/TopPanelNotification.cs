using System.Collections;
using UnityEngine;
using TMPro;

public class TopPanelNotification : MonoBehaviour
{
    public float activeTime;
    public float fadeTime;
    public bool isActive;

    [SerializeField]
    private TextMeshProUGUI elementText;
    private CanvasGroup cg;

    public void Activate()
    {
        cg = GetComponent<CanvasGroup>();
        isActive = true;
        StartCoroutine(FadeIn());
    }

    public void BeginActiveCountdown()
    {
        StartCoroutine(Countdown());
    }

    public void Deactivate()
    {
        isActive = false;
        SceneController.instance.sceneUi.activeNotifications.Remove(this);
        StartCoroutine(FadeOut());
    }

    public void DeactivateImmediate()
    {
        isActive = false;
        SceneController.instance.sceneUi.activeNotifications.Remove(this);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public void SetMessage(string message, Color color)
    {
        elementText.text = message;
        elementText.color = color;
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