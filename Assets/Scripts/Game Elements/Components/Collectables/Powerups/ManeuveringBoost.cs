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
        player.stats.maneuveringSpeedPowerup += Mathf.Abs(percentage);
    }

    public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
        player.stats.maneuveringSpeedPowerup -= Mathf.Abs(percentage);
        EventManager.TriggerEvent("ManeuveringBoostExpired");
    }
}