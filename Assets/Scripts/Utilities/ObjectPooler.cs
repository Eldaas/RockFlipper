using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler instance;

    [Header("Hazard: Asteroids")]
    [SerializeField]
    private GameObject[] asteroidPrefabs;
    [SerializeField]
    private GameObject asteroidsParent;
    public int asteroidCount;
    private List<GameObject> pooledAsteroids = new List<GameObject>();

    [Header("Hazard: Gas Clouds")]
    [SerializeField]
    private GameObject[] gasCloudPrefabs;
    [SerializeField]
    private GameObject gasCloudsParent;
    public int gasCloudCount;
    private List<GameObject> pooledGasClouds = new List<GameObject>();

    [Header("Hazard: Black Holes")]
    [SerializeField]
    private GameObject[] blackHolePrefabs;
    [SerializeField]
    private GameObject blackHolesParent;
    public int blackHoleCount;
    public List<GameObject> pooledBlackHoles = new List<GameObject>();

    [Header("Collectable: Powerups")]
    [SerializeField]
    private GameObject genericPrefab;
    [SerializeField]
    private List<Powerup> powerupProfiles;
    [SerializeField]
    private GameObject powerupsParent;
    public int powerupCount;
    private List<GameObject> pooledPowerups = new List<GameObject>();

    [Header("Background Asteroids")]
    [SerializeField]
    private GameObject[] backgroundAsteroidPrefabs;
    [SerializeField]
    private GameObject backgroundAsteroidsParent;
    public int backgroundAsteroidCount;
    public int backgroundAsteroidPrespawnCount;
    private List<GameObject> pooledBackgroundAsteroids = new List<GameObject>();

    [Header("Projectiles")]
    [SerializeField]
    private GameObject[] projectiles;
    [SerializeField]
    private GameObject projectilesParent;
    [SerializeField]
    private int projectileCount;
    private List<GameObject> pooledProjectiles = new List<GameObject>();

    [Header("Projectile Hit FX")]
    [SerializeField]
    private GameObject[] particleHitFx;
    [SerializeField]
    private GameObject particleHitFxParent;
    [SerializeField]
    private int particleHitFxCount;
    private List<GameObject> pooledParticleHitFx = new List<GameObject>();

    [Header("Resource Particle FX")]
    [SerializeField]
    private GameObject asteroidChunks;
    [SerializeField]
    private GameObject asteroidChunksParent;
    [SerializeField]
    private int asteroidChunksCount;
    private List<GameObject> pooledAsteroidChunks = new List<GameObject>();

    private void Awake()
    {
        #region Singleton
        ObjectPooler[] list = FindObjectsOfType<ObjectPooler>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the Object Pooler component detected. Destroying an instance.");
        }
        else
        {
            instance = this;
        }
        #endregion
    }

    private void Start()
    {
        InstantiatePools();
    }

    /// <summary>
    /// Initialises all object pools and instantiates the requisite number of objects.
    /// </summary>
    public void InstantiatePools()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            GameObject go = Instantiate(asteroidPrefabs[Utility.GenerateRandomInt(0, asteroidPrefabs.Length)]);
            go.transform.parent = asteroidsParent.transform;
            go.SetActive(false);
            pooledAsteroids.Add(go);

            Asteroid asteroid = go.GetComponent<Asteroid>();
            ApplyAsteroidMaterial(asteroid);
            //asteroid.PopulateChildrenObjects();

            /*for (int j = 0; j < asteroid.childrenObjects.Count; j++)
            {
                ApplyShardMaterials(asteroid, j);
                asteroid.CachePositionRotation(j);
            }*/
        }

        //pooledGasClouds = new List<GameObject>();
        for (int i = 0; i < gasCloudCount; i++)
        {
            GameObject go = Instantiate(gasCloudPrefabs[Utility.GenerateRandomInt(0, gasCloudPrefabs.Length)]);
            go.transform.parent = gasCloudsParent.transform;
            go.SetActive(false);
            pooledGasClouds.Add(go);
        }

        //pooledBlackHoles = new List<GameObject>();
        for (int i = 0; i < blackHoleCount; i++)
        {
            GameObject go = Instantiate(blackHolePrefabs[Utility.GenerateRandomInt(0, blackHolePrefabs.Length)]);
            go.transform.parent = blackHolesParent.transform;
            go.SetActive(false);
            pooledBlackHoles.Add(go);
        }

        //pooledBackgroundAsteroids = new List<GameObject>();
        for (int i = 0; i < backgroundAsteroidCount; i++)
        {
            GameObject go = Instantiate(backgroundAsteroidPrefabs[Utility.GenerateRandomInt(0, backgroundAsteroidPrefabs.Length)]);
            go.transform.parent = backgroundAsteroidsParent.transform;
            go.SetActive(false);
            pooledBackgroundAsteroids.Add(go);
        }

        //pooledPowerups = new List<GameObject>();
        for (int i = 0; i < powerupCount; i++)
        {
            // TO DO: Math to assign powerup profile based upon % spawn chance
            
        }

        //pooledProjectiles = new List<GameObject>();
        for (int i = 0; i < projectileCount; i++)
        {
            GameObject go = Instantiate(projectiles[0]);
            go.name = go.name + " " + i;
            go.transform.parent = projectilesParent.transform;
            go.SetActive(false);
            pooledProjectiles.Add(go);
        }

        //pooledParticleHitFx = new List<GameObject>();
        for (int i = 0; i < particleHitFxCount; i++)
        {
            GameObject go = Instantiate(particleHitFx[0]);
            go.name = go.name + " " + i;
            go.transform.parent = particleHitFxParent.transform;
            go.SetActive(false);
            pooledParticleHitFx.Add(go);
        }

        for (int i = 0; i < asteroidChunksCount; i++)
        {
            GameObject go = Instantiate(asteroidChunks);
            go.name = go.name + " " + i;
            go.transform.parent = asteroidChunksParent.transform;
            go.SetActive(false);
            pooledAsteroidChunks.Add(go);
        }

    }

    /// <summary>
    /// Returns first inactive asteroid in the hierarchy.
    /// </summary>
    public GameObject GetPooledAsteroid()
    {
        for (int i = 0; i < pooledAsteroids.Count; i++)
        {
            if(!pooledAsteroids[i].activeInHierarchy)
            {
                return pooledAsteroids[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Returns first inactive gas cloud in the hierarchy.
    /// </summary>
    public GameObject GetPooledGasCloud()
    {
        for (int i = 0; i < pooledGasClouds.Count; i++)
        {
            if (!pooledGasClouds[i].activeInHierarchy)
            {
                return pooledGasClouds[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Returns first inactive black hole in the hierarchy.
    /// </summary>
    public GameObject GetPooledBlackHole()
    {
        for (int i = 0; i < pooledBlackHoles.Count; i++)
        {
            if (!pooledBlackHoles[i].activeInHierarchy)
            {
                return pooledBlackHoles[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Returns first inactive background asteroid in the hierarchy.
    /// </summary>
    public GameObject GetPooledBackgroundAsteroid()
    {
        for (int i = 0; i < pooledBackgroundAsteroids.Count; i++)
        {
            if (!pooledBackgroundAsteroids[i].activeInHierarchy)
            {
                return pooledBackgroundAsteroids[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the first inactive projectile in the hierarchy.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledProjectile()
    {
        for (int i = 0; i < pooledProjectiles.Count; i++)
        {
            if(!pooledProjectiles[i].activeInHierarchy)
            {
                return pooledProjectiles[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the first inactive particle hit FX in the hierarchy.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledHitFx()
    {
        for (int i = 0; i < pooledParticleHitFx.Count; i++)
        {
            if (!pooledParticleHitFx[i].activeInHierarchy)
            {
                return pooledParticleHitFx[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the first inactive asteroid chunks particle in the hierarchy.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledAsteroidChunk()
    {
        for (int i = 0; i < pooledAsteroidChunks.Count; i++)
        {
            if (!pooledAsteroidChunks[i].activeInHierarchy)
            {
                return pooledAsteroidChunks[i];
            }
        }
        return null;
    }

    public IEnumerator ReturnParticleToPool(GameObject particleParent, float lifetime)
    {
        //Debug.Log("Particle being returned to pool in " + lifetime + " seconds.");
        float count = Time.time + lifetime;

        while (Time.time < count)
        {
            yield return null;
        }

        Transform[] particleChildren = particleParent.GetComponentsInChildren<Transform>(true);
        foreach(Transform child in particleChildren)
        {
            child.gameObject.SetActive(true);
        }

        particleParent.SetActive(false);
    }

    private void ApplyAsteroidMaterial(Asteroid asteroid)
    {
        MeshRenderer renderer = asteroid.mainObject.GetComponent<MeshRenderer>();
        Material[] mats = renderer.materials;

        switch (asteroid.asteroidType)
        {
            case AsteroidType.Iron:
                //renderer.materials = new Material[] { renderer.materials[0], asteroid.data.ironRockMaterial };
                mats[0] = asteroid.data.ironRockMaterial;
                break;
            case AsteroidType.Silver:
                //renderer.materials = new Material[] { renderer.materials[0], asteroid.data.silverRockMaterial };
                mats[0] = asteroid.data.silverRockMaterial;
                break;
            case AsteroidType.Gold:
                //renderer.materials = new Material[] { renderer.materials[0], asteroid.data.goldRockMaterial };
                mats[0] = asteroid.data.goldRockMaterial;
                break;
            default:
                Debug.Log("Couldn't find the appropriate material for the asteroid.");
                break;
        }

        renderer.materials = mats;
    }

    /// <summary>
    /// Applies materials to the input shard depending upon what type the asteroid is set to.
    /// </summary>
    /// <param name="i">The shard being iterated over.</param>
    private void ApplyShardMaterials(Asteroid asteroid, int i)
    {
        MeshRenderer renderer = asteroid.childrenObjects[i].GetComponent<MeshRenderer>();
        Material[] mats = renderer.materials;

        switch (asteroid.asteroidType)
        {
            case AsteroidType.Iron:
                mats[0] = asteroid.data.ironMaterial;
                mats[1] = asteroid.data.ironRockMaterial;
                break;
            case AsteroidType.Silver:
                mats[0] = asteroid.data.silverMaterial;
                mats[1] = asteroid.data.silverRockMaterial;
                break;
            case AsteroidType.Gold:
                mats[0] = asteroid.data.goldMaterial;
                mats[1] = asteroid.data.goldRockMaterial;
                break;
            default:
                Debug.Log("Couldn't find the appropriate material for the asteroid.");
                break;
        }

        renderer.materials = mats;

    }
}
