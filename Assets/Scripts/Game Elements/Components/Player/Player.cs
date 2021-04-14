using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region Properties
    public float Shields { get => stats.currentShields; set => stats.currentShields = value; }
    public float MaxShields { get => stats.maxShields; set => stats.maxShields = value; }
    public float ShieldsRechargeRate { get => stats.currentShieldRegen; set => stats.currentShieldRegen = value; }
    public float Armour { get => stats.currentArmour; set => stats.currentArmour = value; }
    public float MaxArmour { get => stats.maxArmour; set => stats.maxArmour = value; }
    public float Hull { get => stats.currentHull; set => stats.currentHull = value; }
    public float MaxHull { get => stats.maxHull; set => stats.maxHull = value; }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        CheckForMissingDataContainers();
    }
    
    private void Start()
    {
        currentShields = stats.maxShields;
        currentArmour = stats.maxArmour;
        currentHull = stats.maxHull;
        shieldsRechargeRate = stats.baseShieldRegen;
        InvokeRepeating("RegenShield", 1f, 1f);
        Debug.Log(Shields);
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
    private void CheckForMissingDataContainers()
    {
        if (!stats || !equipment)
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
