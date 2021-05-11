using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/Equipment Effect Profile")]
public class EquipmentEffectProfile : ScriptableObject
{
    public EffectType effectType;
    public float minStrength;
    public float maxStrength;

    /// <summary>
    /// This is the chance the effect has of being added as a secondary effect, as a value from 0 to 100 (representing a percentage).
    /// </summary>
    public float chanceOfBeingAdded;

    

}

public enum EffectType { None, ShieldCap, ShieldRegen, ShieldCooldown, ArmourCap, ArmourRegen, HullCap, HullRegen, EngineVelocityCap, EngineThrust, ManeuveringSpeed, CollectorRadius, ProfitBoost, Luck }