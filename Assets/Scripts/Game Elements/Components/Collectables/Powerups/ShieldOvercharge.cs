using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOvercharge : Powerup, IPowerup
{
    [Header("Unique Fields")]
    public float shieldRegenPercentage;
    public float shieldCooldownPercentage;
    public float shieldCapacityPercentage;

    private float[] baseStats = new float[3];
    private float[] newStats = new float[3];
    private float[] difference = new float[3];

    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("ShieldOvercharge");
        base.ExecutePowerup(player);
        Debug.Log("Player collected a shield powerup.");

        baseStats[0] = player.stats.baseShieldRegen;
        baseStats[1] = player.stats.baseShieldCooldownTime;
        baseStats[2] = player.stats.baseMaxShields;

        newStats[0] = baseStats[0] * (shieldRegenPercentage / 100);
        newStats[1] = baseStats[1] * (shieldCooldownPercentage / 100);
        newStats[2] = baseStats[2] * (shieldCapacityPercentage / 100);

        difference[0] = newStats[0] - baseStats[0];
        difference[1] = newStats[1] - baseStats[1];
        difference[2] = newStats[2] - baseStats[2];

        player.stats.currentShieldRegen += difference[0];
        player.stats.currentShieldCooldownTime += difference[1];
        player.stats.currentMaxShields += difference[2];
    }

    public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
        player.stats.currentShieldRegen -= difference[0];
        player.stats.currentShieldCooldownTime -= difference[1];
        player.stats.currentMaxShields -= difference[2];
    }
}
