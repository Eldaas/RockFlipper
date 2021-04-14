using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Data Containers")]
    [SerializeField]
    private PlayerStats stats;
    [SerializeField]
    private PlayerEquipment equipment;

    [Header("ReadOnly Properties")]
    [SerializeField, ReadOnly]
    private float currentShields;
    [SerializeField, ReadOnly]
    private float currentArmour;
    [SerializeField, ReadOnly]
    private float currentHull;
    [SerializeField, ReadOnly]
    private float shieldsRechargeRate;

    [Header("Components/Objects")]
    [SerializeField]
    private GameObject shieldObject;

    [Header("Misc Data")]
    private float shieldDestroyedAt;

    #region Properties
    public float Shields { get => stats.currentShields; set => stats.currentShields = value; }
    public float MaxShields { get => stats.currentMaxShields; set => stats.currentMaxShields = value; }
    public float ShieldsRechargeRate { get => stats.currentShieldRegen; set => stats.currentShieldRegen = value; }
    public float Armour { get => stats.currentArmour; set => stats.currentArmour = value; }
    public float MaxArmour { get => stats.currentMaxArmour; set => stats.currentMaxArmour = value; }
    public float Hull { get => stats.currentHull; set => stats.currentHull = value; }
    public float MaxHull { get => stats.currentMaxHull; set => stats.currentMaxHull = value; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        CheckForMissingDataContainers();
    }
    
    private void Start()
    {
        // TO DO: Add max stat modifiers from level modifiers and equipment modifiers

        // Initialise stat values (currently from base levels until modifiers fully implemented)
        stats.currentMaxHull = stats.baseMaxHull;
        stats.currentHull = stats.currentMaxHull;

        stats.currentMaxArmour = stats.baseMaxArmour;
        stats.currentArmour = stats.currentMaxArmour;

        stats.currentMaxShields = stats.baseMaxShields;
        stats.currentShields = stats.currentMaxShields;

        stats.currentShieldRegen = stats.baseShieldRegen;
        stats.currentShieldCooldownTime = stats.baseShieldCooldownTime;

        stats.currentSpeedMitigation = stats.baseSpeedMitigation;
        stats.currentManeuveringSpeed = stats.baseManeuveringSpeed;

        stats.currentHeatSinkCapacity = stats.baseHeatSinkCapacity;
        stats.currentHeatSinkLevel = 0f;

        InvokeRepeating("RegenShield", 1f, 1f);
    }

    private void Update()
    {
        // Sets the ReadOnly values in the inspector for visual feedback
        currentShields = Shields;
        currentArmour = Armour;
        currentHull = Hull;
        shieldsRechargeRate = ShieldsRechargeRate;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponentInParent<Rigidbody>();
        if (rb.CompareTag("Asteroid"))
        {
            EventManager.TriggerEvent("asteroidCollision");
            
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
        if (other.CompareTag("Powerup"))
        {
            EventManager.TriggerEvent("powerupCollected");
            PowerupMono mono = other.GetComponent<PowerupMono>();
            mono.powerup.ExecutePowerup();
            other.gameObject.SetActive(false);
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
        EventManager.TriggerEvent("takeHit");
        float damage = value;

        if(Shields > 0f)
        {
            Shields -= damage;
            if (Shields < 0f)
            {
                damage = Mathf.Abs(Shields);
                Shields = 0f;
                shieldDestroyedAt = Time.time;
                shieldObject.SetActive(false);
                EventManager.TriggerEvent("shieldsDestroyed");
            }
            else
            {
                EventManager.TriggerEvent("shieldsHit");
                damage = 0f;
                return false;
            }
        }
        
        if(Armour > 0f)
        {
            Armour -= damage;
            if (Armour < 0f)
            {
                damage = Mathf.Abs(Armour);
                Armour = 0f;
                EventManager.TriggerEvent("armourDestroyed");
            }
            else
            {
                damage = 0f;
                EventManager.TriggerEvent("armourHit");
                return false;
            }
        }
        
        if(Hull > 0f)
        {
            Hull -= damage;
            if (Hull < 0f)
            {
                Hull = 0f;
                EventManager.TriggerEvent("playerDeath");
                return true;
            }
            else
            {
                EventManager.TriggerEvent("hullHit");
            }
            
            if (Hull / MaxHull <= 0.5f)
            {
                EventManager.TriggerEvent("lowHealth");
            }
        }

        return false;
    }

    /// <summary>
    /// Increase the maximum shield by X amount.
    /// </summary>
    /// <param name="value">The amount to add to the shield, NOT the new shield value.</param>
    public void IncreaseShieldCapacityByValue(float value)
    {
        MaxShields += value;
    }

    /// <summary>
    /// Decrease the maximum shield amount by X amount.
    /// </summary>
    /// <param name="value">The amount to remove from the shield, NOT the new shield value.</param>
    public void DecreaseShieldCapacityByValue(float value)
    {
        MaxShields -= value;
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
                EventManager.TriggerEvent("shieldsOnline");
            }
            
            // Check if shield requires recharging
            if (Shields < MaxShields)
            {
                Shields += ShieldsRechargeRate;
            }

            // Check if shield has been recharged beyond its capacity
            if (Shields > MaxShields)
            {
                EventManager.TriggerEvent("shieldsRecharged");
                Shields = MaxShields;
            }
        }
    }
    #endregion

    
}
