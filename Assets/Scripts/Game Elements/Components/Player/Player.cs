﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class Player : MonoBehaviour
{
    [Header("Data Containers")]
    public PlayerStats stats;
    [SerializeField]
    private PlayerEquipment equipment;

    [Header("Components/Objects")]
    private Rigidbody rb;
    [SerializeField]
    private GameObject shieldObject;

    [Header("VFX")]
    public GameObject vfxParent;
    public GameObject activeEnginesFx;
    public GameObject inactiveEnginesFx;

    [Header("Misc Data")]
    private float shieldDestroyedAt;

    #region Properties
    public float Velocity { get => rb.velocity.magnitude; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CheckForMissingDataContainers();
    }
    
    private void Start()
    {
        InvokeRepeating("RegenShield", 1f, 1f);
        InvokeRepeating("UpdateStats", 0.5f, 0.1f);
    }

    private void Update()
    {
        /*stats.UpdateStats();*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Rigidbody rb = collision.collider.GetComponentInParent<Rigidbody>();
            EventManager.TriggerEvent("AsteroidCollision");
            
            if (rb)
            {
                Asteroid asteroid = rb.GetComponent<Asteroid>();
                Rigidbody prb = GetComponent<Rigidbody>();

                float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
                if (collisionForce > 20000f)
                {
                    asteroid.CollideWithAsteroid();
                }

                Player player = GetComponent<Player>();
                if (player != null)
                {
                    float normalisedDamage = Mathf.Clamp(collisionForce / asteroid.data.collisionMaxForce, 0, 1);
                    player.TakeDamage(normalisedDamage * 100 * SceneController.instance.levelMods.takenDamageMultiplier);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            IPowerup thisPowerup = other.GetComponent<IPowerup>();
            IEnumerator coroutine = PowerupCoroutine(thisPowerup);
            StartCoroutine(coroutine);
        }

    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Causes the player to take damage. Automatically calculates which health pool the damage should come from, and if the player reaches zero hull then the player dies.
    /// </summary>
    /// <returns>True if the damage destroys the player, false if the damage does not.</returns>
    public bool TakeDamage(float value)
    {
        EventManager.TriggerEvent("TakeHit");
        float damage = value;

        if(stats.currentShields > 0f)
        {
            stats.currentShields -= damage;
            if (stats.currentShields < 0f)
            {
                damage = Mathf.Abs(stats.currentShields);
                stats.currentShields = 0f;
                shieldDestroyedAt = Time.time;
                shieldObject.SetActive(false);
                EventManager.TriggerEvent("ShieldsDestroyed");
            }
            else
            {
                EventManager.TriggerEvent("ShieldsHit");
                damage = 0f;
                return false;
            }
        }
        
        if(stats.currentArmour > 0f)
        {
            stats.currentArmour -= damage;
            if (stats.currentArmour < 0f)
            {
                damage = Mathf.Abs(stats.currentArmour);
                stats.currentArmour = 0f;
                EventManager.TriggerEvent("ArmourDestroyed");
            }
            else
            {
                damage = 0f;
                EventManager.TriggerEvent("ArmourHit");
                return false;
            }
        }
        
        if(stats.currentHull > 0f)
        {
            stats.currentHull -= damage;
            if (stats.currentHull < 0f)
            {
                stats.currentHull = 0f;
                EventManager.TriggerEvent("PlayerDeath");
                return true;
            }
            else
            {
                EventManager.TriggerEvent("HullHit");
            }
            
            if (stats.currentHull / stats.currentMaxHull <= 0.5f)
            {
                EventManager.TriggerEvent("HealthLow");
            }
        }

        return false;
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Ensures that the player has the required data containers assigned
    /// </summary>
    private void CheckForMissingDataContainers()
    {
        if (!stats || !equipment)
        {
            Debug.LogError("Missing a critical component on the Player object!");
        }
    }

    /// <summary>
    /// Regenerates the player's shield by the amount set in the stats data container
    /// </summary>
    private void RegenShield()
    {
        if(Time.time > shieldDestroyedAt + stats.currentShieldCooldownTime)
        {
            // Reactivate shield if it's supposed to be active
            if(!shieldObject.activeInHierarchy)
            {
                shieldObject.SetActive(true);
                EventManager.TriggerEvent("ShieldsOnline");
            }
            
            // Check if shield requires recharging
            if (stats.currentShields < stats.currentMaxShields)
            {
                stats.currentShields += stats.currentShieldRegen;
            }

            // Check if shield has been recharged beyond its capacity
            if (stats.currentShields > stats.currentMaxShields)
            {
                EventManager.TriggerEvent("ShieldsRecharged");
                stats.currentShields = stats.currentMaxShields;
            }
        }
    }

    private IEnumerator PowerupCoroutine(IPowerup powerup)
    {
        //Debug.Log("Coroutine started.");

        float endTime = Time.time + powerup.EffectDuration;
        //Debug.Log("Effect duration is: " + powerup.EffectDuration);
        powerup.ExecutePowerup(this);

        while(Time.time < endTime)
        {
            yield return null;
        }

        powerup.EndPowerup(this);
    }

    private void UpdateStats()
    {
        stats.UpdateStats();
    }
    #endregion

    
}
