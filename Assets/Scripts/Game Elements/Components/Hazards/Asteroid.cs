using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object is designed to be used with our object pooling system. Inside the pooling system simply access this component and call the relevant methods.
/// </summary>
public class Asteroid : Hazard
{
    [Header("Inspector References")]
    [SerializeField]
    private AsteroidType asteroidType;
    public AsteroidData data;

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
        for (int i = 0; i < childrenObjects.Count; i++)
        {
            CachePositionRotation(i);
            ApplyMaterials(i);
            childrenObjects[i].gameObject.SetActive(true);
            rb = childrenObjects[i].GetComponent<Rigidbody>();
            rb.AddExplosionForce(force, mainObject.transform.position, explosionRadius);
        }
        Debug.Log("Asteroid has exploded");
    }

    /// <summary>
    /// This method can be called to handle behaviour when an object (such as the player) collides with the object.
    /// </summary>
    public void CollideWithAsteroid()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        PopulateChildrenObjects();
        mainObject.SetActive(false);
        for (int i = 0; i < childrenObjects.Count; i++)
        {
            ApplyMaterials(i);
            CachePositionRotation(i);
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
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Applies materials to the input shard depending upon what type the asteroid is set to.
    /// </summary>
    /// <param name="i">The shard being iterated over.</param>
    private void ApplyMaterials(int i)
    {
        MeshRenderer renderer = childrenObjects[i].GetComponent<MeshRenderer>();

        switch (asteroidType)
        {
            case AsteroidType.Iron:
                renderer.materials = new Material[] { renderer.materials[0], data.ironMaterial };
                break;
            case AsteroidType.Silver:
                renderer.materials = new Material[] { renderer.materials[0], data.silverMaterial };
                break;
            case AsteroidType.Gold:
                renderer.materials = new Material[] { renderer.materials[0], data.goldMaterial };
                break;
            default:
                Debug.Log("Couldn't find the appropriate material for the asteroid.");
                break;
        }

    }
    #endregion

    #region Unity Methods
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        Debug.Log(collision.collider.name);

        if(collision.collider.CompareTag("Projectile"))
        {
            Debug.Log("Projectile collided with asteroid.");
        }
    }
    #endregion
}

enum AsteroidType { Iron, Silver, Gold }