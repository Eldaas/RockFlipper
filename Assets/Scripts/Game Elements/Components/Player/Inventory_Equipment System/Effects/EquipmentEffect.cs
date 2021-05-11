using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EquipmentEffect is the runtime data container that generates according to the effect profiles assigned to the parent module via the inspector.
/// </summary>
public class EquipmentEffect
{
    public EquipmentEffect(EquipmentEffectProfile inputProfile, float inputEffectStrength, float rarity)
    {
        profile = inputProfile;
        effectStrength = inputEffectStrength;
        effectRarity = rarity;
    }

    public EquipmentEffectProfile profile;
    public float effectStrength;
    public float effectRarity;

    #region Effect Implementation
    /// <summary>
    /// Checks the effect type and applies the relevant modifiers to the player's stats.
    /// </summary>
    public void ApplyEffect()
    {
        PlayerStats stats = EquipmentManager.instance.stats;
        EffectType effectType = profile.effectType;
        Debug.Log("Applying effect type " + effectType.ToString() + " with strength " + effectStrength);

        switch(effectType)
        {
            case EffectType.ArmourCap:
                stats.maxArmourEquipment += effectStrength;
                break;
            case EffectType.CollectorRadius:
                stats.collectionRangeEquipment += effectStrength;
                break;
            case EffectType.EngineThrust:
                stats.forwardThrustEquipment = (stats.baseForwardThrust + stats.forwardThrustEquipment) * ((100 - effectStrength) / 100);
                break;
            case EffectType.EngineVelocityCap:
                stats.maximumVelocityEquipment += effectStrength;
                break;
            case EffectType.HullCap:
                stats.maxHullEquipment += effectStrength;
                break;
            case EffectType.Luck:
                stats.luckEquipment += effectStrength;
                break;
            case EffectType.ManeuveringSpeed:
                stats.maneuveringSpeedEquipment += effectStrength;
                break;
            case EffectType.ProfitBoost:
                stats.profitBoostEquipment += effectStrength;
                break;
            case EffectType.ShieldCap:
                stats.maxShieldsEquipment += effectStrength;
                break;
            case EffectType.ShieldCooldown:
                stats.shieldCooldownTimeEquipment += effectStrength;
                break;
            case EffectType.ShieldRegen:
                stats.shieldRegenEquipment += effectStrength;
                break;
        }

    }
    #endregion
}
