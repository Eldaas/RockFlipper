using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EquipmentEffect is the runtime data container that generates according to the effect profiles assigned to the parent module via the inspector.
/// </summary>
public class EquipmentEffect : MonoBehaviour
{
    public EquipmentEffect(EquipmentEffectProfile inputProfile, int inputEffectStrength)
    {
        profile = inputProfile;
        effectStrength = inputEffectStrength;
    }

    public EquipmentEffectProfile profile;
    public int effectStrength;
}
