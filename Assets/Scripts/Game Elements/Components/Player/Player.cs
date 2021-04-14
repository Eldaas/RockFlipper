using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth health;
    [SerializeField]
    private PlayerStats stats;
    [SerializeField]
    private PlayerEquipment equipment;

    [Header("Exposed Properties")]
    [SerializeField]
    private float currentShields;
    [SerializeField]
    private float currentArmour;
    [SerializeField]
    private float currentHull;
    [SerializeField]
    private float shieldsRechargeRate;

    #region Properties
    public float Shields { get => currentShields; set => currentShields = value; }
    public float MaxShields { get => health.maxShields; }
    public float ShieldsRechargeRate { get => shieldsRechargeRate; set => shieldsRechargeRate = value; }
    public float Armour { get => currentArmour; set => currentArmour = value; }
    public float MaxArmour { get => health.maxArmour; }
    public float Hull { get => currentHull; set => currentHull = value; }
    public float MaxHull { get => health.maxHull; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        CheckForMissingDataContainers();
    }
    
    private void Start()
    {
        currentShields = health.maxShields;
        currentArmour = health.maxArmour;
        currentHull = health.maxHull;
        shieldsRechargeRate = health.baseShieldRegen;
        InvokeRepeating("RegenShield", 1f, 1f);
        Debug.Log(Shields);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.name);
        Rigidbody rb = collision.collider.GetComponentInParent<Rigidbody>();
        if (rb && rb.CompareTag("Asteroid"))
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
    #endregion

    #region Public Methods
    /// <summary>
    /// Causes the player to take damage.
    /// </summary>
    /// <returns>True if the damage destroys the player, false if the damage does not.</returns>
    public bool TakeDamage(float value)
    {
        float damage = value;

        Shields -= damage;
        if (Shields < 0f)
        {
            damage = Mathf.Abs(Shields);
            Shields = 0f;
        }
        else
        {
            damage = 0f;
        }

        Armour -= damage;
        if (Armour < 0f)
        {
            damage = Mathf.Abs(Armour);
            Armour = 0f;
        }
        else
        {
            damage = 0f;
        }

        Hull -= damage;
        if (Hull < 0f)
        {
            Hull = 0f;
            return true;
        }

        return false;
    }
    #endregion

    #region Private Methods
    private void CheckForMissingDataContainers()
    {
        if (!health || !stats || !equipment)
        {
            Debug.LogError("Missing a critical component on the Player object!");
        }
    }

    private void RegenShield()
    {
        if (Shields < MaxShields)
        {
            Shields += ShieldsRechargeRate * SceneController.instance.levelMods.shieldsRegenMultiplier;
        }

        if(Shields > MaxShields)
        {
            Shields = MaxShields;
        }
    }
    #endregion

    
}
