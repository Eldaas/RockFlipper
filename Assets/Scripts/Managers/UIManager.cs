using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject loadScreen;
    public Color itemSelectionColor;
    public Color itemBackgroundColor;

    #region Properties
    public float LoadScreenAlpha => LoadScreenAlphaValue();
    #endregion

    private void Awake()
    {
        #region Singleton
        UIManager[] list = FindObjectsOfType<UIManager>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the UIManager component detected. Destroying an instance.");
        }
        else
        {
            instance = this;
        }
        #endregion
    }

    #region Public Methods
    public void LoadScreen(bool active)
    {
        Image image = loadScreen.GetComponent<Image>();
        Color loadColour = image.color;

        if (active)
        {
            loadScreen.SetActive(true);
            loadColour.a = 0f;
            image.color = loadColour;
            image.CrossFadeAlpha(1, 2f, false);
        }
        else
        {
            loadColour.a = 1f;
            image.color = loadColour;
            image.CrossFadeAlpha(0, 2f, false);
            StartCoroutine("DeactivateLoadScreen");
        }
    }

    #endregion

    #region Private Methods
    private float LoadScreenAlphaValue()
    {
        Image image = loadScreen.GetComponent<Image>();
        return image.color.a;
    }

    private IEnumerator DeactivateLoadScreen()
    {
        while(LoadScreenAlpha > 0f)
        {
            yield return new WaitForEndOfFrame();
        }

        loadScreen.SetActive(false);
    }
    #endregion

}
