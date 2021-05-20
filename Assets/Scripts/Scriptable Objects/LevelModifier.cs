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

    // Normalised modifiers require a value between 0f and 1f
    [Header("Normalised Modifiers")]
    public float ironChance; 
    public float silverChance;
    public float goldChance;
}
