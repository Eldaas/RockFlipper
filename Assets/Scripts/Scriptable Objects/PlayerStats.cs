using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data/Player Stats Container")]
public class PlayerStats : ScriptableObject
{
    // "Base" values are used to set initial values via the inspector, which are then applied at runtime.
    // "Equipment" values are what the player's current equipment provides.
    // "Powerup" values are what the player's current active powerup effects provide.
    // "Current" values are the player's stat values incorporating both equipment and powerup modifiers.
    // "ReadOnly" fields aren't intended to be altered in the inspector due to being dynamically calculated.

    #region Hull Stats

    public float baseMaxHull;
    [ReadOnly] public float maxHullEquipment;
    [ReadOnly] public float maxHullPowerup;
    [ReadOnly] public float currentHull;
    public float CurrentMaxHull { get => baseMaxHull + maxHullEquipment + maxHullPowerup; }

    #endregion


    #region Armour Stats

    public float baseMaxArmour;
    [ReadOnly] public float maxArmourEquipment;
    [ReadOnly] public float maxArmourPowerup;
    [ReadOnly] public float currentArmour;
    public float CurrentMaxArmour { get => baseMaxArmour + maxArmourEquipment + maxArmourPowerup; }

    #endregion


    #region Shield Stats

    #region Shield Cap

    public float baseMaxShields;
    [ReadOnly] public float maxShieldsEquipment;
    [ReadOnly] public float maxShieldsPowerup;
    [ReadOnly] public float currentShields;
    public float CurrentMaxShields { get => baseMaxShields + maxShieldsEquipment + maxShieldsPowerup; }

    #endregion


    #region Shield Regen

    public float baseShieldRegen;
    [ReadOnly] public float shieldRegenEquipment;
    [ReadOnly] public float shieldRegenPowerup;
    public float CurrentShieldRegen { get => baseShieldRegen + shieldRegenEquipment + shieldRegenPowerup; }
    
    #endregion


    #region Shield Cooldown

    public float baseShieldCooldownTime;
    [ReadOnly] public float shieldCooldownTimeEquipment;
    [ReadOnly] public float shieldCooldownTimePowerup;
    public float CurrentShieldCooldownTime { get => baseShieldCooldownTime + shieldCooldownTimeEquipment + shieldCooldownTimePowerup; }

    #endregion

    #endregion


    #region Movement Stats

    public float baseForwardThrust;
    [ReadOnly] public float forwardThrustEquipment;
    [ReadOnly] public float forwardThrustPowerup;
    public float CurrentForwardThrust { get => baseForwardThrust * forwardThrustEquipment * forwardThrustPowerup; }

    public float hardVelocityCap;
    public float baseMaximumVelocity;
    [ReadOnly] public float maximumVelocityEquipment;
    [ReadOnly] public float maximumVelocityPowerup;
    [ReadOnly] public float maximumVelocityIncrementor;
    public float CurrentMaximumVelocity { get => baseMaximumVelocity + maximumVelocityEquipment + maximumVelocityPowerup + maximumVelocityIncrementor; }
    
    public float baseManeuveringSpeed;
    [ReadOnly] public float maneuveringSpeedEquipment;
    [ReadOnly] public float maneuveringSpeedPowerup;
    public float CurrentManeuveringSpeed { get => baseManeuveringSpeed + maneuveringSpeedEquipment + maneuveringSpeedPowerup; }

    public float baseBatteryCapacity;
    [ReadOnly] public float batteryCapacityEquipment;
    [ReadOnly] public float batteryCapacityPowerup;
    [ReadOnly] public float currentBatteryLevel;
    public float CurrentBatteryCapacity { get => baseBatteryCapacity + batteryCapacityEquipment + batteryCapacityPowerup; }

    #endregion


    #region Projectiles

    public float baseProjectileSpeed;
    [ReadOnly] public float projectileSpeedEquipment;
    [ReadOnly] public float projectileSpeedPowerup;
    public float CurrentProjectileSpeed { get => baseProjectileSpeed + projectileSpeedEquipment + projectileSpeedPowerup; }

    public float baseProjectileDamage;
    [ReadOnly] public float projectileDamageEquipment;
    [ReadOnly] public float projectileDamagePowerup;
    public float CurrentProjectileDamage { get => baseProjectileDamage + projectileDamageEquipment + projectileDamagePowerup; }

    #endregion


    #region Collection Stats

    public float baseCollectionRange;
    [ReadOnly] public float collectionRangeEquipment;
    [ReadOnly] public float collectionRangePowerup;
    public float CurrentCollectionRange { get => baseCollectionRange + collectionRangeEquipment + collectionRangePowerup; }

    #endregion

    #region Miscellaneous Stats
    public float baseLuck;
    [ReadOnly] public float luckEquipment;
    [ReadOnly] public float luckPowerup;

    public float baseProfitBoost;
    [ReadOnly] public float profitBoostEquipment;
    [ReadOnly] public float profitBoostPowerup;
    #endregion

    #region Public Methods
    public void ResetPowerupStats()
    {
        maxHullPowerup = 0f;
        maxArmourPowerup = 0f;
        maxShieldsPowerup = 0f;
        shieldRegenPowerup = 0f;
        shieldCooldownTimePowerup = 0f;
        forwardThrustPowerup = 0f;
        maximumVelocityPowerup = 0f;
        maneuveringSpeedPowerup = 0f;
        batteryCapacityPowerup = 0f;
        projectileSpeedPowerup = 0f;
        projectileDamagePowerup = 0f;
        collectionRangePowerup = 0f;
        luckPowerup = 0f;
        profitBoostPowerup = 0f;
    }
    #endregion

}
