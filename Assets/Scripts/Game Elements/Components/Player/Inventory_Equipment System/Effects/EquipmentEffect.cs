using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// EquipmentEffect is the runtime data container that generates according to the effect profiles assigned to the parent module via the inspector.
/// </summary>
public class EquipmentEffect
{
    public EquipmentEffect(EquipmentEffectProfile inputProfile, float inputEffectStrength, float rarity)
    {
        profile = inputProfile;
        effectStrength = inputEffectStrength;
        effectRarityValue = rarity;
        TranslateRarity();
    }

    public EquipmentEffectProfile profile;
    public float effectStrength;
    public float effectRarityValue;
    public Rarity effectRarity;

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
                stats.maxArmourEquipment += effectStrength;
                Debug.Log($"maxArmourEquipment is now {stats.maxArmourEquipment}");
                break;
            case EffectType.CollectorRadius:
                stats.collectionRangeEquipment += effectStrength;
                Debug.Log($"collectionRangeEquipment is now {stats.collectionRangeEquipment}");
                break;
            case EffectType.EngineThrust:
                stats.forwardThrustEquipment += effectStrength;
                Debug.Log($"forwardThrustEquipment is now {stats.forwardThrustEquipment}");
                break;
            case EffectType.EngineVelocityCap:
                stats.maximumVelocityEquipment += effectStrength;
                Debug.Log($"maximumVelocityEquipment is now {stats.maximumVelocityEquipment}");
                break;
            case EffectType.HullCap:
                stats.maxHullEquipment += effectStrength;
                Debug.Log($"maxHullEquipment is now {stats.maxHullEquipment}");
                break;
            case EffectType.Luck:
                stats.luckEquipment += effectStrength;
                Debug.Log($"luckEquipment is now {stats.luckEquipment}");
                break;
            case EffectType.ManeuveringSpeed:
                stats.maneuveringSpeedEquipment += effectStrength;
                Debug.Log($"maneuveringSpeedEquipment is now {stats.maneuveringSpeedEquipment}");
                break;
            case EffectType.ProfitBoost:
                stats.profitBoostEquipment += effectStrength;
                Debug.Log($"profitBoostEquipment is now {stats.profitBoostEquipment}");
                break;
            case EffectType.ShieldCap:
                stats.maxShieldsEquipment += effectStrength;
                Debug.Log($"maxShieldsEquipment is now {stats.maxShieldsEquipment}");
                break;
            case EffectType.ShieldCooldown:
                stats.shieldCooldownTimeEquipment += effectStrength;
                Debug.Log($"shieldCooldownTimeEquipment is now {stats.shieldCooldownTimeEquipment}");
                break;
            case EffectType.ShieldRegen:
                stats.shieldRegenEquipment += effectStrength;
                Debug.Log($"shieldRegenEquipment is now {stats.shieldRegenEquipment}");
                break;
            case EffectType.ProjectileDamage:
                stats.projectileDamageEquipment += effectStrength;
                break;
            case EffectType.ProjectileSpeed:
                stats.projectileSpeedEquipment += effectStrength;
                break;
        }
    }
    #endregion

    #region Private Methods
    private void TranslateRarity()
    {
        if (effectRarityValue >= 0f && effectRarityValue <= 0.35f) effectRarity = Rarity.Common;
        else if (effectRarityValue > 0.35f && effectRarityValue <= 0.60f) effectRarity = Rarity.Uncommon;
        else if (effectRarityValue > 0.60f && effectRarityValue <= 0.85f) effectRarity = Rarity.Rare;
        else if (effectRarityValue > 0.85f) effectRarity = Rarity.Legendary;
    }
    #endregion
}

public enum Rarity { Common, Uncommon, Rare, Legendary }
