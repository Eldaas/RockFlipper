using UnityEngine;

public class SpeedMitigation : Powerup, IPowerup
{
    // ThrustReductionPercentage is out of 100. If you want to decrease by 10%, input 10 in the inspector field.
	[SerializeField] private float thrustReductionPercentage;
    private float differenceThrust;
    private float differenceVelocity;

	// Implement 'on execute' functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("SpeedMitigation");
        base.ExecutePowerup(player);
        Debug.Log("Player collected a speed reduction powerup.");

        float thrust = player.stats.currentForwardThrust;
        float maxVelocity = player.stats.currentMaximumVelocity;

        float thrustReduction = thrust * (thrustReductionPercentage / 100); // Returns a positive value
        differenceThrust = thrust - thrustReduction;

        float velocityReduction = maxVelocity * (thrustReductionPercentage / 100);
        differenceVelocity = maxVelocity - velocityReduction;

        // Reduce forward thrust by the reduction percentage (temporary effect)
        player.stats.forwardThrustPowerup -= differenceThrust;

        // Reduce maximum velocity by reduction percentage (temporary effect)
        player.stats.maximumVelocityPowerup -= differenceVelocity;

        // Reset velocity incrementor back to base value, so it starts counting from 0 again (permanent effect)
        player.stats.maximumVelocityIncrementor = 0f;

        player.activeEnginesFx.SetActive(false);
        player.inactiveEnginesFx.SetActive(true);
    }

    // Set stats back to normal values
    public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
        player.stats.forwardThrustPowerup += differenceThrust;
        player.stats.maximumVelocityPowerup += differenceVelocity;
        player.activeEnginesFx.SetActive(true);
        player.inactiveEnginesFx.SetActive(false);
    }
}