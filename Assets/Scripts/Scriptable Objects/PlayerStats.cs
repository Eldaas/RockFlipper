using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data/Player Stats Container")]
public class PlayerStats : ScriptableObject
{
    // "Base" values are used to set initial values via the inspector, which are then applied at runtime.
    // ReadOnly fields aren't intended to be altered in the inspector at runtime due to being dynamically calculated

    /// <summary>
    /// Base max hull represents the starting amount the hull should have, before modifiers are taken into account
    /// </summary>
    public float baseMaxHull;

    /// <summary>
    /// Current hull represents the player's current (live) hull value
    /// </summary>
    [ReadOnly] public float currentHull;

    /// <summary>
    /// Current max hull represents the current maximum capacity of the player's hull health value, taking modifiers into account
    /// </summary>
    [ReadOnly] public float currentMaxHull;

    /// <summary>
    /// Base max armour represents the starting amount the armour should have, before modifiers are taken into account
    /// </summary>
    public float baseMaxArmour;

    /// <summary>
    /// Current armour represents the player's current (live) armour value
    /// </summary>
    [ReadOnly] public float currentArmour;

    /// <summary>
    /// Current max armour represents the current maximum capacity of the player's armour health value, taking modifiers into account
    /// </summary>
    [ReadOnly] public float currentMaxArmour;

    /// <summary>
    /// Base max shields represents the starting amount the shield should have, before modifiers are taken into account
    /// </summary>
    public float baseMaxShields;

    /// <summary>
    /// Current shields represents the player's current (live) shields value
    /// </summary>
    [ReadOnly] public float currentShields;

    /// <summary>
    /// Current max shields represents the current maximum capacity of the player's shields health value, taking modifiers into account
    /// </summary>
    [ReadOnly] public float currentMaxShields;

    /// <summary>
    /// Base shield regen is how much the player's shield will regenerate per second before modifiers are applied
    /// </summary>
    public float baseShieldRegen;

    /// <summary>
    /// Current shield regen is the current (live) rate at which the player's shield regenerates per second, with modifiers taken into account
    /// </summary>
    [ReadOnly] public float currentShieldRegen;

    /// <summary>
    /// Base shield cooldown time is the amount of seconds between the shield being destroyed and the time at which it begins regenerating, without taking modifiers into account
    /// </summary>
    public float baseShieldCooldownTime;

    /// <summary>
    /// Current shield cooldown time is the current amount of seconds between the shield being destroyed and the time at which it begins regenerating, taking modifiers into account
    /// </summary>
    [ReadOnly] public float currentShieldCooldownTime;

    /// <summary>
    /// Base forward thrust is the starting thrust at which the player moves forward in the game level, without modifiers taken into account. Thrust is the physics force applied to the player object
    /// </summary>
    public float baseForwardThrust;

    /// <summary>
    /// Current forward thrust is the thrust at which the player is currently moving forward in the game level. Thrust is the physics force applied to the player object
    /// </summary>
    public float currentForwardThrust;

    /// <summary>
    /// This is the base value for the player's maximum velocity, before modifiers are taken into account.
    /// </summary>
    public float baseMaximumVelocity;

    /// <summary>
    /// This is the current value for the player's maximum velocity, with modifiers taken into account
    /// </summary>
    public float currentMaximumVelocity;

    /// <summary>
    /// Base maneuvering speed pertains to how fast the player can move left and right to avoid collisions, without modifiers taken into account
    /// </summary>
    public float baseManeuveringSpeed;

    /// <summary>
    /// Current maneuvering speed pertains to how fast the player can currently move left and right to avoid collisions, with modifiers taken into account
    /// </summary>
    [ReadOnly] public float currentManeuveringSpeed;

    /// <summary>
    /// The base heat sink capacity is the unmodified capacity the player starts with
    /// </summary>
    public float baseHeatSinkCapacity;

    /// <summary>
    /// This represents a normalised value (0 - 1) to represent the current % saturation of the heat sink capacity
    /// </summary>
    [ReadOnly] public float currentHeatSinkLevel;

    /// <summary>
    /// The current heat sink capacity is the current maximum value that the heat sink can reach, with modifiers taken into account
    /// </summary>
    [ReadOnly] public float currentHeatSinkCapacity;





}
