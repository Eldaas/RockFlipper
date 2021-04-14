using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data/Player Stats Container")]
public class PlayerStats : ScriptableObject
{
    // Current hull represents the player's current (live) hull value
    public float currentHull;

    // Max hull represents the maximum capacity of the player's hull health value
    public float maxHull;

    // Current armour represents the player's current (live) armour value
    public float currentArmour;

    // Max armour represents the maximum capacity of the player's armour health value
    public float maxArmour;

    // Current shields represents the player's current (live) shields value
    public float currentShields;

    // Max shields represents the maximum capacity of the player's shields health value
    public float maxShields;

    // Current shield regen is the current (live) rate at which the player's shield regenerates per second
    public float currentShieldRegen;

    // Base shield regen is how much the player's shield will regenerate per second, before level modifiers are applied
    public float baseShieldRegen;

    // Speed mitigation slows the player down
    public float speedMitigation;

    // Maneuvering speed pertains to how fast the player can move left and right to avoid collisions
    public float maneuveringSpeed;

    // Heating sink capacity is the maximum value that the heat sink can reach
    public float heatSinkCapacity;

    



}
