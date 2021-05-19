﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class HangarUI : MonoBehaviour
{
    [Header("Main Navigation")]
    public GameObject mainHud;
    public TextMeshProUGUI balanceText;
    public Button zoneSelectButton;
    public Button shipFitoutButton;
    public Button mainMenuButton;
    public Button exitGameButton;
    public List<GameObject> screenList;

    [Header("End Level Screen")]
    public GameObject endLevelScreen;
    public TextMeshProUGUI ironText;
    public TextMeshProUGUI silverText;
    public TextMeshProUGUI goldText;
    public Button endLevelOkButton;

    [Header("Equipment and Inventory")]
    public GameObject equipmentScreen;
    public Button buyShield;
    public Button buyArmour;
    public Button buyHull;
    public Button buyEngine;
    public Button buyThruster;
    public Button buyWeapon;
    public Button equipCloseButton;
    public GameObject statsParent;
    public GameObject itemsParent;
    public List<EquipmentSlot> equipmentSlots;
    public GameObject trashIcon;
    public GameObject equipmentItemPrefab;
    public GameObject statsItemPrefab;

    [Header("Zone Select Screen")]
    public GameObject zoneSelectScreen;
    public Button asteroidZoneButton;
    public Button nebulaZoneButton;
    public Button blackHoleZoneButton;
    public Button zoneCloseButton;

    [Header("Game Data")]
    public PlayerStats stats;

    [Header("Events")]
    private UnityAction updateBalanceDelegate;
    private UnityAction itemPurchasedDelegate;
    private UnityAction updateInventoryDelegate;
    private UnityAction updateStatsDelegate;
    private UnityAction updateEquipmentSlotsDelegate;

    #region Unity Methods
    private void Start()
    {
        RefreshInventory();
        RefreshEquipment();
    }
    #endregion

    #region Public Methods
    public void RegisterListeners()
    {
        // Buttons
        endLevelOkButton.onClick.AddListener(delegate { SetScreen("NavigationMenu"); });
        zoneSelectButton.onClick.AddListener(delegate { SetScreen("ZoneSelectScreen"); });
        shipFitoutButton.onClick.AddListener(delegate { SetScreen("EquipmentScreen"); });
        mainMenuButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(GameStates.IntroMenu); });
        exitGameButton.onClick.AddListener(delegate { Utility.QuitGame(); });

        buyShield.onClick.AddListener(delegate { EquipmentManager.instance.GenerateItem(EquipmentType.Shield); });
        buyArmour.onClick.AddListener(delegate { EquipmentManager.instance.GenerateItem(EquipmentType.Armour); });
        buyHull.onClick.AddListener(delegate { EquipmentManager.instance.GenerateItem(EquipmentType.Hull); });
        buyEngine.onClick.AddListener(delegate { EquipmentManager.instance.GenerateItem(EquipmentType.Engine); });
        buyThruster.onClick.AddListener(delegate { EquipmentManager.instance.GenerateItem(EquipmentType.Maneuvering); });
        buyWeapon.onClick.AddListener(delegate { EquipmentManager.instance.GenerateItem(EquipmentType.Weapon); });
        equipCloseButton.onClick.AddListener(delegate { SetScreen("NavigationMenu"); });

        asteroidZoneButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(GameStates.AsteroidField); });
        nebulaZoneButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(GameStates.Nebula); });
        blackHoleZoneButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(GameStates.BlackHoles); });
        zoneCloseButton.onClick.AddListener(delegate { SetScreen("NavigationMenu"); });

        // Custom Events
        updateBalanceDelegate = UpdateBalance;
        EventManager.StartListening("UpdateBalance", updateBalanceDelegate);

        updateInventoryDelegate = RefreshInventory;
        EventManager.StartListening("UpdateInventory", updateInventoryDelegate);        

        updateEquipmentSlotsDelegate = RefreshEquipment;
        EventManager.StartListening("UpdateEquipmentSlots", updateEquipmentSlotsDelegate);

        updateStatsDelegate = UpdateStats;
        EventManager.StartListening("UpdateStats", updateStatsDelegate);
    }

    public void SetScreen(string tag)
    {
        Debug.Log("Setting screen to " + tag);

        foreach(GameObject item in screenList)
        {
            if(item.CompareTag(tag))
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
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

    public void SetEndLevelText()
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

        if (record.silverCollected > 0)
        {
            silverText.text = $"Silver: {record.silverCollected} - ${record.silverTotalValue}";
            silverText.enabled = true;
        }
        else
        {
            silverText.enabled = false;
        }

        if (record.goldCollected > 0)
        {
            goldText.text = $"Gold: {record.goldCollected} - ${record.goldTotalValue}";
            goldText.enabled = true;
        }
        else
        {
            goldText.enabled = false;
        }

        SetScreen("EndLevelScreen");
    }

    #endregion

    #region Private Methods

    private void RefreshInventory()
    {
        Debug.Log("Refreshing inventory");
        foreach (Transform child in itemsParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Equipment item in EquipmentManager.instance.playerInventory)
        {
            GameObject uiItem = Instantiate(equipmentItemPrefab, itemsParent.transform);
            uiItem.tag = "EquipmentItem";
            List<Image> images = new List<Image>(uiItem.GetComponentsInChildren<Image>());
            images[1].sprite = item.EquipmentIcon;

            AssociatedEquipment assEquipment = uiItem.GetComponent<AssociatedEquipment>();
            assEquipment.equipment = item;
        }
    }

    private void RefreshEquipment()
    {
        // Delete any existing child objects that aren't the placeholder image
        foreach(EquipmentSlot slot in equipmentSlots)
        {
            List<Transform> children = new List<Transform>(slot.GetComponentsInChildren<Transform>());
            children.Remove(slot.transform);

            foreach(Transform child in children)
            {
                if(!child.CompareTag("DontDestroy"))
                {
                    Destroy(child.gameObject);
                }
            }
        }

        // Creates new UI items and assigns the relevant equipment from the player equipment list
        foreach(Equipment equipment in EquipmentManager.instance.playerEquipment)
        {
            EquipmentSlot thisSlot = null;

            foreach(EquipmentSlot slot in equipmentSlots)
            {
                if(equipment.EquipmentType == slot.slotType)
                {
                    thisSlot = slot;
                    break;
                }
            }

            if(thisSlot != null)
            {
                thisSlot.placeholderImage.SetActive(false);
                GameObject uiItem = Instantiate(equipmentItemPrefab, thisSlot.transform);
                AssociatedEquipment uiEquipment = uiItem.GetComponent<AssociatedEquipment>();
                uiEquipment.equipment = equipment;
                Image icon = uiItem.transform.GetChild(0).GetComponent<Image>();
                icon.sprite = uiEquipment.equipment.equipmentProfile.equipmentIcon;
                uiItem.tag = "EquipmentSlot";
            }
            else
            {
                Debug.LogError("Error in allocating an equipment UI item to an equipment slot.");
            }
        }

        EventManager.TriggerEvent("UpdateInventory");
        EquipmentManager.instance.RecalcEquipmentEffects();
    }

    private void ClearStats()
    {
        foreach(Transform child in statsParent.transform)
        {
            if(!child.CompareTag("DontDestroy"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void UpdateStats()
    {
        ClearStats();

        foreach(Equipment equipment in EquipmentManager.instance.playerEquipment)
        {
            foreach(EquipmentEffect effect in equipment.effects)
            {
                GameObject statItem = Instantiate(statsItemPrefab, statsParent.transform);
                TextMeshProUGUI statText = statItem.GetComponent<TextMeshProUGUI>();

                string sign;

                if(effect.effectStrength > 0f)
                {
                    sign = "+";
                }
                else
                {
                    sign = string.Empty;
                }

                EffectType effectType = effect.profile.effectType;
                string currentValue = "ERROR";
                string currentValueSign = "ERROR";

                switch (effectType)
                {
                    case EffectType.ArmourCap:
                        currentValue = stats.currentMaxArmour.ToString("#.#");
                        currentValueSign = " points";
                        break;
                    case EffectType.CollectorRadius:
                        currentValue = stats.currentCollectionRange.ToString("#.#");
                        currentValueSign = " metres";
                        break;
                    case EffectType.EngineThrust:
                        currentValue = stats.currentForwardThrust.ToString("#.#");
                        currentValueSign = " joules";
                        break;
                    case EffectType.EngineVelocityCap:
                        currentValue = stats.currentMaximumVelocity.ToString("#.#");
                        currentValueSign = " metres p/sec";
                        break;
                    case EffectType.HullCap:
                        currentValue = stats.currentMaxHull.ToString("#.#");
                        currentValueSign = " points";
                        break;
                    case EffectType.Luck:
                        currentValue = stats.currentLuck.ToString("#.#");
                        currentValueSign = "x";
                        break;
                    case EffectType.ManeuveringSpeed:
                        currentValue = stats.currentManeuveringSpeed.ToString("#.#");
                        currentValueSign = "x";
                        break;
                    case EffectType.ProfitBoost:
                        currentValue = stats.currentProfitBoost.ToString("#.#");
                        currentValueSign = "x";
                        break;
                    case EffectType.ShieldCap:
                        currentValue = stats.currentMaxShields.ToString("#.#");
                        currentValueSign = " points";
                        break;
                    case EffectType.ShieldCooldown:
                        currentValue = stats.currentShieldCooldownTime.ToString("#.#");
                        currentValueSign = " seconds";
                        break;
                    case EffectType.ShieldRegen:
                        currentValue = stats.currentShieldRegen.ToString("#.#");
                        currentValueSign = " points p/sec";
                        break;
                    case EffectType.ProjectileDamage:
                        currentValue = stats.currentProjectileDamage.ToString("#.#");
                        currentValueSign = " points";
                        break;
                    case EffectType.ProjectileSpeed:
                        currentValue = stats.currentProjectileSpeed.ToString("#.#");
                        currentValueSign = " meters p/sec";
                        break;
                }

                statText.text = $"{effect.profile.description} {sign}{effect.effectStrength.ToString("#.#")} {effect.profile.unitOfMeasurement} (Now {currentValue}{currentValueSign})";
            }
        }
    }

    #endregion
}

public enum HangarUIScreen { None, Main, EndLevel, Equipment }