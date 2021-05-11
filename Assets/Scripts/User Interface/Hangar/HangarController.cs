using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HangarController : MonoBehaviour
{
    public static HangarController instance;

    [SerializeField]
    private HangarUI hangarUi;

    [Header("Game Data")]
    public ResourceValues resources;

    private void Awake()
    {
        #region Singleton
        HangarController[] list = FindObjectsOfType<HangarController>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the HangarController component detected. Destroying an instance.");
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
        hangarUi.RegisterListeners();
        ActivateUI();
    }

    public void ActivateUI()
    {
        if(GameManager.instance.levelRecord != null)
        {
            CalculateResources();
            hangarUi.SetScreen(HangarUIScreen.EndLevel, true);
            hangarUi.SetScreen(HangarUIScreen.Main, false);
            SellResources();
        }
        else
        {
            EventManager.TriggerEvent("UpdateBalance");
            Debug.Log("Event triggered.");
            hangarUi.SetScreen(HangarUIScreen.Main, true);
        }
    }

    #region Private Methods
    private void RegisterListeners()
    {
    }

    private void CalculateResources()
    {
        LevelRecord record = GameManager.instance.levelRecord;

        record.ironTotalValue = record.ironCollected * resources.ironValue;
        record.silverTotalValue = record.silverCollected * resources.silverValue;
        record.goldTotalValue = record.goldCollected * resources.goldValue;
    }

    private void SellResources()
    {
        Debug.Log("Sell Resources called");
        LevelRecord record = GameManager.instance.levelRecord;

        float total = record.ironTotalValue + record.silverTotalValue + record.goldTotalValue;
        ProfileManager.instance.currentProfile.balance += total;
        ProfileManager.instance.SaveProfile();
        GameManager.instance.levelRecord = null;
        EventManager.TriggerEvent("UpdateBalance");
    }
    #endregion

    private void TestingMode()
    {

    }
}
