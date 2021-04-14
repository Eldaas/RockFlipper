using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Powerup : ScriptableObject
{
    public GameObject prefab;
    public float baseDuration;
    public float spawnChance;
    
    #region Properties
    public float EffectDuration { get => baseDuration; set => baseDuration = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    #endregion

    public virtual void ExecutePowerup()
    {
        EventManager.TriggerEvent("powerupCollected");
    }
}
