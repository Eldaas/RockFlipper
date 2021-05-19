﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOvercharge : Powerup, IPowerup
{
    [Header("Unique Fields")]
    public float shieldRegenPercentage;
    public float shieldCooldownPercentage;
    public float shieldCapacityPercentage;

<<<<<<< HEAD
    private float[] stats = new float[3];
    private float[] newStats = new float[3];
    private float[] difference = new float[3];

    private float currentRegen;
    private float currentCooldown;
    private float currentMax;

    private float newRegen;
    private float newCooldown;
    private float newMax;

    private float diffRegen;
    private float diffCooldown;
    private float diffMax;

=======
>>>>>>> implement-inventory-equipment
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("ShieldOvercharge");
        base.ExecutePowerup(player);
        Debug.Log("Player collected a shield powerup.");

<<<<<<< HEAD
        currentRegen = player.stats.currentShieldRegen;
        currentCooldown = player.stats.currentShieldCooldownTime;
        currentMax = player.stats.currentMaxShields;

        newRegen = currentRegen * (1 + (shieldRegenPercentage / 100));
        newCooldown = currentCooldown * (1 - (shieldCooldownPercentage / 100));
        newMax = currentMax * (shieldCapacityPercentage / 100);

        diffRegen = newRegen - currentRegen;
        diffCooldown = newCooldown - currentCooldown;
        diffMax = newMax - currentMax;

        player.stats.shieldRegenPowerup += diffRegen;
        player.stats.shieldCooldownTimePowerup += diffCooldown;
        player.stats.maxShieldsPowerup += diffMax;
=======
        player.stats.shieldRegenPowerup += shieldRegenPercentage;
        player.stats.shieldCooldownTimePowerup += shieldCooldownPercentage;
        player.stats.maxShieldsPowerup += shieldCapacityPercentage;
>>>>>>> implement-inventory-equipment
    }

    public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
<<<<<<< HEAD
        player.stats.shieldRegenPowerup -= diffRegen;
        player.stats.shieldCooldownTimePowerup -= diffCooldown;
        player.stats.maxShieldsPowerup -= diffMax;
=======
        player.stats.shieldRegenPowerup -= shieldRegenPercentage;
        player.stats.shieldCooldownTimePowerup -= shieldCooldownPercentage;
        player.stats.maxShieldsPowerup -= shieldCapacityPercentage;
>>>>>>> implement-inventory-equipment
    }
}
