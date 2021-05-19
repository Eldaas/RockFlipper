using UnityEngine;

public class ManeuveringBoost : Powerup, IPowerup
{
	[Header("Unique Fields")]
    public float percentage;

	// Implement all functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("ManeuveringBoost");
        base.ExecutePowerup(player);
        Debug.Log("Player collected a maneuvering boost powerup.");

<<<<<<< HEAD
        float speed = player.stats.currentManeuveringSpeed;
        float newSpeed = speed * (percentage / 100);
        difference = newSpeed - speed;

        player.stats.maneuveringSpeedPowerup += difference;
=======
        player.stats.maneuveringSpeedPowerup += Mathf.Abs(percentage);
>>>>>>> implement-inventory-equipment
    }

    public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
<<<<<<< HEAD
        player.stats.maneuveringSpeedPowerup -= difference;
=======
        player.stats.maneuveringSpeedPowerup -= Mathf.Abs(percentage);
>>>>>>> implement-inventory-equipment
    }
}