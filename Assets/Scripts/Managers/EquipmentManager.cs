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
    private int numToGenerate;
    [SerializeField]
    private int refreshEveryXSeconds;
    [SerializeField]
    private List<EquipmentProfile> equipmentProfiles;
    [SerializeField]
    private AnimationCurve strengthRarityCurve;

    [Header("Data")]
    [SerializeField]
    public List<Equipment> shopEquipment;
    public List<Equipment> playerEquipment;

    [Header("Events")]
    private UnityAction generateShopItemsDelegate;

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
        ClearShopEquipment();
        GenerateShopEquipment();
        EquipPlayer();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void RegisterListeners()
    {
        generateShopItemsDelegate = GenerateShopEquipment;
        EventManager.StartListening("GenerateShopItems", generateShopItemsDelegate);
    }

    private void GenerateShopEquipment()
    {
        shopEquipment = ProfileManager.instance.currentProfile.shopEquipment;

        while (shopEquipment.Count < numToGenerate)
        {
            // Creates a new empty equipment object, assigns a random profile to determine which type it will become, and gives it a name.
            Equipment newModule = new Equipment();
            newModule.equipmentProfile = equipmentProfiles[Utility.GenerateRandomInt(0, equipmentProfiles.Count - 1)];
            // TO DO: Pick name from a list of pre-generated names
            newModule.name = newModule.equipmentProfile.equipmentType.ToString();

            // Determine the effects this module should provide as according to the random profile picked and assigned.
            // Add guaranteed effects as defined in equipment profile.
            foreach (EquipmentEffectProfile effectProfile in newModule.equipmentProfile.guaranteedEffects)
            {
                // Generate the strength of the effect and add the effect to the equipment module's effects list
                GenerateNewEffect(effectProfile, newModule);
            }

            // Test if secondary effect(s) should be added (based on their chance value)
            foreach (EquipmentEffectProfile effectProfile in newModule.equipmentProfile.possibleSecondaryEffects)
            {
                float randomFloat = Utility.GenerateRandomFloat(0, 100);
                if (effectProfile.chanceOfBeingAdded <= randomFloat)
                {
                    GenerateNewEffect(effectProfile, newModule);
                }
            }

            shopEquipment.Add(newModule);
        }

        ProfileManager.instance.currentProfile.shopEquipment = shopEquipment;
        ProfileManager.instance.SaveProfile();
    }

    private void GenerateNewEffect(EquipmentEffectProfile effectProfile, Equipment newModule)
    {
        // Get a value from the curve.
        float rarityValue = strengthRarityCurve.Evaluate(Utility.GenerateRandomFloat(0f, 1f));
        
        // Find out the difference between the max strength and min strength before multiplying this difference by the item rarity.
        float thisItemRarity = (effectProfile.maxStrength - effectProfile.minStrength) * rarityValue;

        // The strength of our item is now the minimum value + the rarity determination
        float strength = effectProfile.minStrength + thisItemRarity;
        
        // Create the new effect object by applying the profile and strength.
        EquipmentEffect newEffect = new EquipmentEffect(effectProfile, strength, rarityValue);
        Debug.Log($"Generating new effect named {effectProfile.name} with strength {strength} at {rarityValue * 100}% rarity.");
        
        // Add the effect to the list of effects for the equipment module.
        newModule.effects.Add(newEffect);
    }

    private void EquipPlayer()
    {
        playerEquipment.Add(shopEquipment[0]);

        foreach(Equipment equipment in playerEquipment)
        {
            equipment.Equip();
        }
        
    }

    private void ClearShopEquipment()
    {
        shopEquipment.Clear();
        ProfileManager.instance.currentProfile.shopEquipment.Clear();
        ProfileManager.instance.SaveProfile();
    }
    #endregion



}
