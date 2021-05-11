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

        float speed = player.stats.baseManeuveringSpeed + player.stats.maneuveringSpeedEquipment;
        float newSpeed = speed * (percentage / 100);
        difference = newSpeed - speed;

        player.stats.maneuveringSpeedPowerup += difference;
    }

    public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
        player.stats.maneuveringSpeedPowerup -= difference;
    }
}