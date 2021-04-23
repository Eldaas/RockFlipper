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
    [SerializeField]
    private GameObject explosionParticles;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private int dropYieldMin;
    [SerializeField]
    private int dropYieldMax;
    

    #region Public Methods
    /// <summary>
    /// Disables the main asteroid, activates all childrenObjects and adds an explosion force.
    /// </summary>
    /// <param name="force">The kinetic force to apply to the childrenObjects.</param>
    /// <param name="explosionRadius">The radius of the explosion.</param>
    public void ExplodeAsteroid(float force, float explosionRadius)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        //PopulateChildrenObjects();
        mainObject.SetActive(false);
        explosionParticles.SetActive(true);

        /*for (int i = 0; i < childrenObjects.Count; i++)
        {
            childrenObjects[i].gameObject.SetActive(true);
            rb = childrenObjects[i].GetComponent<Rigidbody>();
            rb.AddExplosionForce(force, mainObject.transform.position, explosionRadius);

        }*/
        //Debug.Log("Asteroid has exploded");
    }

    /// <summary>
    /// This method can be called to handle behaviour when an object (such as the player) collides with the object.
    /// </summary>
    public void CollideWithAsteroid()
    {
        //Rigidbody rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        
        //mainObject.SetActive(false);
/*        for (int i = 0; i < childrenObjects.Count; i++)
        {
            childrenObjects[i].gameObject.SetActive(true);
        }*/
    }

    public void SetAsteroidHealth()
    {
        if(healthMultiplier != 0f)
        {
            currentHealth = data.baseHealth * healthMultiplier;
        }
        else
        {
            currentHealth = data.baseHealth;
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

    
    #endregion

    #region Unity Methods
    protected override void OnCollisionEnter(Collision collision)
    {
        /*base.OnCollisionEnter(collision);
        Debug.Log(collision.collider.name);*/
    }

    protected override void OnParticleCollision(GameObject collider)
    {
        base.OnParticleCollision(collider);

        // Check if the collision is actually a defined projectile
        if (collider.CompareTag("Projectile"))
        {
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

            // If health is lower than zero, trigger the asteroid explosion chain
            if(currentHealth <= 0f)
            {
                // Get a pooled resource particles prefab
                GameObject asteroidChunks = ObjectPooler.instance.GetPooledAsteroidChunk();
                if (asteroidChunks)
                {
                    // Set the number of particles to spawn (this equals the number of resource units dropped)
                    ParticleSystem asteroidChunksParticles = asteroidChunks.GetComponent<ParticleSystem>();

                    ParticleSystem.Burst burst = asteroidChunksParticles.emission.GetBurst(0);
                    burst.minCount = (short)dropYieldMin;
                    burst.maxCount = (short)dropYieldMax;
                    asteroidChunksParticles.emission.SetBurst(0, burst);

                    //burst.count = new ParticleSystem.MinMaxCurve(dropYieldMin, dropYieldMax);
                    //Debug.Log(burst.count);

                    // Set the material on the particles to match the type of asteroid, set its position, set active and start reset coroutine
                    ParticleSystemRenderer asteroidChunksRenderer = asteroidChunks.GetComponent<ParticleSystemRenderer>();
                    Material[] mats = asteroidChunksRenderer.materials;

                    switch (asteroidType)
                    {
                        case AsteroidType.Iron:
                            mats[0] = data.ironMaterial;
                            break;
                        case AsteroidType.Silver:
                            mats[0] = data.silverMaterial;
                            break;
                        case AsteroidType.Gold:
                            mats[0] = data.goldMaterial;
                            break;
                        default:
                            Debug.Log("Couldn't find the appropriate material for the asteroid.");
                            break;
                    }

                    asteroidChunksRenderer.materials = mats;
                    asteroidChunks.transform.position = transform.position;
                    asteroidChunks.SetActive(true);
                    ParticleSystem asteroidChunkParticles = asteroidChunks.GetComponent<ParticleSystem>();
                    IEnumerator coroutine = ObjectPooler.instance.ReturnParticleToPool(asteroidChunks, asteroidChunkParticles.main.startLifetime.constant);
                    ObjectPooler.instance.StartCoroutine(coroutine);
                }

                ExplodeAsteroid(120000f * transform.localScale.magnitude, 1000f);
            }

        }
    }
    #endregion
}

public enum AsteroidType { Iron, Silver, Gold }