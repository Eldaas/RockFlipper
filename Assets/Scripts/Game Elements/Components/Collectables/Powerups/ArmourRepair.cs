using UnityEngine;

[CreateAssetMenu(menuName = "Powerup Type/ArmourRepair")]
public class ArmourRepair : Powerup, IPowerup
{
	// Define custom fields here
	[SerializeField]
    private float value;

	// Implement 'on execute' functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("ArmourRepair");
        player.stats.currentArmour += value;

        if (player.stats.currentArmour > player.stats.currentMaxArmour)
        {
            player.stats.currentArmour = player.stats.currentMaxArmour;
        }
    }
	
	// Set stats back to normal values
	public override void EndPowerup(Player player)
    {
        
    }
}