using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Data Containers")]
    public PlayerStats stats;

    [Header("Components/Objects")]
    private Rigidbody rb;
    [SerializeField]
    private GameObject shieldObject;
    [SerializeField]
    private GameObject shipVisual;
    private ResourceCollector resourceCollector;

    [Header("VFX")]
    public GameObject vfxParent;
    public GameObject activeEnginesFx;
    public GameObject inactiveEnginesFx;
    public GameObject explosionFX;

    [Header("Misc Data")]
    private float shieldDestroyedAt;

    #region Properties
    public float VelocityX { get => rb.velocity.x; }
    public float VelocityZ { get => rb.velocity.z; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        resourceCollector = GetComponent<ResourceCollector>();
        CheckForMissingDataContainers();
    }
    
    private void Start()
    {
        EquipmentManager.instance.RecalcEquipmentEffects();
        InvokeRepeating("RegenShield", 1f, 1f);
        InvokeRepeating("RechargeBattery", 1f, 1f);
        InvokeRepeating("UpdateStats", 0.5f, 0.1f);
    }

    private void Update()
    {
        ProfileManager.instance.currentProfile.totalPlayTime += Time.deltaTime;
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

                float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
                Debug.Log($"Collided with force: {collisionForce}");
                if (collisionForce > 10000000f)
                {
                    asteroid.ExplodeAsteroid(1000f * transform.localScale.magnitude, 100f);
                }

                Player player = GetComponent<Player>();
                if (player != null)
                {
                    float normalisedDamage = Mathf.Clamp(collisionForce / asteroid.data.collisionMaxForce, 0, 1);
                    player.TakeDamage(normalisedDamage * 100);
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
        bool eventTriggered = false;

        if(stats.currentShields > 0f)
        {
            EventManager.TriggerEvent("ShieldsHit");
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
                damage = 0f;
                return false;
            }
        }
        
        if(stats.currentArmour > 0f)
        {
            EventManager.TriggerEvent("ArmourHullHit");
            eventTriggered = true;
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
                return false;
            }
        }

        if (stats.currentHull > 0f)
        {
            if(!eventTriggered)
            {
                EventManager.TriggerEvent("ArmourHullHit");
            }

            stats.currentHull -= damage;
            if (stats.currentHull < 0f)
            {
                stats.currentHull = 0f;
                DeathSequence();
                return true;
            }
            
            if (stats.currentHull / stats.currentMaxHull <= 0.5f)
            {
                EventManager.TriggerEvent("HealthLow");
            }
        }

        

        
        return false;
    }

    public bool StruckLucky()
    {
        int randomInt = Utility.GenerateRandomInt(0, 100);

        if (randomInt <= 1 * (stats.currentLuck / 100))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Ensures that the player has the required data containers assigned
    /// </summary>
    private void CheckForMissingDataContainers()
    {
        if (!stats)
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

    private void RechargeBattery()
    {
        if(stats.currentBatteryLevel < stats.currentBatteryCapacity)
        {
            stats.currentBatteryLevel += stats.currentBatteryRecharge;
        }

        if(stats.currentBatteryLevel > stats.currentBatteryCapacity)
        {
            stats.currentBatteryLevel = stats.currentBatteryCapacity;
        }
    }

    private IEnumerator PowerupCoroutine(IPowerup powerup)
    {
        float endTime = Time.time + powerup.EffectDuration;
        powerup.ExecutePowerup(this);
        GameObject icon = null;

        if(powerup.UiIconPrefab != null)
        {
            icon = Instantiate(powerup.UiIconPrefab, SceneController.instance.sceneUi.powerupsParent.transform);
        }

        PowerupUI pui = icon.GetComponent<PowerupUI>();
        float timeRemaining = endTime - Time.time;

        while (timeRemaining > Mathf.Epsilon)
        {
            timeRemaining -= Time.deltaTime;
            pui.text.text = $"{Math.Truncate(timeRemaining)}s";
            pui.bar.SetPercent(timeRemaining / powerup.EffectDuration);
            yield return new WaitForEndOfFrame();
        }

        if(icon != null)
        {
            Destroy(icon);
        }
        
        powerup.EndPowerup(this);
    }

    private void UpdateStats()
    {
        stats.UpdateStats();
    }

    private void DeathSequence()
    {
        Debug.Log("Player has died!");
        EventManager.TriggerEvent("PlayerDeath");
        ProfileManager.instance.currentProfile.isDead = true;
        ProfileManager.instance.currentProfile.numOfDeaths++;
        ProfileManager.instance.SaveProfile();
        rb.isKinematic = true;
        resourceCollector.enabled = false;
        explosionFX.SetActive(true);
        activeEnginesFx.SetActive(false);
        inactiveEnginesFx.SetActive(false);
        vfxParent.SetActive(false);
        shipVisual.SetActive(false);
        GameManager.instance.SetState(GameStates.DeathState);
        StopAllCoroutines();
    }
    #endregion

    
}
