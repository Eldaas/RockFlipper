using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class IntroMenu : UIController
{
    [Header("Main Menu")]
    [SerializeField]
    private Canvas introMenuCanvas;
    [SerializeField]
    private Button visitHangarButton;
    [SerializeField]
    private Button profileButton;
    [SerializeField]
    private Button highScoresButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button exitGameButton;

    [Header("Profile Screen")]
    [SerializeField]
    private Canvas profileScreenCanvas;
    [SerializeField]
    private Button newProfileButton;
    [SerializeField]
    private TMP_InputField newProfileInput;
    [SerializeField]
    private Button inputConfirmButton;
    [SerializeField]
    private TMP_Dropdown profileSelectDropdown;
    [SerializeField]
    private Button returnToMenuButton;
    [SerializeField]
    private TextMeshProUGUI currentProfileText;
    [SerializeField]
    private TextMeshProUGUI balanceText;
    [SerializeField]
    private TextMeshProUGUI reputationText;


    private void Awake()
    {
        RegisterListeners();
    }

    private void Update()
    {
        Validation();
    }

    protected override void RegisterListeners()
    {
        visitHangarButton.onClick.AddListener(VisitHangar);
        profileButton.onClick.AddListener(ProfileWindow);
        highScoresButton.onClick.AddListener(HighScores);
        settingsButton.onClick.AddListener(Settings);
        exitGameButton.onClick.AddListener(ExitGame);
        newProfileButton.onClick.AddListener(NewProfile);
        inputConfirmButton.onClick.AddListener(ConfirmProfileInput);
        returnToMenuButton.onClick.AddListener(ReturnToMenu);
    }

    #region Private Methods
    private void Validation()
    {
        if(ProfileManager.instance.currentProfile.profileName == string.Empty)
        {
            TextMeshProUGUI text = profileButton.GetComponent<TextMeshProUGUI>();
            text.text = "Select Profile";

            visitHangarButton.interactable = false;
            highScoresButton.interactable = false;
        }
        else 
        {
            TextMeshProUGUI text = profileButton.GetComponent<TextMeshProUGUI>();
            text.text = "View Profile";

            visitHangarButton.interactable = true;
            highScoresButton.interactable = true;

            currentProfileText.text = "Current Profile: " + ProfileManager.instance.currentProfile.profileName;
            balanceText.text = "Balance: " + ProfileManager.instance.currentProfile.balance.ToString();
            reputationText.text = "Reputation: " + ProfileManager.instance.currentProfile.reputation.ToString();
        }


    }

    private void VisitHangar()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");

        // Temporarily routes to level one
        GameManager.instance.LoadLevel(GameStates.LevelOne);
        
    }

    private void ProfileWindow()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
        introMenuCanvas.gameObject.SetActive(false);
        profileScreenCanvas.gameObject.SetActive(true);
    }

    private void HighScores()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
    }

    private void Settings()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
    }

    private void NewProfile()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
        newProfileButton.gameObject.SetActive(false);
        newProfileInput.gameObject.SetActive(true);
        newProfileInput.ActivateInputField();
    }

    private void ConfirmProfileInput()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
        string name = newProfileInput.text;
        Debug.Log("The player has entered " + name);
        newProfileInput.DeactivateInputField(true);
        newProfileInput.gameObject.SetActive(false);
        newProfileButton.gameObject.SetActive(true);


        ProfileManager.instance.CreateNewProfile(name);
        // TO DO: Trigger the creation of a new profile JSON file, set as active
    }

    private void ReturnToMenu()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
        profileScreenCanvas.gameObject.SetActive(false);
        introMenuCanvas.gameObject.SetActive(true);
    }

    private void ExitGame()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
        Application.Quit();
    }

    private void RefreshProfileData()
    {

    }
    #endregion
}
