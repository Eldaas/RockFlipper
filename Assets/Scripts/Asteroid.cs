using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object is designed to be used with an object pooling system. Inside the pooling system simply access this component and call the relevant methods.
/// </summary>
public class Asteroid : MonoBehaviour
{
    [Header("Inspector References")]
    [SerializeField]
    private AsteroidType asteroidType;
    [SerializeField]
    private GameObject mainAsteroid;
    [SerializeField]
    private GameObject shardsParent;
    [SerializeField]
    private Material ironMaterial;
    [SerializeField]
    private Material silverMaterial;
    [SerializeField]
    private Material goldMaterial;

    [SerializeField]
    private List<Transform> shards;
    private List<Vector3> shardsOriginalPositions = new List<Vector3>();
    private List<Quaternion> shardsOriginalRotations = new List<Quaternion>();

    #region Public Methods
    /// <summary>
    /// Disables the main asteroid, activates all shards and adds an explosion force.
    /// </summary>
    /// <param name="force">The kinetic force to apply to the shards.</param>
    /// <param name="explosionRadius">The radius of the explosion.</param>
    public void ExplodeAsteroid(float force, float explosionRadius)
    {
        PopulateShardList();
        mainAsteroid.SetActive(false);
        for (int i = 0; i < shards.Count; i++)
        {
            CachePositionRotation(i);
            ApplyMaterials(i);
            shards[i].gameObject.SetActive(true);
            Rigidbody rb = shards[i].GetComponent<Rigidbody>();
            rb.AddExplosionForce(force, mainAsteroid.transform.position, explosionRadius);
        }
        Debug.Log("Asteroid has exploded");
    }

    /// <summary>
    /// This method can be called to handle behaviour when an object (such as the player) collides with the object.
    /// </summary>
    public void CollideWithAsteroid()
    {
        PopulateShardList();
        mainAsteroid.SetActive(false);
        for (int i = 0; i < shards.Count; i++)
        {
            ApplyMaterials(i);
            CachePositionRotation(i);
            shards[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Resets the positions of all asteroid shards, deactivates them and reactivates the main asteroid.
    /// </summary>
    public void ResetAsteroid()
    {
        for (int i = 0; i < shards.Count; i++)
        {
            shards[i].transform.position = shardsOriginalPositions[i];
            shards[i].transform.rotation = shardsOriginalRotations[i];
            shards[i].gameObject.SetActive(false);
        }

        mainAsteroid.SetActive(true);
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Gets all of the shards from the parent game object and puts them in a list.
    /// </summary>
    private void PopulateShardList()
    {
        shards = new List<Transform>(shardsParent.GetComponentsInChildren<Transform>(true));
        shards.Remove(shardsParent.transform);
    }

    /// <summary>
    /// Stores the original position and rotation of the input shard so that the shard can be reset when returned to the pool.
    /// </summary>
    /// <param name="i">The shard being iterated over.</param>
    private void CachePositionRotation(int i)
    {
        shardsOriginalPositions.Add(shards[i].transform.localPosition);
        shardsOriginalRotations.Add(shards[i].transform.localRotation);
    }

    /// <summary>
    /// Applies materials to the input shard depending upon what type the asteroid is set to.
    /// </summary>
    /// <param name="i">The shard being iterated over.</param>
    private void ApplyMaterials(int i)
    {
        Debug.Log("BREAK");
        MeshRenderer renderer = shards[i].GetComponent<MeshRenderer>();

        switch (asteroidType)
        {
            case AsteroidType.Iron:
                Debug.Log("BREAK1");
                renderer.materials = new Material[] { renderer.materials[0], ironMaterial };
                break;
            case AsteroidType.Silver:
                Debug.Log("BREAK2");
                renderer.materials = new Material[] { renderer.materials[0], silverMaterial };
                break;
            case AsteroidType.Gold:
                renderer.materials = new Material[] { renderer.materials[0], goldMaterial };
                break;
            default:
                Debug.Log("Couldn't find the appropriate material for the asteroid.");
                break;
        }

    }
    #endregion

    #region Unity Methods
    public void OnCollisionEnter(Collision collision)
    {
        CollideWithAsteroid();
    }
    #endregion

}

enum AsteroidType { Iron, Silver, Gold }