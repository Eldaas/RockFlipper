using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public float Shields { get => stats.currentShields; set => stats.currentShields = value; }
    public float MaxShields { get => stats.CurrentMaxShields; }
    public float ShieldsRechargeRate { get => stats.CurrentShieldRegen; }
    public float Armour { get => stats.currentArmour; set => stats.currentArmour = value; }
    public float MaxArmour { get => stats.CurrentMaxArmour; }
    public float Hull { get => stats.currentHull; set => stats.currentHull = value; }
    public float MaxHull { get => stats.CurrentMaxHull; }
    public float Velocity { get => rb.velocity.magnitude; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        CheckForMissingDataContainers();
        stats.ResetPowerupStats();
    }
    
    private void Start()
    {
        InvokeRepeating("RegenShield", 1f, 1f);
    }

    private void Update()
    {
        
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

        if(Shields > 0f)
        {
            Shields -= damage;
            if (Shields < 0f)
            {
                damage = Mathf.Abs(Shields);
                Shields = 0f;
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
        
        if(Armour > 0f)
        {
            Armour -= damage;
            if (Armour < 0f)
            {
                damage = Mathf.Abs(Armour);
                Armour = 0f;
                EventManager.TriggerEvent("ArmourDestroyed");
            }
            else
            {
                damage = 0f;
                EventManager.TriggerEvent("ArmourHit");
                return false;
            }
        }
        
        if(Hull > 0f)
        {
            Hull -= damage;
            if (Hull < 0f)
            {
                Hull = 0f;
                EventManager.TriggerEvent("PlayerDeath");
                return true;
            }
            else
            {
                EventManager.TriggerEvent("HullHit");
            }
            
            if (Hull / MaxHull <= 0.5f)
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
        if(Time.time > shieldDestroyedAt + stats.CurrentShieldCooldownTime)
        {
            // Reactivate shield if it's supposed to be active
            if(!shieldObject.activeInHierarchy)
            {
                shieldObject.SetActive(true);
                EventManager.TriggerEvent("ShieldsOnline");
            }
            
            // Check if shield requires recharging
            if (Shields < MaxShields)
            {
                Shields += ShieldsRechargeRate;
            }

            // Check if shield has been recharged beyond its capacity
            if (Shields > MaxShields)
            {
                EventManager.TriggerEvent("ShieldsRecharged");
                Shields = MaxShields;
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
    #endregion

    
}
