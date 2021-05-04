using UnityEngine;

public class ArmourRepair : Powerup, IPowerup
{
	[Header("Unique Fields")]
	[SerializeField]
    private float value;
    
	// Implement 'on execute' functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("ArmourRepair");
        base.ExecutePowerup(player);
        Debug.Log("Player collected an armour powerup.");

        player.stats.currentArmour += value;

        if (player.stats.currentArmour > player.stats.currentMaxArmour)
        {
            player.stats.currentArmour = player.stats.currentMaxArmour;
        }

        
    }
	
	// Set stats back to normal values
	public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
    }
}