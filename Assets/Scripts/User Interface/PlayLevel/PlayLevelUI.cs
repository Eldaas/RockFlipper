using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class PlayLevelUI : UIController
{
    [Header("Build Target UI")]
    [SerializeField]
    private GameObject androidUi;
    [SerializeField]
    private GameObject windowsUi;

    [Header("Player Stat Bars")]
    [SerializeField]
    private Vector3 statBarsOffset;
    [SerializeField]
    private GameObject playerStatBars;
    [SerializeField]
    private GameObject playerStatBarsParent;
    public GameObject powerupsParent;
    [SerializeField]
    private Image batteryIndicator;
    [SerializeField]
    private Image velocityIndicator;
    [SerializeField]
    private Image hullIndicator;
    [SerializeField]
    private Image armourIndicator;
    [SerializeField]
    private Image shieldsIndicator;

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
        playerStatBars.transform.position = player.transform.position + statBarsOffset;

        // Rotation causes glitching of the world space canvas. Unsure how to fix at present.
            //Vector3 newRotation = new Vector3(0f, 0f, player.transform.eulerAngles.z);
            //playerStatBarsParent.transform.rotation = Quaternion.Euler(newRotation);

        UpdateStat(batteryIndicator, stats.currentBatteryLevel, stats.currentBatteryCapacity);
        UpdateStat(velocityIndicator, player.VelocityZ, stats.currentMaximumVelocity);
        UpdateStat(armourIndicator, stats.currentArmour, stats.currentMaxArmour);
        UpdateStat(hullIndicator, stats.currentHull, stats.currentMaxHull);
        UpdateStat(shieldsIndicator, stats.currentShields, stats.currentMaxShields);
    }

    private void UpdateStat(Image image, float currValue, float maxValue)
    {
        float percentage = currValue / maxValue;
        image.fillAmount = Mathf.SmoothStep(image.fillAmount, percentage, 0.1f);
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
        // TO DO: Write pause menu functionality (procedural GUI)
    }

    private void ClearPowerupIcons()
    {
        foreach(Transform powerupIcon in powerupsParent.transform)
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
