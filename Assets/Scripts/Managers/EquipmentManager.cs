using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    [Header("References")]
    public PlayerStats stats;

    [Header("Generation")]
    [SerializeField]
    private List<EquipmentProfile> equipmentProfiles;
    [SerializeField]
    private AnimationCurve strengthRarityCurve;

    [Header("Data")]
    [SerializeField]
    public List<Equipment> playerInventory;
    public List<Equipment> playerEquipment;

    #region Unity Methods
    private void Awake()
    {
        #region Singleton
        EquipmentManager[] list = FindObjectsOfType<EquipmentManager>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the EquipmentManager component detected. Destroying an instance.");
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
        GetPlayerData();
    }
    #endregion

    #region Public Methods
    public void ApplyEquipmentEffects()
    {
        foreach (Equipment equipment in playerEquipment)
        {
            equipment.ApplyEffects();
        }
    }

    public void EquipItem(Equipment equipment)
    {
        // Removes any item already in the slot
        foreach(Equipment item in playerEquipment.ToArray())
        {
            if(item.EquipmentType == equipment.EquipmentType)
            {
                UnequipItem(item, false);
            }
        }

        playerInventory.Remove(equipment);
        playerEquipment.Add(equipment);
        equipment.isEquipped = true;
        ProfileManager.instance.SaveProfile();

        EventManager.TriggerEvent("UpdateEquipmentSlots");
        EventManager.TriggerEvent("UpdateInventory");
    }

    public void UnequipItem(Equipment equipment, bool triggerEvents)
    {
        playerInventory.Add(equipment);
        playerEquipment.Remove(equipment);
        equipment.isEquipped = false;
        ProfileManager.instance.SaveProfile();

        if(triggerEvents)
        {
            EventManager.TriggerEvent("UpdateEquipmentSlots");
            EventManager.TriggerEvent("UpdateInventory");
        }
    }

    /// <summary>
    /// Overload method for removing equipment without needing to specify whether events should be triggered
    /// </summary>
    /// <param name="equipment">The equipment item to be unequipped.</param>
    public void UnequipItem(Equipment equipment)
    {
        UnequipItem(equipment, false);
    }

    public bool BuyItem(EquipmentType type)
    {
        if(PerformTransaction(type))
        {
            // Creates a new empty equipment object, assigns a random profile to determine which type it will become, and gives it a name.
            Equipment newModule = new Equipment();

            for (int i = 0; i < equipmentProfiles.Count; i++)
            {
                if (equipmentProfiles[i].equipmentType == type)
                {
                    newModule.equipmentProfile = equipmentProfiles[i];
                    break;
                }
            }

            if (newModule.equipmentProfile == null)
            {
                Debug.LogError("There was an error in generating an item. newModule.equipmentProfile should not be null.");
                return false;
            }

            // TO DO: Pick name from a list of pre-generated names
            newModule.name = newModule.equipmentProfile.equipmentType.ToString();

            // Determine the effects this module should provide as according to the random profile picked and assigned.
            // Add guaranteed effects as defined in equipment profile.
            foreach (EquipmentEffectProfile effectProfile in newModule.equipmentProfile.guaranteedEffects)
            {
                // Generate the strength of the effect and add the effect to the equipment module's effects list
                EquipmentEffect thisEffect = GenerateNewEffect(effectProfile, newModule);
                thisEffect.wasGuaranteed = true;
            }

            // Test if secondary effect(s) should be added (based on their chance value)
            foreach (EquipmentEffectProfile effectProfile in newModule.equipmentProfile.possibleSecondaryEffects)
            {
                float randomFloat = Utility.GenerateRandomFloat(0, 100);
                if (effectProfile.chanceOfBeingAdded >= randomFloat)
                {
                    GenerateNewEffect(effectProfile, newModule);
                }
            }

            playerInventory.Add(newModule);
            UpdateModulePrice(newModule.EquipmentType);
            ProfileManager.instance.SaveProfile();
            EventManager.TriggerEvent("ItemPurchased");
            EventManager.TriggerEvent("UpdateInventory");
            EventManager.TriggerEvent("UpdateBalance");

            return true;
        }
        else
        {
            EventManager.TriggerEvent("CantAffordItem");
            return false;
        }
        
    }

    public void RecalcEquipmentEffects()
    {
        stats.ResetStats();
        ApplyEquipmentEffects();
        stats.SetInitialStats();
        EventManager.TriggerEvent("UpdateStats");
    }

    public void DestroyEquipment(Equipment equipment)
    {
        playerEquipment.Remove(equipment);
        playerInventory.Remove(equipment);

        ProfileManager.instance.SaveProfile();
        EventManager.TriggerEvent("UpdateInventory");
        EventManager.TriggerEvent("UpdateEquipmentSlots");
        RecalcEquipmentEffects();
    }
    #endregion

    #region Private Methods
    private void RegisterListeners()
    {
    }

    private void GetPlayerData()
    {
        playerEquipment = ProfileManager.instance.currentProfile.currentEquipment;
        playerInventory = ProfileManager.instance.currentProfile.currentInventory;
    }

    private EquipmentEffect GenerateNewEffect(EquipmentEffectProfile effectProfile, Equipment newModule)
    {
        // Get a value from the curve.
        float rarityValue = strengthRarityCurve.Evaluate(Utility.GenerateRandomFloat(0f, 1f));
        
        // Find out the difference between the max strength and min strength before multiplying this difference by the item rarity.
        float thisItemRarity = (effectProfile.maxStrength - effectProfile.minStrength) * rarityValue;

        // The strength of our item is now the minimum value + the rarity determination
        float strength = effectProfile.minStrength + thisItemRarity;
        
        // Create the new effect object by applying the profile and strength.
        EquipmentEffect newEffect = new EquipmentEffect(effectProfile, strength, rarityValue);
        //Debug.Log($"Generating new effect named {effectProfile.name} with strength {strength} at {rarityValue * 100}% rarity.");
        
        // Add the effect to the list of effects for the equipment module.
        newModule.effects.Add(newEffect);
        return newEffect;
    }

    private bool PerformTransaction(EquipmentType type)
    {
        PlayerProfile profile = ProfileManager.instance.currentProfile;
        bool hasFunds = false;

        switch (type)
        {
            case EquipmentType.Armour:
                hasFunds = TakeFromBalance(profile.armourModPrice);
                break;
            case EquipmentType.Engine:
                hasFunds = TakeFromBalance(profile.engineModPrice);
                break;
            case EquipmentType.Hull:
                hasFunds = TakeFromBalance(profile.hullModPrice);
                break;
            case EquipmentType.Maneuvering:
                hasFunds = TakeFromBalance(profile.thrusterModPrice);
                break;
            case EquipmentType.Shield:
                hasFunds = TakeFromBalance(profile.shieldModPrice);
                break;
            case EquipmentType.Weapon:
                hasFunds = TakeFromBalance(profile.weaponModPrice);
                break;
        }

        return hasFunds;
    }

    private bool TakeFromBalance(float modPrice)
    {
        PlayerProfile profile = ProfileManager.instance.currentProfile;

        if (modPrice < profile.balance)
        {
            profile.balance -= modPrice;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateModulePrice(EquipmentType type)
    {
        PlayerProfile profile = ProfileManager.instance.currentProfile;

        switch(type)
        {
            case EquipmentType.Armour:
                profile.armourModPrice *= 1.1f;
                break;
            case EquipmentType.Engine:
                profile.engineModPrice *= 1.1f;
                break;
            case EquipmentType.Hull:
                profile.hullModPrice *= 1.1f;
                break;
            case EquipmentType.Maneuvering:
                profile.thrusterModPrice *= 1.1f;
                break;
            case EquipmentType.Shield:
                profile.shieldModPrice *= 1.1f;
                break;
            case EquipmentType.Weapon:
                profile.weaponModPrice *= 1.1f;
                break;
        }

        EventManager.TriggerEvent("UpdateModulePrices");
        ProfileManager.instance.SaveProfile();
    }

    #endregion
}
