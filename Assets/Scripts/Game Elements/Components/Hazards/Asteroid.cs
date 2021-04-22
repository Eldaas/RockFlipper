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
    [SerializeField]
    private GameObject explosionParticles;
    

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
        PopulateChildrenObjects();
        mainObject.SetActive(false);

        explosionParticles.SetActive(true);

        for (int i = 0; i < childrenObjects.Count; i++)
        {
            childrenObjects[i].gameObject.SetActive(true);
            rb = childrenObjects[i].GetComponent<Rigidbody>();
            rb.AddExplosionForce(force, mainObject.transform.position, explosionRadius);

        }
        //Debug.Log("Asteroid has exploded");
    }

    /// <summary>
    /// This method can be called to handle behaviour when an object (such as the player) collides with the object.
    /// </summary>
    public void CollideWithAsteroid()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        
        mainObject.SetActive(false);
        for (int i = 0; i < childrenObjects.Count; i++)
        {
            childrenObjects[i].gameObject.SetActive(true);
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

        if (collider.CompareTag("Projectile"))
        {
            collider.SetActive(false);
            List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
            ParticleSystem thisSystem = collider.GetComponent<ParticleSystem>();
            thisSystem.GetCollisionEvents(gameObject, events);

            GameObject hitFx = ObjectPooler.instance.GetPooledHitFx();

            if (hitFx)
            {
                hitFx.transform.position = events[0].intersection;
                hitFx.gameObject.SetActive(true);
            }

            ExplodeAsteroid(120000f * transform.localScale.magnitude, 1000f);
        }
    }
    #endregion
}

public enum AsteroidType { Iron, Silver, Gold }