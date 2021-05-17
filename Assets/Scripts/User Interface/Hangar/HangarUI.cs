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
    public GameObject weaponSlot;
    public GameObject shieldSlot;
    public GameObject armourSlot;
    public GameObject engineSlot;
    public GameObject thrusterSlot;
    public GameObject hullSlot;
    public GameObject utilitySlot;
    public GameObject trashIcon;
    public GameObject equipmentItemPrefab;

    [Header("Zone Select Screen")]
    public GameObject zoneSelectScreen;
    public Button asteroidZoneButton;
    public Button nebulaZoneButton;
    public Button blackHoleZoneButton;
    public Button zoneCloseButton;

    [Header("Events")]
    private UnityAction updateBalanceDelegate;
    private UnityAction itemPurchasedDelegate;

    #region Unity Methods
    private void Start()
    {
        
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

        itemPurchasedDelegate = RefreshInventory;
        EventManager.StartListening("ItemPurchased", itemPurchasedDelegate);
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
        Transform[] items = itemsParent.GetComponentsInChildren<Transform>();

        foreach(Transform child in items)
        {
            Destroy(child);
        }

        foreach(Equipment item in EquipmentManager.instance.playerInventory)
        {
            GameObject uiItem = Instantiate(equipmentItemPrefab, itemsParent.transform);
            List<Image> images = new List<Image>(uiItem.GetComponentsInChildren<Image>());
            images[1].sprite = item.EquipmentIcon;   
        }


    }

    #endregion
}

public enum HangarUIScreen { None, Main, EndLevel, Equipment }