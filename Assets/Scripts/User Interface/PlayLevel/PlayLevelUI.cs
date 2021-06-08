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

    [Header("Player Stat Bars")]
    [SerializeField]
    private RadialSegmentedHealthBar energyBar;
    [SerializeField]
    private TextMeshProUGUI energyBarText;
    [SerializeField]
    private RadialSegmentedHealthBar shieldBar;
    [SerializeField]
    private TextMeshProUGUI shieldBarText;
    [SerializeField]
    private RadialSegmentedHealthBar armourBar;
    [SerializeField]
    private TextMeshProUGUI armourBarText;
    [SerializeField]
    private RadialSegmentedHealthBar hullBar;
    [SerializeField]
    private TextMeshProUGUI hullBarText;
    [SerializeField]
    private float activeAlphaValue;
    [SerializeField]
    private float inactiveAlphaValue;

    [Header("Powerups")]
    public Transform powerupsParent;

    [Header("Resources Panel")]
    [SerializeField]
    private GameObject resourcesPanel;
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

    [Header("Misc")]
    private Player player;
    private PlayerStats stats;

    [Header("Events")]
    private UnityAction updateResourceDelegate;

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
    public IEnumerator BlinkIcon(Image image, float time)
    {
        float trackedTime = time;
        Color colour = image.color;
        float counter = 0f;
        
        while(trackedTime > Mathf.Epsilon)
        {
            trackedTime -= Time.deltaTime;
            counter += 0.1f;

            colour.a = Mathf.PingPong(counter, 1f);
            image.color = colour;
            yield return new WaitForEndOfFrame();
        }  
    }
    #endregion

    #region Protected & Private Methods
    protected override void RegisterListeners()
    {
        androidHomeButton.onClick.AddListener(ReturnToBase);
        androidShootButton.onClick.AddListener(Shoot);
        androidMenuButton.onClick.AddListener(PauseMenu);

        updateResourceDelegate = UpdateResourcesCount;
        EventManager.StartListening("ResourceCollected", updateResourceDelegate);
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
        UpdateStat(energyBar, energyBarText, stats.currentBatteryLevel, stats.currentBatteryCapacity);
        //UpdateStat(armourBar, armourBarText, stats.currentArmour, stats.currentMaxArmour);
        //UpdateStat(hullBar, hullBarText, stats.currentHull, stats.currentMaxHull); ;
        //UpdateStat(shieldBar, shieldBarText, stats.currentShields, stats.currentMaxShields);
    }

    private void UpdateStat(RadialSegmentedHealthBar bar, TextMeshProUGUI text, float currValue, float maxValue)
    {
        float percentage = currValue / maxValue;
        Debug.Log(percentage);
        float uiPercentage = Mathf.SmoothStep(bar.RemovedSegments.Value, percentage, 0.1f);
        bar.SetPercent(uiPercentage);
        text.text = Math.Truncate(percentage * 100) + "%";
        Color color = bar.InnerColor.Value;
        Color textColour = text.color;

        if (currValue < maxValue)
        {
            color.a = activeAlphaValue;
            textColour.a = activeAlphaValue;
        }
        else
        {
            color.a = inactiveAlphaValue;
            textColour.a = inactiveAlphaValue;
        }

        bar.InnerColor.Value = color;
        text.color = textColour;
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
        UIManager.instance.ShowPauseMenu();
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

#endregion

}
