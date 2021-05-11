using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EquipmentEffect is the runtime data container that generates according to the effect profiles assigned to the parent module via the inspector.
/// </summary>
public class EquipmentEffect
{
    public EquipmentEffect(EquipmentEffectProfile inputProfile, int inputEffectStrength)
    {
        profile = inputProfile;
        effectStrength = inputEffectStrength;
    }

    public EquipmentEffectProfile profile;
    public int effectStrength;

    #region Effect Implementation
    /// <summary>
    /// Checks the effect type and applies the relevant modifiers to the player's stats.
    /// </summary>
    public void ApplyEffect()
    {
        PlayerStats stats = EquipmentManager.instance.stats;
        EffectType effectType = profile.effectType;

        switch(effectType)
        {
            case EffectType.ArmourCap:
                stats.baseMaxArmour += effectStrength;
                break;
            case EffectType.ArmourRegen:
                // NOT IMPLEMENTED IN PLAYER STATS
                break;
            case EffectType.CollectorRadius:
                stats.baseCollectionRange += effectStrength;
                break;
            case EffectType.EngineThrust:
                stats.baseForwardThrust += effectStrength;
                break;
            case EffectType.EngineVelocityCap:
                stats.baseMaximumVelocity += effectStrength;
                break;
            case EffectType.HullCap:
                stats.baseMaxHull += effectStrength;
                break;
            case EffectType.HullRegen:
                // NOT IMPLEMENTED IN PLAYER STATS
                break;
            case EffectType.Luck:
                // NOT IMPLEMENTED IN PLAYER STATS
                break;
            case EffectType.ManeuveringSpeed:
                stats.baseManeuveringSpeed += effectStrength;
                break;
            case EffectType.ProfitBoost:
                // NOT IMPLEMENTED IN PLAYER STATS
                break;
            case EffectType.ShieldCap:
                stats.baseMaxShields += effectStrength;
                break;
            case EffectType.ShieldCooldown:
                stats.baseShieldCooldownTime += effectStrength;
                break;
            case EffectType.ShieldRegen:
                stats.baseShieldRegen += effectStrength;
                break;
        }

    }
    #endregion
}
