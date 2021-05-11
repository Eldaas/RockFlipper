using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data/Player Stats Container")]
public class PlayerStats : ScriptableObject
{
    // "Base" values are used to set initial values via the inspector, which are then applied at runtime.
    // "Current" values are generally autocalculated values incorporating both equipment and powerup modifiers.
    // ReadOnly fields aren't intended to be altered in the inspector due to being dynamically calculated.

    /// <summary>
    /// Base max hull represents the inspector-set starting amount the hull should have
    /// </summary>
    public float baseMaxHull;

    /// <summary>
    /// Current hull represents the player's current (live) hull value
    /// </summary>
    [ReadOnly] public float currentHull;

    /// <summary>
    /// Current max hull represents the current maximum capacity of the player's hull health value
    /// </summary>
    [ReadOnly] public float currentMaxHull;

    /// <summary>
    /// Base max armour represents the inspector-set starting amount the armour should have
    /// </summary>
    public float baseMaxArmour;

    /// <summary>
    /// Current armour represents the player's current (live) armour value
    /// </summary>
    [ReadOnly] public float currentArmour;

    /// <summary>
    /// Current max armour represents the current maximum capacity of the player's armour health value
    /// </summary>
    [ReadOnly] public float currentMaxArmour;

    /// <summary>
    /// Base max shields represents the inspector-set starting amount the shield should have
    /// </summary>
    public float baseMaxShields;

    /// <summary>
    /// Current shields represents the player's current (live) shields value
    /// </summary>
    [ReadOnly] public float currentShields;

    /// <summary>
    /// Current max shields represents the current maximum capacity of the player's shields health value
    /// </summary>
    [ReadOnly] public float currentMaxShields;

    /// <summary>
    /// Base shield regen is the inspector-set value for how much the player's shield will regenerate per second
    /// </summary>
    public float baseShieldRegen;

    /// <summary>
    /// Current shield regen is the current (live) rate at which the player's shield regenerates per second
    /// </summary>
    [ReadOnly] public float currentShieldRegen;

    /// <summary>
    /// Base shield cooldown time is the inspector-set number of seconds between the shield being destroyed and the time at which it begins regenerating
    /// </summary>
    public float baseShieldCooldownTime;

    /// <summary>
    /// Current shield cooldown time is the current amount of seconds between the shield being destroyed and the time at which it begins regenerating
    /// </summary>
    [ReadOnly] public float currentShieldCooldownTime;

    /// <summary>
    /// Base forward thrust is the inspector-set starting thrust at which the player moves forward in the game level. Thrust is the physics force applied to the player object.
    /// </summary>
    public float baseForwardThrust;

    /// <summary>
    /// Current forward thrust is the thrust at which the player is currently moving forward in the game level. Thrust is the physics force applied to the player object
    /// </summary>
    [ReadOnly] public float currentForwardThrust;

    /// <summary>
    /// This is the inspector-set base value for the player's maximum velocity
    /// </summary>
    public float baseMaximumVelocity;

    /// <summary>
    /// This is the current value for the player's maximum velocity
    /// </summary>
    [ReadOnly] public float currentMaximumVelocity;

    /// <summary>
    /// This caps the velocity to an amount that the player absolutely cannot go beyond (otherwise they will keep on accelerating ad infinitum).
    /// </summary>
    public float velocityCap;

    /// <summary>
    /// Base maneuvering speed is the inspector-set value for how fast the player can move horizontally (left and right).
    /// </summary>
    public float baseManeuveringSpeed;

    /// <summary>
    /// Current maneuvering speed is the current (live) value for how fast the player can move horizontally (left and right).
    /// </summary>
    [ReadOnly] public float currentManeuveringSpeed;

    /// <summary>
    /// The base heat sink capacity is the inspector-set value for how many 'points of health' the heat sink has.
    /// </summary>
    public float baseHeatSinkCapacity;

    /// <summary>
    /// This is a normalised value (0 - 1) to represent the current % saturation of the heat sink capacity.
    /// </summary>
    [ReadOnly] public float currentHeatSinkLevel;

    /// <summary>
    /// The current heat sink capacity is the current maximum 'points of health' value that the heat sink can reach.
    /// </summary>
    [ReadOnly] public float currentHeatSinkCapacity;

    /// <summary>
    /// Base projectile speed represents the base value (without modifiers applied) at which the player's cannon projectile will travel at.
    /// </summary>
    public float baseProjectileSpeed;

    /// <summary>
    /// Current projectile speed represents the current (live) value at which the player's cannon projectiles will travel at.
    /// </summary>
    [ReadOnly] public float currentProjectileSpeed;

    /// <summary>
    /// Base projectile damage represents the inspector-set starting amount of damage that the player deals to asteroids.
    /// </summary>
    public float baseProjectileDamage;

    /// <summary>
    /// Current projectile damage represents the current amount of damage that the player deals to asteroids.
    /// </summary>
    [ReadOnly] public float currentProjectileDamage;

    /// <summary>
    /// Base collection range is the inspector-set distance from which resource chunks will be pulled toward the player ship.
    /// </summary>
    public float baseCollectionRange;

    /// <summary>
    /// Current collection range is the current (live) distance from which resource chunks will be pulled toward the player ship.
    /// </summary>
    [ReadOnly] public float currentCollectionRange;
}
