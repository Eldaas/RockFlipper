using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object is designed to be used with our object pooling system. Inside the pooling system simply access this component and call the relevant methods.
/// </summary>
public class Asteroid : Hazard
{
    [Header("Inspector References")]
    public AsteroidType asteroidType;
    public AsteroidData data;
    public float healthMultiplier;
    public HealthBar healthBar;
    [SerializeField]
    private GameObject explosionParticles;
    public float currentHealth;
    public float maxHealth;
    [SerializeField]
    private int dropYieldMin;
    [SerializeField]
    private int dropYieldMax;
    
    delegate GameObject GetPooledObject();
    GetPooledObject getPooledObject;
    
    #region Public Methods
    /// <summary>
    /// Disables the asteroid visual game object and sets active explosion particle effect.
    /// </summary>
    /// <param name="force">The kinetic force to apply to the childrenObjects.</param>
    /// <param name="explosionRadius">The radius of the explosion.</param>
    public void ExplodeAsteroid(float force, float explosionRadius)
    {
        mainObject.SetActive(false);
        explosionParticles.SetActive(true);
        healthBar.Deactivate();
        // TO DO: differentiate between large and medium explosions, play different sound for each
        EventManager.TriggerEvent("LargeAsteroidExplosion");

        if (asteroidType != AsteroidType.Barren)
        {
            SpawnCollectables(force, explosionRadius);
        }
    }

    /// <summary>
    /// This method can be called to handle behaviour when an object (such as the player) collides with the object.
    /// </summary>
    public void CollideWithAsteroid()
    {
        // Functionality temporarily removed - to implement in a later polish pass
    }

    public void SetAsteroidHealth()
    {
        if(healthMultiplier != 0f)
        {
            maxHealth = data.baseHealth * healthMultiplier;
            
        }
        else
        {
            maxHealth = data.baseHealth;
        }

        currentHealth = maxHealth;
    }

    public void MultiplyAsteroidMass()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass *= 40;
    }

    public void AssignCollectableType()
    {
        if (asteroidType == AsteroidType.Iron)
        {
            getPooledObject = ObjectPooler.instance.GetPooledIron;
        }
        else if (asteroidType == AsteroidType.Silver)
        {
            getPooledObject = ObjectPooler.instance.GetPooledSilver;
        }
        else if (asteroidType == AsteroidType.Gold)
        {
            getPooledObject = ObjectPooler.instance.GetPooledGold;
        }
    }

    /// <summary>
    /// Resets the positions of all asteroid childrenObjects, deactivates them and reactivates the main asteroid.
    /// </summary>
    public override void ResetHazard()
    {
        base.ResetHazard();
        mainObject.SetActive(true);
        explosionParticles.SetActive(false);
        SetAsteroidHealth();

    }

    #endregion

    #region Private Methods
    private void SpawnCollectables(float force, float explosionRadius)
    {
        int numToSpawn;

        if(SceneController.instance.player.StruckLucky())
        {
           numToSpawn = dropYieldMax * 2;
           EventManager.TriggerEvent("StruckLucky");
        }
        else
        {
            numToSpawn = Utility.GenerateRandomInt(dropYieldMin, dropYieldMax);
            EventManager.TriggerEvent("ResourcesDropped");
        }
        
        for (int i = 0; i < numToSpawn; i++)
        {
            GameObject go = getPooledObject();

            if(go != null)
            {
                go.transform.position = transform.position + Utility.GenerateRandomOffset(new Vector3(-0.5f, 0f, -0.5f), new Vector3(0.5f, 0f, 0.5f));
                go.SetActive(true);

                Rigidbody rb = go.GetComponent<Rigidbody>();
                rb.AddExplosionForce(force * transform.localScale.magnitude, transform.position, explosionRadius);
            }
        }
    }

    #endregion

    #region Unity Methods

    protected override void OnCollisionEnter(Collision collision)
    {
        
    }

    protected override void OnParticleCollision(GameObject collider)
    {
        base.OnParticleCollision(collider);

        // Check if the collision is actually a defined projectile
        if (collider.CompareTag("Projectile"))
        {
            EventManager.TriggerEvent("ProjectileHit");
            // Get the particle events so we can get the intersect location
            collider.gameObject.SetActive(false);
            List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
            ParticleSystem projectileParticles = collider.GetComponent<ParticleSystem>();
            projectileParticles.GetCollisionEvents(gameObject, events);

            // Get a pooled hit FX game object and set its location to the intersection point, set active and start reset coroutine
            GameObject hitFx = ObjectPooler.instance.GetPooledHitFx();
            if (hitFx)
            {
                hitFx.transform.position = events[0].intersection;
                hitFx.gameObject.SetActive(true);
                ParticleSystem hitFxParticles = hitFx.GetComponent<ParticleSystem>();
                IEnumerator coroutine = ObjectPooler.instance.ReturnParticleToPool(hitFx, hitFxParticles.main.startLifetime.constant);
                ObjectPooler.instance.StartCoroutine(coroutine);
            }

            // Deal damage to the asteroid health
            currentHealth -= SceneController.instance.player.stats.currentProjectileDamage;
            healthBar.Activate();

            // If health is lower than zero, trigger the asteroid explosion chain
            if(currentHealth <= 0f)
            {
                ExplodeAsteroid(3000f * transform.localScale.magnitude, 100f);
            }

        }
    }
    #endregion
}

public enum AsteroidType { Barren, Iron, Silver, Gold }