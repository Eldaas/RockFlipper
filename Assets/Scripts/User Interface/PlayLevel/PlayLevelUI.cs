using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using RengeGames.HealthBars;

public class PlayLevelUI : UIController
{
    [Header("Build Target UI")]
    [SerializeField]
    private GameObject androidUi;
    [SerializeField]
    private GameObject windowsUi;
    [SerializeField]
    private GameObject universalUi;

    [Header("Player Stat Bars")]
    [SerializeField]
    private GameObject barsPanel;
    [SerializeField]
    private RadialSegmentedHealthBar energyBar;
    [SerializeField]
    private TextMeshProUGUI energyBarPercentageText;
    [SerializeField]
    private TextMeshProUGUI energyBarValueText;
    [SerializeField]
    private RadialSegmentedHealthBar shieldBar;
    [SerializeField]
    private TextMeshProUGUI shieldBarPercentageText;
    [SerializeField]
    private TextMeshProUGUI shieldBarValueText;
    [SerializeField]
    private RadialSegmentedHealthBar armourBar;
    [SerializeField]
    private TextMeshProUGUI armourBarPercentageText;
    [SerializeField]
    private TextMeshProUGUI armourBarValueText;
    [SerializeField]
    private RadialSegmentedHealthBar hullBar;
    [SerializeField]
    private TextMeshProUGUI hullBarPercentageText;
    [SerializeField]
    private TextMeshProUGUI hullBarValueText;
    [SerializeField]
    private float activeAlphaValue;
    [SerializeField]
    private float inactiveAlphaValue;

    [Header("Powerups")]
    public Transform powerupsParent;

    [Header("Resources Panel")]
    [SerializeField]
    private GameObject ironPanel;
    [SerializeField]
    private TextMeshProUGUI ironText;
    [SerializeField]
    private GameObject silverPanel;
    [SerializeField]
    private TextMeshProUGUI silverText;
    [SerializeField]
    private GameObject goldPanel;
    [SerializeField]
    private TextMeshProUGUI goldText;

    [Header("Buttons")]
    [SerializeField]
    private Button androidHomeButton;
    [SerializeField]
    private Button androidShootButton;
    [SerializeField]
    private Button androidMenuButton;

    [Header("Top Notification Panel")]
    [SerializeField]
    private float tnpTime;
    [SerializeField]
    private Transform tnpParent;
    [SerializeField]
    private GameObject tnpPrefab;

    [Header("Screen Space Notification Panel")]
    [SerializeField]
    private Transform ssnpParent;
    [SerializeField]
    private GameObject ssnpPrefab;

    [Header("Notification Colours")]
    [SerializeField]
    private Color red;
    [SerializeField]
    private Color green;
    [SerializeField]
    private Color yellow;
    [SerializeField]
    private Color blue;

    [Header("Misc")]
    private Player player;
    private PlayerStats stats;

    [Header("Events")]
    private UnityAction updateResourceDelegate;
    private UnityAction playerDeathDelegate;
    private UnityAction shieldOfflineDelegate;
    private UnityAction armourDestroyedDelegate;
    private UnityAction hullLowDelegate;
    private UnityAction lowEnergyDelegate;
    private UnityAction noEnergyDelegate;

    private void Awake()
    {
        RegisterListeners();
        player = SceneController.instance.player;
        stats = player.stats;
    }

    private void Start()
    {
        Initialise();
        UpdateResourcesCount();
    }

    private void LateUpdate()
    {
        UpdateStatBars();
    }

    #region Public Methods
    
    #endregion

    #region Protected & Private Methods
    protected override void RegisterListeners()
    {
        // Buttons
        androidHomeButton.onClick.AddListener(ReturnToBase);
        androidShootButton.onClick.AddListener(Shoot);
        androidMenuButton.onClick.AddListener(PauseMenu);

        // Custom events
        updateResourceDelegate = UpdateResourcesCount;
        EventManager.StartListening("ResourceCollected", updateResourceDelegate);

        playerDeathDelegate = PlayerDeath;
        EventManager.StartListening("PlayerDeath", playerDeathDelegate);

        shieldOfflineDelegate = delegate { NewNotification("SHIELD OFFLINE", blue); };
        EventManager.StartListening("ShieldDestroyed", shieldOfflineDelegate);

        armourDestroyedDelegate = delegate { NewNotification("ARMOUR DESTROYED", yellow); };
        EventManager.StartListening("ArmourDestroyed", armourDestroyedDelegate);

        hullLowDelegate = delegate { NewNotification("HULL LOW!", red); };
        EventManager.StartListening("HealthLow", hullLowDelegate);

        lowEnergyDelegate = delegate { NewNotification("LOW ENERGY", green); };
        EventManager.StartListening("EnergyLow", lowEnergyDelegate);

        noEnergyDelegate = delegate { NewNotification("NO ENERGY LEFT", red); };
        EventManager.StartListening("BatteryIsEmpty", noEnergyDelegate);
    }

    private void Initialise()
    {
        ClearResourcesPanel();
        ClearPowerupIcons();

#if UNITY_ANDROID
        androidUi.SetActive(true);
        windowsUi.SetActive(false);
#endif

#if UNITY_STANDALONE_WIN
        androidUi.SetActive(false);
        windowsUi.SetActive(true);
#endif
    }

    private void ReturnToBase()
    {
        EventManager.TriggerEvent("UISelect");
        UIManager.instance.LoadScreen(true);
        EventManager.TriggerEvent("ReturningToBase");
        StartCoroutine("EndScene");
    }

    private IEnumerator EndScene()
    {
        while(UIManager.instance.LoadScreenAlpha > Mathf.Epsilon)
        {
            yield return new WaitForEndOfFrame();
        }

        GameManager.instance.LoadLevel(GameStates.Hangar);
    }

    private void UpdateStatBars()
    {
        UpdateStat(energyBar, energyBarPercentageText, energyBarValueText, stats.currentBatteryLevel, stats.currentBatteryCapacity);
        UpdateStat(armourBar, armourBarPercentageText, armourBarValueText, stats.currentArmour, stats.currentMaxArmour);
        UpdateStat(hullBar, hullBarPercentageText, hullBarValueText, stats.currentHull, stats.currentMaxHull); ;
        UpdateStat(shieldBar, shieldBarPercentageText, shieldBarValueText, stats.currentShields, stats.currentMaxShields);
    }

    private void UpdateStat(RadialSegmentedHealthBar bar, TextMeshProUGUI percentageText, TextMeshProUGUI valueText, float currValue, float maxValue)
    {
        float percentage = currValue / maxValue;
        
        float currentRadialValue = 1 - bar.RemovedSegments.Value;
        float uiPercentage = Mathf.SmoothStep(currentRadialValue, percentage, 0.1f);
        bar.SetPercent(uiPercentage);

        percentageText.text = Math.Truncate(percentage * 100) + "%";
        valueText.text = $"{Math.Truncate(currValue)}/{Math.Truncate(maxValue)}";
    }

    private void Shoot()
    {
        EventManager.TriggerEvent("Shoot");
    }

    private void UpdateResourcesCount()
    {
        int ironCollected = GameManager.instance.levelRecord.ironCollected;
        int silverCollected = GameManager.instance.levelRecord.silverCollected;
        int goldCollected = GameManager.instance.levelRecord.goldCollected;

        ironText.text = ironCollected.ToString();
        silverText.text = silverCollected.ToString();
        goldText.text = goldCollected.ToString();

        if(ironCollected > 0)
        {
            ironPanel.SetActive(true);
        }

        if(silverCollected > 0)
        {
            silverPanel.SetActive(true);
        }

        if(goldCollected > 0)
        {
            goldPanel.SetActive(true);
        }
    }

    private void PauseMenu()
    {
        EventManager.TriggerEvent("PauseMenu");
    }

    private void ClearPowerupIcons()
    {
        foreach (Transform powerupIcon in powerupsParent.transform)
        {
            Destroy(powerupIcon.gameObject);
        }
    }

    private void ClearResourcesPanel()
    {
        ironPanel.SetActive(false);
        silverPanel.SetActive(false);
        goldPanel.SetActive(false);
    }

    private void PlayerDeath()
    {
        universalUi.SetActive(false);
        windowsUi.SetActive(false);
        androidUi.SetActive(false);
    }

    /// <summary>
    /// Pass a message to the notification system to be displayed on the top panel.
    /// </summary>
    /// <param name="message">The message to be printed within the panel.</param>
    private void NewNotification(string message, Color color)
    {
        GameObject newUiElement = Instantiate(tnpPrefab, tnpParent);
        TextMeshProUGUI elementText = newUiElement.GetComponentInChildren<TextMeshProUGUI>();
        elementText.text = message;
        elementText.color = color;

        StartCoroutine(ElementTimer(tnpTime, newUiElement));
    }

    private IEnumerator ElementTimer(float time, GameObject element)
    {
        float timeRemaining = time;
        CanvasGroup cg = element.GetComponent<CanvasGroup>();
        float fadeTime = 1f;
        float fadeInRemaining = fadeTime;
        float fadeOutRemaining = fadeTime;

        while (timeRemaining > Mathf.Epsilon)
        {
            timeRemaining -= Time.deltaTime;

            if (fadeInRemaining > Mathf.Epsilon)
            {
                fadeInRemaining -= Time.deltaTime;
                cg.alpha = Mathf.SmoothStep(0f, 1f, 1f - (fadeInRemaining / fadeTime));
            }

            if (timeRemaining < fadeTime)
            {
                fadeOutRemaining -= Time.deltaTime;
                cg.alpha = Mathf.SmoothStep(1f, 0f, 1f - (fadeOutRemaining / fadeTime));
            }

            yield return null;
        }

        Destroy(element);
    }

#endregion

}
