using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public List<IEquippable> shopEquipment;
    public List<IEquippable> playerEquipment;

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
    }

    private void GenerateNewEffect(EquipmentEffectProfile effectProfile, Equipment newModule)
    {
        int strength = Mathf.RoundToInt(Utility.GenerateRandomFloat(effectProfile.minStrength, effectProfile.maxStrength) * strengthRarityCurve.Evaluate(Utility.GenerateRandomFloat(0, 1)));
        EquipmentEffect newEffect = new EquipmentEffect(effectProfile, strength);
        newModule.effects.Add(newEffect);
    }
    #endregion



}
