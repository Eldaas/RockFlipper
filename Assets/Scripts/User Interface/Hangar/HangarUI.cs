using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class HangarUI : MonoBehaviour
{
    [Header("Main Navigation")]
    public GameObject mainHud;
    public Button returnToLevelButton;
    public TextMeshProUGUI balanceText;
    public Button quitGameButton;
    public Button mainMenuButton;

    [Header("End Level Screen")]
    public GameObject endLevelScreen;
    public TextMeshProUGUI ironText;
    public TextMeshProUGUI silverText;
    public TextMeshProUGUI goldText;
    public Button endLevelOkButton;

    [Header("Events")]
    private UnityAction updateBalanceDelegate;


    #region Unity Methods
    private void Start()
    {

    }
    #endregion

    #region Public Methods
    public void RegisterListeners()
    {
        // Buttons
        endLevelOkButton.onClick.AddListener(delegate { SetScreen(HangarUIScreen.EndLevel, false); });
        returnToLevelButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(GameStates.AsteroidField); });
        quitGameButton.onClick.AddListener(delegate { Utility.QuitGame(); });
        mainMenuButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(GameStates.IntroMenu); });

        // Custom Events
        updateBalanceDelegate = UpdateBalance;
        EventManager.StartListening("UpdateBalance", updateBalanceDelegate);
        Debug.Log("Listeners registered");
    }

    public void SetScreen(HangarUIScreen screen, bool active)
    {
        if(screen == HangarUIScreen.Main)
        {
            if(active) mainHud.SetActive(true);
            else mainHud.SetActive(false);
        }
        else if(screen == HangarUIScreen.EndLevel)
        {
            if(active) SetEndLevelText();
            else
            {
                endLevelScreen.SetActive(false);
                mainHud.SetActive(true);
            }
        }   
    }

    public void UpdateBalance()
    {
        Debug.Log("Update balance called");
        if(ProfileManager.instance.currentProfile.balance != 0f)
        {
            balanceText.text = $"Balance: ${ProfileManager.instance.currentProfile.balance.ToString("#,#")}";
        }
        else
        {
            balanceText.text = "Balance: $0";
        }
        
    }

    #endregion

    #region Private Methods
    private void SetEndLevelText()
    {
        LevelRecord record = GameManager.instance.levelRecord;

        if (record.ironCollected > 0)
        {
            ironText.text = $"Iron: {record.ironCollected} - ${record.ironTotalValue}";
            ironText.enabled = true;
        }
        else
        {
            ironText.enabled = false;
        }

        if(record.silverCollected > 0)
        {
            silverText.text = $"Silver: {record.silverCollected} - ${record.silverTotalValue}";
            silverText.enabled = true;
        }
        else
        {
            silverText.enabled = false;
        }

        if(record.goldCollected > 0)
        {
            goldText.text = $"Gold: {record.goldCollected} - ${record.goldTotalValue}";
            goldText.enabled = true;
        }
        else
        {
            goldText.enabled = false;
        }

        endLevelScreen.SetActive(true);
    }

    #endregion
}

public enum HangarUIScreen { None, Main, EndLevel }