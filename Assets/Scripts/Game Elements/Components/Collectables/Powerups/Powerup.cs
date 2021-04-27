using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Powerup : ScriptableObject, IPowerup
{
    [Header("Base Stats")]
    public GameObject prefab;
    public float baseDuration;
    
    // Value out of 100
    public float chanceToSpawn;

    #region Properties
    public float EffectDuration { get => baseDuration; set => baseDuration = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public float ChanceToSpawn { get => chanceToSpawn; }
    #endregion

    public virtual void ExecutePowerup(Player player)
    {
        EventManager.TriggerEvent("PowerupCollected");

    }

    public virtual void EndPowerup(Player player)
    {

    }
}
