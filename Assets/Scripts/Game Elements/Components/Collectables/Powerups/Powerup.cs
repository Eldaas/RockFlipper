using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Powerup : MonoBehaviour
{
    [Header("Base Data")]
    public float baseDuration;
    public GameObject objectParticle;
    public GameObject playerParticleFxPrefab;
    public Vector3 playerParticleFxOffset;
    protected GameObject playerParticleFx;
    public GameObject uiIconPrefab;
    public bool isActive;
    
    // Value out of 100
    public float chanceToSpawn;

    #region Properties
    public float EffectDuration { get => baseDuration; set => baseDuration = value; }
    public float ChanceToSpawn { get => chanceToSpawn; }
    public GameObject UiIconPrefab { get => uiIconPrefab; }
    public bool IsActive { get => isActive; }
    #endregion

    public virtual void ExecutePowerup(Player player)
    {
        Debug.Log("ExecutePowerup triggered.");
        EventManager.TriggerEvent("PowerupCollected");
        playerParticleFx = Instantiate(playerParticleFxPrefab, player.vfxParent.transform);
        playerParticleFx.transform.localPosition = playerParticleFxOffset;
        Debug.Log("Instantiated player particles: " + playerParticleFx.name);
        playerParticleFx.SetActive(true);
        objectParticle.SetActive(false);
        isActive = true;

    }

    public virtual void EndPowerup(Player player)
    {
        Debug.Log("Ending the powerup.");
        Debug.Log("Setting gameobject to inactive: " + playerParticleFx.name);
        playerParticleFx.SetActive(false);
        objectParticle.SetActive(true);
        isActive = false;
    }
}
