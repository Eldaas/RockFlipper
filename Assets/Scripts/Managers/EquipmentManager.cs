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
        if(TakeFromBalance(type))
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

    public float CalcModulePrice(EquipmentType type)
    {
        PlayerProfile profile = ProfileManager.instance.currentProfile;
        ProgressionMapping progMap = GameManager.instance.progMap;
        int clampedPurchases = 0;

        switch (type)
        {
            case EquipmentType.Armour:
                clampedPurchases = Mathf.Clamp(profile.armourModValueIndex, 0, progMap.equipIndexRate);
                Debug.Log($"clampedPurchases: {clampedPurchases}, armourModValueIndex: {profile.armourModValueIndex}");
                break;
            case EquipmentType.Engine:
                clampedPurchases = Mathf.Clamp(profile.engineModValueIndex, 0, progMap.equipIndexRate);
                break;
            case EquipmentType.Hull:
                clampedPurchases = Mathf.Clamp(profile.hullModValueIndex, 0, progMap.equipIndexRate);
                break;
            case EquipmentType.Maneuvering:
                clampedPurchases = Mathf.Clamp(profile.thrusterModValueIndex, 0, progMap.equipIndexRate);
                break;
            case EquipmentType.Shield:
                clampedPurchases = Mathf.Clamp(profile.shieldModValueIndex, 0, progMap.equipIndexRate);
                break;
            case EquipmentType.Weapon:
                clampedPurchases = Mathf.Clamp(profile.weaponModValueIndex, 0, progMap.equipIndexRate);
                break;
        }

        float percentage = (float)clampedPurchases / (float)progMap.equipIndexRate;
        float curveValue = GameManager.instance.progMap.equipmentCostScale.Evaluate(percentage);
        float modPrice = progMap.maxPrice * curveValue;

        return modPrice;
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

    private bool TakeFromBalance(EquipmentType type)
    {
        PlayerProfile profile = ProfileManager.instance.currentProfile;
        float modPrice = CalcModulePrice(type);

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
                profile.armourModValueIndex++;
                break;
            case EquipmentType.Engine:
                profile.engineModValueIndex++;
                break;
            case EquipmentType.Hull:
                profile.hullModValueIndex++;
                break;
            case EquipmentType.Maneuvering:
                profile.thrusterModValueIndex++;
                break;
            case EquipmentType.Shield:
                profile.shieldModValueIndex++;
                break;
            case EquipmentType.Weapon:
                profile.weaponModValueIndex++;
                break;
        }

        EventManager.TriggerEvent("UpdateModulePrices");
        ProfileManager.instance.SaveProfile();
    }

    #endregion
}
