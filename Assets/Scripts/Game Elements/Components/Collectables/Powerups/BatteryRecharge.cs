using UnityEngine;

public class BatteryRecharge : Powerup, IPowerup
{
	// Define custom fields here
	[SerializeField]
    private float immediateRecoveryAmount;
    [SerializeField]
    private float rechargePerSecondPercentage;

	// Implement 'on execute' functionality within this function
    public override void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("BatteryRecharge");
        base.ExecutePowerup(player);
        Debug.Log("Player collected a battery powerup.");
        player.stats.currentBatteryLevel += immediateRecoveryAmount;
        
        if(player.stats.currentBatteryLevel > player.stats.currentBatteryCapacity)
        {
            player.stats.currentBatteryLevel = player.stats.currentBatteryCapacity;
        }

        player.stats.batteryRechargePowerup += rechargePerSecondPercentage;

    }
	
	// Set stats back to normal values
	public override void EndPowerup(Player player)
    {
        base.EndPowerup(player);
        player.stats.batteryRechargePowerup -= rechargePerSecondPercentage;
        EventManager.TriggerEvent("BatteryRechargeExpired");
    }
}