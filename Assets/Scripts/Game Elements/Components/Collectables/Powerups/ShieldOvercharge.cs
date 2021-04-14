using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerup Type/Shield Overcharge")]
public class ShieldOvercharge : Powerup, IPowerup
{
    private float percentageIncrease;

    public override void ExecutePowerup()
    {
        EventManager.TriggerEvent("shieldOvercharge");
        
    }
}
