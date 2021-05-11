using UnityEngine;

public class ManeuveringBoost : Powerup, IPowerup
{
	[Header("Unique Fields")]
    public float percentage;
    private float difference;

	// Implement all functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("ManeuveringBoost");
        base.ExecutePowerup(player);
        Debug.Log("Player collected a maneuvering boost powerup.");

        float baseSpeed = player.stats.baseManeuveringSpeed;
        float newSpeed = baseSpeed * (percentage / 100);
        difference = newSpeed - baseSpeed;

        player.stats.currentManeuveringSpeed += difference;
    }

    public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
        player.stats.currentManeuveringSpeed -= difference;
    }
}