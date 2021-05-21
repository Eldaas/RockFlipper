using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Level Modifier Profile")]
public class LevelModifier : ScriptableObject
{
    // All multipliers use a decimal value, where 1f = no change

    [Header("Core Modifiers")]
    public float asteroidHealthMultiplier;
    public float asteroidDensityMultiplier;
    public float resourcesDroppedMultiplier;

    // These modifiers take a value of 0f to 100f
    [Header("0 - 100% Modifiers")]
    public float ironChance; 
    public float silverChance;
    public float goldChance;
}
