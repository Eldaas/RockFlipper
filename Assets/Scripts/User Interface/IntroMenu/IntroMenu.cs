using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class IntroMenu : UIController
{
    public static IntroMenu instance;

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
    private Transform profileScreenContent;
    [SerializeField]
    private Button newProfileButton;
    [SerializeField]
    private Button returnToMenuButton;
    [SerializeField]
    private TextMeshProUGUI currentProfileText;
    [SerializeField]
    private Button profileScreenCloseButton;
    [SerializeField]
    private GameObject profileRecordPrefab;
    public ProfileRecord selectedProfileRecord;
    private bool alreadyLoaded = false;

    [Header("New Profile Modal")]
    [SerializeField]
    private GameObject newProfileModal;
    [SerializeField]
    private TMP_InputField newProfileModalInput;
    [SerializeField]
    private Button newProfileModalConfirmButton;
    [SerializeField]
    private Button newProfileModalCloseButton;

    [Header("Events")]
    private UnityAction profileLoadedDelegate;

    private void Awake()
    {
        instance = this;
        RegisterListeners();
    }

    private void Start()
    {
        PopulateProfileList();
    }

    private void Update()
    {
        Validation();
    }

    protected override void RegisterListeners()
    {
        // Buttons
        visitHangarButton.onClick.AddListener(VisitHangar);
        profileButton.onClick.AddListener(ProfileWindow);
        highScoresButton.onClick.AddListener(HighScores);
        settingsButton.onClick.AddListener(Settings);
        exitGameButton.onClick.AddListener(ExitGame);
        newProfileButton.onClick.AddListener(NewProfile);
        newProfileModalConfirmButton.onClick.AddListener(ConfirmProfileInput);
        returnToMenuButton.onClick.AddListener(ReturnToMenu);
        profileScreenCloseButton.onClick.AddListener(ReturnToMenu);
        newProfileModalCloseButton.onClick.AddListener(delegate { newProfileModal.SetActive(false); });

        // Custom Events
        profileLoadedDelegate = UpdateProfileWindow;
        EventManager.StartListening("ProfileLoaded", profileLoadedDelegate);
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
            visitHangarButton.interactable = true;
            highScoresButton.interactable = true;

            currentProfileText.text = "Current Loaded Profile: " + ProfileManager.instance.currentProfile.profileName;
        }
    }

    private void VisitHangar()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
        GameManager.instance.LoadLevel(GameStates.Hangar);
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
        GameManager.instance.LoadLevel(GameStates.HighScores);
    }

    private void Settings()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
    }

    private void NewProfile()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
        newProfileModal.gameObject.SetActive(true);
        newProfileModalInput.ActivateInputField();
    }

    private void ConfirmProfileInput()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
        string name = newProfileModalInput.text;
       
        if(name != string.Empty)
        {
            Debug.Log("The player has entered " + name);
            newProfileModalInput.text = string.Empty;

            if (ProfileManager.instance.CreateNewProfile(name))
            {
                newProfileModal.SetActive(false);
                GameManager.instance.LoadLevel(GameStates.Hangar);
            }
            else
            {
                EventManager.TriggerEvent("InvalidProfileName");
            }
        }
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

    private void UpdateProfileWindow()
    {
        currentProfileText.text = $"Current Loaded Profile: {ProfileManager.instance.currentProfile.profileName}";
    }

    private void PopulateProfileList()
    {
        ClearProfileList();
        if(ProfileManager.instance.profileList.Count > 0)
        {
            foreach (PlayerProfile profile in ProfileManager.instance.profileList)
            {
                GameObject uiObj = Instantiate(profileRecordPrefab, profileScreenContent);
                ProfileRecord record = uiObj.GetComponent<ProfileRecord>();
                record.nameField.text = profile.profileName;
                record.statsField.text = $"Balance: {Math.Truncate(profile.balance)}\nSaved On: {profile.lastSaved}";
            }
        }
    }

    private void ClearProfileList()
    {
        Debug.Log("Clearing profiles.");
        foreach(Transform child in profileScreenContent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion
}
