using UnityEngine;

[CreateAssetMenu(menuName = "Powerup Type/HullRepair")]
public class HullRepair : Powerup, IPowerup
{
	// Define custom fields here
	[SerializeField]
    private float value;

	// Implement 'on execute' functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("HullRepair");
        player.stats.currentHull += value;

        if(player.stats.currentHull > player.stats.currentMaxHull)
        {
            player.stats.currentHull = player.stats.currentMaxHull;
        }
    }
	
	// Set stats back to normal values
	public override void EndPowerup(Player player)
    {
        
    }
}