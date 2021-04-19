using UnityEngine;

[CreateAssetMenu(menuName = "Powerup Type/SpeedMitigation")]
public class SpeedMitigation : Powerup, IPowerup
{
	// Define custom fields here
	[SerializeField]
    private float thrustReductionPercentage;

    private float differenceThrust;

	// Implement 'on execute' functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("SpeedMitigation");
        float baseThrust = player.stats.baseForwardThrust;
        float baseVelocity = player.stats.baseMaximumVelocity;

        float thrustReduction = baseThrust * (thrustReductionPercentage / 100); // Returns a positive value
        differenceThrust = baseThrust - thrustReduction; 

        // Reduce forward thrust by the reduction percentage (temporary effect)
        player.stats.currentForwardThrust -= differenceThrust;
        // Reset maximum velocity back to base value (permanent effect)
        player.stats.currentMaximumVelocity = player.stats.baseMaximumVelocity;
    }
	
	// Set stats back to normal values
	public override void EndPowerup(Player player)
    {
        player.stats.currentForwardThrust += differenceThrust;
    }
}