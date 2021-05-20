using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject loadScreen;
    public Color itemSelectionColor;
    public Color itemBackgroundColor;

    [Header("Error Modal")]
    public GameObject errorModal;
    public Button errorModalCloseButton;
    public Button errorModalOkButton;
    public TextMeshProUGUI errorModalTitle;
    public TextMeshProUGUI errorModalDescription;

    [Header("Events")]
    private UnityAction errorModalCantAffordDelegate;
    private UnityAction errorModalPlayerDiedDelegate;
    private UnityAction errorModalReturnedFromDeathDelegate;

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

    private void Start()
    {
        RegisterListeners();
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
    private void RegisterListeners()
    {
       
        // Custom events
        errorModalCantAffordDelegate = delegate { ErrorModal(ErrorType.CantAfford); };
        EventManager.StartListening("CantAffordItem", errorModalCantAffordDelegate);

        errorModalPlayerDiedDelegate = delegate { ErrorModal(ErrorType.PlayerDeath); };
        EventManager.StartListening("PlayerDeath", errorModalPlayerDiedDelegate);

        errorModalReturnedFromDeathDelegate = delegate { ErrorModal(ErrorType.ReturnFromDeath); };
        EventManager.StartListening("ReturnedFromDeath", errorModalReturnedFromDeathDelegate);
    }

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

    private void ErrorModal(ErrorType type)
    {
        switch (type)
        {
            case ErrorType.Unspecified:
                errorModalDescription.text = "An unknown error occurred.";
                RemoveModalListeners();
                ApplyStandardModalListeners();
                break;
            case ErrorType.CantAfford:
                errorModalTitle.text = "Oops.";
                errorModalDescription.text = "You can't afford this item! Try again after collecting some resources.";
                RemoveModalListeners();
                ApplyStandardModalListeners(); 
                break;
            case ErrorType.PlayerDeath:
                errorModalTitle.text = "Oops.";
                errorModalDescription.text = "Oh no! Your ship exploded. Don't worry. The Company will give you a new one on your return to base... for a small fee.";
                // Button listeners are handled by the DeathState state 
                break;
            case ErrorType.ReturnFromDeath:
                errorModalTitle.text = "New Ship";
                errorModalDescription.text = $"We've issued you with a new ship and deducted ${ProfileManager.instance.currentProfile.deathCost.ToString("#,#")} from your balance to cover the insurance costs. Try not to let it happen again, otherwise the costs keep going up!";
                ApplyStandardModalListeners();
                break;
        }

        errorModal.SetActive(true);
    }

    private void RemoveModalListeners()
    {
        errorModalOkButton.onClick.RemoveAllListeners();
        errorModalCloseButton.onClick.RemoveAllListeners();
    }

    private void ApplyStandardModalListeners()
    {
        errorModalOkButton.onClick.AddListener(delegate { errorModal.SetActive(false); });
        errorModalCloseButton.onClick.AddListener(delegate { errorModal.SetActive(false); });
    }
    #endregion

}

public enum ErrorType { Unspecified, CantAfford, PlayerDeath, ReturnFromDeath }