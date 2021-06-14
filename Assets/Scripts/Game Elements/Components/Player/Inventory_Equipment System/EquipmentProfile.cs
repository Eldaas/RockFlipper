using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Equipment/Equipment Profile"), System.Serializable]
public class EquipmentProfile : ScriptableObject
{
    public EquipmentType equipmentType;
    public Sprite equipmentIcon;

    /// <summary>
    /// Guaranteed effects are always applied, but the strength of the effect is still randomised.
    /// </summary>
    public List<EquipmentEffectProfile> guaranteedEffects = new List<EquipmentEffectProfile>();
    
    /// <summary>
    /// Secondary effects are selected from random, as is the strength of the effect.
    /// </summary>
    public List<EquipmentEffectProfile> possibleSecondaryEffects = new List<EquipmentEffectProfile>();
}
