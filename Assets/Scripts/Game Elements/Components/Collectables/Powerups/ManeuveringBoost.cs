using UnityEngine;

[CreateAssetMenu(menuName = "Powerup Type/ManeuveringBoost")]
public class ManeuveringBoost : Powerup, IPowerup
{
	// Define custom fields here
    public float percentage;
    private float difference;

	// Implement all functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("ManeuveringBoost");
        float baseSpeed = player.stats.baseManeuveringSpeed;
        float newSpeed = baseSpeed * (percentage / 100);
        difference = newSpeed - baseSpeed;

        player.stats.currentManeuveringSpeed += difference;
    }

    public override void EndPowerup(Player player)
    {
        player.stats.currentManeuveringSpeed -= difference;
    }
}