using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOvercharge : Powerup, IPowerup
{
    [Header("Unique Fields")]
    public float shieldRegenPercentage;
    public float shieldCooldownPercentage;
    public float shieldCapacityPercentage;

    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("ShieldOvercharge");
        base.ExecutePowerup(player);
        Debug.Log("Player collected a shield powerup.");

        player.stats.shieldRegenPowerup += shieldRegenPercentage;
        player.stats.shieldCooldownTimePowerup += shieldCooldownPercentage;
        player.stats.maxShieldsPowerup += shieldCapacityPercentage;
    }

    public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);

        player.stats.shieldRegenPowerup -= shieldRegenPercentage;
        player.stats.shieldCooldownTimePowerup -= shieldCooldownPercentage;
        player.stats.maxShieldsPowerup -= shieldCapacityPercentage;
    }
}
