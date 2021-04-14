using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data/Player Health Container")]
public class PlayerHealth : ScriptableObject
{
    public float maxHull;
    public float maxArmour;
    public float maxShields;

    // Shield regen is per second
    public float baseShieldRegen;

}
