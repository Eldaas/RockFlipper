using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/Equipment Effect Profile")]
public class EquipmentEffectProfile : ScriptableObject
{
    public EffectType effectType;
    public float minStrength;
    public float maxStrength;
    public bool isRare;
}

public enum EffectType { None, ShieldCap, ShieldRegen, ShieldCooldown, ArmourCap, ArmourRegen, HullCap, HullRegen, EngineVelocityCap, EngineThrust, ManeuveringSpeed, CollectorRadius, ProfitBoost, Luck }