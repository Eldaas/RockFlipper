using UnityEngine;

public class SpeedMitigation : Powerup, IPowerup
{
    // ThrustReductionPercentage is out of 100. If you want to decrease by 10%, input 10 in the inspector field.
	[SerializeField] private float thrustReductionPercentage;
    [SerializeField] private float maxVelocityReductionPercentage;

	// Implement 'on execute' functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("SpeedMitigation");
        base.ExecutePowerup(player);
        Debug.Log("Player collected a speed reduction powerup.");

        player.stats.forwardThrustPowerup -= Mathf.Abs(thrustReductionPercentage);
        player.stats.maximumVelocityPowerup -= Mathf.Abs(maxVelocityReductionPercentage);

        // Reset velocity incrementor back to base value, so it starts counting from 0 again (permanent effect)
        player.stats.maximumVelocityIncrementor = 0f;

        player.activeEnginesFx.SetActive(false);
        player.inactiveEnginesFx.SetActive(true);
    }

    // Set stats back to normal values
    public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
        player.stats.forwardThrustPowerup += Mathf.Abs(thrustReductionPercentage);
        player.stats.maximumVelocityPowerup += Mathf.Abs(maxVelocityReductionPercentage);

        player.activeEnginesFx.SetActive(true);
        player.inactiveEnginesFx.SetActive(false);
    }
}