using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Level Modifier Profile")]
public class LevelModifier : ScriptableObject
{
    [Header("Core Modifiers")]
    public float takenDamageMultiplier;
    public float dealtDamageMultiplier;
    public float speedMultiplier;

    [Header("Health Modifiers")]
    public float shieldsMultiplier;
    public float shieldsRegenMultiplier;
    public float armourMultiplier;
    public float hullMultiplier;



}
