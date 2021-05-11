using UnityEngine;

public class HullRepair : Powerup, IPowerup
{
	[Header("Unique Fields")]
	[SerializeField]
    private float value;

	// Implement 'on execute' functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("HullRepair");
        base.ExecutePowerup(player);
        Debug.Log("Player collected a hull powerup.");

        player.stats.currentHull += value;

        if(player.stats.currentHull > player.stats.CurrentMaxHull)
        {
            player.stats.currentHull = player.stats.CurrentMaxHull;
        }

    }

    // Set stats back to normal values
    public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
    }
}