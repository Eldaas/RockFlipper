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
    [ReadOnly] public float currentMaxHull; 

    #endregion


    #region Armour Stats

    public float baseMaxArmour;
    [ReadOnly] public float maxArmourEquipment;
    [ReadOnly] public float maxArmourPowerup;
    [ReadOnly] public float currentArmour;
    [ReadOnly] public float currentMaxArmour; 

    #endregion


    #region Shield Stats

    #region Shield Cap

    public float baseMaxShields;
    [ReadOnly] public float maxShieldsEquipment;
    [ReadOnly] public float maxShieldsPowerup;
    [ReadOnly] public float currentShields;
    [ReadOnly] public float currentMaxShields; 

    #endregion


    #region Shield Regen

    public float baseShieldRegen;
    [ReadOnly] public float shieldRegenEquipment;
    [ReadOnly] public float shieldRegenPowerup;
    [ReadOnly] public float currentShieldRegen; 
    
    #endregion


    #region Shield Cooldown

    public float baseShieldCooldownTime;
    [ReadOnly] public float shieldCooldownTimeEquipment;
    [ReadOnly] public float shieldCooldownTimePowerup;
    [ReadOnly] public float currentShieldCooldownTime; 

    #endregion

    #endregion


    #region Movement Stats

    public float baseForwardThrust;
    [ReadOnly] public float forwardThrustEquipment;
    [ReadOnly] public float forwardThrustPowerup;
    [ReadOnly] public float currentForwardThrust; 

    public float hardVelocityCap;
    public float baseMaximumVelocity;
    [ReadOnly] public float maximumVelocityEquipment;
    [ReadOnly] public float maximumVelocityPowerup;
    [ReadOnly] public float maximumVelocityIncrementor;
    [ReadOnly] public float currentMaximumVelocity; 
    
    public float baseManeuveringSpeed;
    [ReadOnly] public float maneuveringSpeedEquipment;
    [ReadOnly] public float maneuveringSpeedPowerup;
    [ReadOnly] public float currentManeuveringSpeed;
    #endregion

    #region Battery Stats

    public float baseBatteryCapacity;
    [ReadOnly] public float batteryCapacityEquipment;
    [ReadOnly] public float batteryCapacityPowerup;
    [ReadOnly] public float currentBatteryLevel;
    [ReadOnly] public float currentBatteryCapacity;

    public float baseBatteryRecharge;
    [ReadOnly] public float batteryRechargeEquipment;
    [ReadOnly] public float batteryRechargePowerup;
    [ReadOnly] public float currentBatteryRecharge;

    #endregion


    #region Projectiles

    public float baseProjectileSpeed;
    [ReadOnly] public float projectileSpeedEquipment;
    [ReadOnly] public float projectileSpeedPowerup;
    [ReadOnly] public float currentProjectileSpeed;

    public float baseProjectileDamage;
    [ReadOnly] public float projectileDamageEquipment;
    [ReadOnly] public float projectileDamagePowerup;
    [ReadOnly] public float currentProjectileDamage;

    #endregion


    #region Collection Stats

    public float baseCollectionRange;
    [ReadOnly] public float collectionRangeEquipment;
    [ReadOnly] public float collectionRangePowerup;
    [ReadOnly] public float currentCollectionRange;

    #endregion


    #region Miscellaneous Stats
    public float baseLuck;
    [ReadOnly] public float luckEquipment;
    [ReadOnly] public float luckPowerup;
    [ReadOnly] public float currentLuck;

    public float baseProfitBoost;
    [ReadOnly] public float profitBoostEquipment;
    [ReadOnly] public float currentProfitBoost;
    #endregion


    #region Public Methods
    public void ResetStats()
    {
        //Debug.Log("Resetting stats.");
        currentMaxHull = 0f;
        maxHullEquipment = 0f;
        maxHullPowerup = 0f;

        currentMaxArmour = 0f;
        maxArmourEquipment = 0f;
        maxArmourPowerup = 0f;

        currentMaxShields = 0f;
        maxShieldsEquipment = 0f;
        maxShieldsPowerup = 0f;

        currentShieldRegen = 0f;
        shieldRegenEquipment = 0f;
        shieldRegenPowerup = 0f;

        currentShieldCooldownTime = 0f;
        shieldCooldownTimeEquipment = 0f;
        shieldCooldownTimePowerup = 0f;

        currentForwardThrust = 0f;
        forwardThrustEquipment = 0f;
        forwardThrustPowerup = 0f;

        currentMaximumVelocity = 0f;
        maximumVelocityEquipment = 0f;
        maximumVelocityPowerup = 0f;
        maximumVelocityIncrementor = 0f;

        currentManeuveringSpeed = 0f;
        maneuveringSpeedEquipment = 0f;
        maneuveringSpeedPowerup = 0f;

        currentBatteryCapacity = 0f;
        batteryCapacityEquipment = 0f;
        batteryCapacityPowerup = 0f;

        currentBatteryRecharge = 0f;
        batteryRechargeEquipment = 0f;
        batteryRechargePowerup = 0f;

        currentProjectileSpeed = 0f;
        projectileSpeedPowerup = 0f;
        projectileSpeedEquipment = 0f;

        currentProjectileDamage = 0f;
        projectileDamageEquipment = 0f;
        projectileDamagePowerup = 0f;

        currentCollectionRange = 0f;
        collectionRangeEquipment = 0f;
        collectionRangePowerup = 0f;

        currentLuck = 0f;
        luckEquipment = 0f;
        luckPowerup = 0f;

        currentProfitBoost = 0f;
        profitBoostEquipment = 0f;
    }

    public void SetInitialStats()
    {
        UpdateStats();
        //Debug.Log("Setting initial stats.");
        currentShields = currentMaxShields;
        currentArmour = currentMaxArmour;
        currentHull = currentMaxHull;
    }

    public void UpdateStats()
    {
        //Debug.Log("Updating stats.");
        currentMaxHull = baseMaxHull + maxHullEquipment + maxHullPowerup;
        currentMaxArmour = baseMaxArmour + maxArmourEquipment + maxArmourPowerup;
        currentMaxShields = baseMaxShields + maxShieldsEquipment + maxShieldsPowerup;
        currentShieldRegen = baseShieldRegen + shieldRegenEquipment + shieldRegenPowerup;

        currentShieldCooldownTime = 0f;
        if(baseShieldCooldownTime != 0f)
        {
            currentShieldCooldownTime = baseShieldCooldownTime;
        }
        if(shieldCooldownTimeEquipment != 0f)
        {
            currentShieldCooldownTime += shieldCooldownTimeEquipment;
        }
        if(shieldCooldownTimePowerup != 0f)
        {
            currentShieldCooldownTime += shieldCooldownTimePowerup;
        }

        currentForwardThrust = 0f;
        if (baseForwardThrust != 0f)
        {
            currentForwardThrust += baseForwardThrust;
        }
        if (forwardThrustEquipment != 0f)
        {
            currentForwardThrust *= forwardThrustEquipment;
        }
        if (forwardThrustPowerup != 0f)
        {
            currentForwardThrust += forwardThrustPowerup;
        }

        currentMaximumVelocity = baseMaximumVelocity + maximumVelocityEquipment + maximumVelocityPowerup + maximumVelocityIncrementor;

        currentManeuveringSpeed = 0f;
        if (baseManeuveringSpeed != 0f)
        {
            currentManeuveringSpeed += baseManeuveringSpeed;
        }
        if (maneuveringSpeedEquipment != 0f)
        {
            currentManeuveringSpeed += maneuveringSpeedEquipment;
        }
        if (maneuveringSpeedPowerup != 0f)
        {
            currentManeuveringSpeed += maneuveringSpeedPowerup;
        }

        currentBatteryCapacity = 0f;
        if (baseBatteryCapacity != 0f)
        {
            currentBatteryCapacity += baseBatteryCapacity;
        }
        if (batteryCapacityEquipment != 0f)
        {
            currentBatteryCapacity *= batteryCapacityEquipment;
        }
        if (batteryCapacityPowerup != 0f)
        {
            currentBatteryCapacity *= batteryCapacityPowerup;
        }

        currentBatteryRecharge = 0f;
        if (baseBatteryRecharge != 0f)
        {
            currentBatteryRecharge += baseBatteryRecharge;
        }
        if (batteryRechargeEquipment != 0f)
        {
            currentBatteryRecharge *= batteryRechargeEquipment;
        }
        if (batteryRechargePowerup != 0f)
        {
            currentBatteryRecharge *= batteryRechargePowerup;
        }

        currentProjectileSpeed = 0f;
        if (baseProjectileSpeed != 0f)
        {
            currentProjectileSpeed += baseProjectileSpeed;
        }
        if (projectileSpeedEquipment != 0f)
        {
            currentProjectileSpeed *= projectileSpeedEquipment;
        }
        if (projectileSpeedPowerup != 0f)
        {
            currentProjectileSpeed *= projectileSpeedPowerup;
        }

        currentProjectileDamage = baseProjectileDamage + projectileDamageEquipment + projectileDamagePowerup;

        currentCollectionRange = 0f;
        if (baseCollectionRange != 0f)
        {
            currentCollectionRange += baseCollectionRange;
        }
        if (collectionRangeEquipment != 0f)
        {
            currentCollectionRange *= (1 + (collectionRangeEquipment / 100));
        }
        if (collectionRangePowerup != 0f)
        {
            currentCollectionRange *= (1 + (collectionRangePowerup / 100));
        }

        currentLuck = 0f;
        if (baseLuck != 0f)
        {
            currentLuck += baseLuck;
        }
        if (luckEquipment != 0f)
        {
            currentLuck += luckEquipment;
        }
        if (luckPowerup != 0f)
        {
            currentLuck += luckPowerup;
        }

        currentProfitBoost = 0f;
        if (baseProfitBoost != 0f)
        {
            currentProfitBoost += baseProfitBoost;
        }
        if (profitBoostEquipment != 0f)
        {
            currentProfitBoost += profitBoostEquipment;
        }
    }
    #endregion

}
