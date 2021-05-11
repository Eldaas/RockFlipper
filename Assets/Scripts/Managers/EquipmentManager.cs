using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentManager : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField]
    private int numToGenerate;
    [SerializeField]
    private int refreshEveryXSeconds;
    [SerializeField]
    private List<EquipmentProfile> equipmentProfiles;
    [SerializeField]
    private AnimationCurve strengthRarityCurve;
    [SerializeField]
    private AnimationCurve numEffectsCurve;

    [Header("Data")]
    public List<IEquippable> shopEquipment;
    public List<IEquippable> playerEquipment;
    
    [Header("Events")]
    private UnityAction generateShopItemsDelegate;

    #region Unity Methods
    private void Awake()
    {
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
            Equipment newModule = new Equipment();
            newModule.equipmentProfile = equipmentProfiles[Utility.GenerateRandomInt(0, equipmentProfiles.Count - 1)];
            // TO DO: Pick name from a list of pre-generated names
            newModule.name = newModule.equipmentProfile.equipmentType.ToString();

            // Add guaranteed effects as defined in equipment profile
            foreach(EquipmentEffectProfile effect in newModule.equipmentProfile.guaranteedEffects)
            {
                // Generate the strength of the effect and add the effect to the equipment module's effects list
                int strength = Mathf.RoundToInt(Utility.GenerateRandomFloat(effect.minStrength, effect.maxStrength) * strengthRarityCurve.Evaluate(Utility.GenerateRandomFloat(0, 1)));
                EquipmentEffect newEffect = new EquipmentEffect(effect, strength);
                newModule.effects.Add(newEffect);
            }


            float effectCountMod = numEffectsCurve.Evaluate(Utility.GenerateRandomFloat(0, 1));
            int effectCount = Mathf.RoundToInt(Utility.GenerateRandomFloat(0, newModule.equipmentProfile.possibleSecondaryEffects.Count - 1) * effectCountMod);

            for (int i = 0; i < effectCount; i++)
            {
                
            }

            // Randomly selects and transposes effects from the profile to the active equipment
            float effectStrength = strengthRarityCurve.Evaluate(Utility.GenerateRandomFloat(0f, 1f));




        }
    }
    #endregion



}
