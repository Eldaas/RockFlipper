using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Powerup : ScriptableObject
{
    public GameObject prefab;
    public float baseDuration;
    
    #region Properties
    public float EffectDuration { get => baseDuration; set => baseDuration = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    #endregion

    public virtual void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("PowerupCollected");

    }

    public virtual void EndPowerup(Player player)
    {

    }
}
