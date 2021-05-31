using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler instance;
    [SerializeField]
    private Spawner spawner;
    private LevelModifier level;

    [Header("Hazard: Asteroids")]
    [SerializeField]
    private GameObject[] asteroidPrefabs;
    [SerializeField]
    private GameObject asteroidsParent;
    [SerializeField]
    private GameObject asteroidHpBarPrefab;
    [SerializeField]
    private GameObject asteroidHpBarsParent;
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
    // Powerup prefabs are assigned through the LevelPowerups profile, as they are scene specific.
    // LevelPowerups profile is assigned via the scene controller. 
    [SerializeField]
    private GameObject powerupsParent;
    public int powerupCount;
    private List<GameObject> pooledPowerups = new List<GameObject>();
    private List<GameObject> levelPowerups = new List<GameObject>();

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

    [Header("Resource Collectables")]
    [SerializeField]
    private GameObject ironPrefab;
    [SerializeField]
    private GameObject silverPrefab;
    [SerializeField]
    private GameObject goldPrefab;
    [SerializeField]
    private GameObject ironParent;
    [SerializeField]
    private GameObject silverParent;
    [SerializeField]
    private GameObject goldParent;
    [SerializeField]
    private int resourcesCount;
    private List<GameObject> pooledIron = new List<GameObject>();
    private List<GameObject> pooledSilver = new List<GameObject>();
    private List<GameObject> pooledGold = new List<GameObject>();

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

        level = SceneController.instance.levelMods;
    }

    private void Start()
    {
        InstantiatePools();

        
        Debug.LogError("BREAK");
        Debug.Log($"LevelMods loaded: {level.asteroidDensityMultiplier}");
    }

    /// <summary>
    /// Initialises all object pools and instantiates the requisite number of objects.
    /// </summary>
    public void InstantiatePools()
    {
        levelPowerups = SceneController.instance.levelPowerups.powerups;

        for (int i = 0; i < asteroidCount; i++)
        {
            GameObject go = Instantiate(asteroidPrefabs[Utility.GenerateRandomInt(0, asteroidPrefabs.Length)]);
            go.transform.parent = asteroidsParent.transform;
            go.name = go.name + " " + i;
            go.SetActive(false);
            pooledAsteroids.Add(go);

            Asteroid asteroid = go.GetComponent<Asteroid>();
            asteroid.asteroidType = GenerateTypeFromData();
            asteroid.AssignCollectableType();
            ApplyAsteroidMaterial(asteroid);
            asteroid.SetAsteroidHealth();
            asteroid.MultiplyAsteroidMass();
            GameObject healthBar = Instantiate(asteroidHpBarPrefab, asteroidHpBarsParent.transform);
            asteroid.healthBar = healthBar.GetComponent<HealthBar>();
            asteroid.healthBar.Deactivate();
        }

        for (int i = 0; i < gasCloudCount; i++)
        {
            GameObject go = Instantiate(gasCloudPrefabs[Utility.GenerateRandomInt(0, gasCloudPrefabs.Length)]);
            go.transform.parent = gasCloudsParent.transform;
            go.SetActive(false);
            pooledGasClouds.Add(go);
        }

        for (int i = 0; i < blackHoleCount; i++)
        {
            GameObject go = Instantiate(blackHolePrefabs[Utility.GenerateRandomInt(0, blackHolePrefabs.Length)]);
            go.transform.parent = blackHolesParent.transform;
            go.SetActive(false);
            pooledBlackHoles.Add(go);
        }

        for (int i = 0; i < backgroundAsteroidCount; i++)
        {
            GameObject go = Instantiate(backgroundAsteroidPrefabs[Utility.GenerateRandomInt(0, backgroundAsteroidPrefabs.Length)]);
            go.transform.parent = backgroundAsteroidsParent.transform;
            go.SetActive(false);
            pooledBackgroundAsteroids.Add(go);
        }

        List<GameObject> hits = new List<GameObject>();

        for (int i = 0; i < powerupCount; i++)
        {
            bool generated = false;

            if(levelPowerups.Count > 0)
            {
                while (!generated)
                {
                    if (GeneratePowerup(levelPowerups, hits))
                    {
                        generated = true;
                    }
                }

                while (hits.Count > 1)
                {
                    GameObject[] array = hits.ToArray();

                    foreach (GameObject powerup in array)
                    {
                        int randomInt = Utility.GenerateRandomInt(0, 100);
                        if (randomInt < 50 && hits.Count > 1) // Have to check hits.Count again - not an error
                        {
                            hits.Remove(powerup);
                        }
                    }
                }

                GameObject instantiatedPowerup = Instantiate(hits[0], powerupsParent.transform);
                pooledPowerups.Add(instantiatedPowerup);
                instantiatedPowerup.SetActive(false);
            }
            else
            {
                Debug.LogError("There were no powerups defined within the levelPowerups profile attached to the SceneManager.");
            }
        }
        

        for (int i = 0; i < projectileCount; i++)
        {
            GameObject go = Instantiate(projectiles[0]);
            go.name = go.name + " " + i;
            go.transform.parent = projectilesParent.transform;
            go.SetActive(false);
            pooledProjectiles.Add(go);
        }

        for (int i = 0; i < particleHitFxCount; i++)
        {
            GameObject go = Instantiate(particleHitFx[0]);
            go.name = go.name + " " + i;
            go.transform.parent = particleHitFxParent.transform;
            go.SetActive(false);
            pooledParticleHitFx.Add(go);
        }

        for (int i = 0; i < resourcesCount; i++)
        {
            GameObject go = Instantiate(ironPrefab);
            go.name = go.name + " " + i;
            go.transform.parent = ironParent.transform;
            go.tag = "Iron";
            go.SetActive(false);
            pooledIron.Add(go);

            go = Instantiate(silverPrefab);
            go.name = go.name + " " + i;
            go.transform.parent = silverParent.transform;
            go.tag = "Silver";
            go.SetActive(false);
            pooledSilver.Add(go);

            go = Instantiate(goldPrefab);
            go.name = go.name + " " + i;
            go.transform.parent = goldParent.transform;
            go.tag = "Gold";
            go.SetActive(false);
            pooledGold.Add(go);
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
    /// Returns the first inactive iron collectable in the hierarchy.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledIron()
    {
        for (int i = 0; i < pooledIron.Count; i++)
        {
            if (!pooledIron[i].activeInHierarchy)
            {
                spawner.activeIron.Add(pooledIron[i]);
                return pooledIron[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the first inactive silver collectable in the hierarchy.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledSilver()
    {
        for (int i = 0; i < pooledSilver.Count; i++)
        {
            if (!pooledSilver[i].activeInHierarchy)
            {
                spawner.activeSilver.Add(pooledSilver[i]);
                return pooledSilver[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the first inactive gold collectable in the hierarchy.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledGold()
    {
        for (int i = 0; i < pooledGold.Count; i++)
        {
            if (!pooledGold[i].activeInHierarchy)
            {
                spawner.activeGold.Add(pooledGold[i]);
                return pooledGold[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Returns a RANDOM inactive powerup in the hierarchy.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledPowerup()
    {
        if(pooledPowerups.Count > 0)
        {
            int randomIndex = Utility.GenerateRandomInt(0, pooledPowerups.Count - 1);

            if (!pooledPowerups[randomIndex].activeInHierarchy)
            {
                return pooledPowerups[randomIndex];
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

    #region Private Methods
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
            case AsteroidType.Barren:
                mats[0] = asteroid.data.barrenAsteroidMaterial;
                mats[1] = asteroid.data.barrenAsteroidMaterial;
                break;
            case AsteroidType.Iron:
                mats[0] = asteroid.data.ironRockMaterial;
                mats[1] = asteroid.data.ironRockMaterial;
                break;
            case AsteroidType.Silver:
                mats[0] = asteroid.data.silverRockMaterial;
                mats[1] = asteroid.data.silverRockMaterial;
                break;
            case AsteroidType.Gold:
                mats[0] = asteroid.data.goldRockMaterial;
                mats[1] = asteroid.data.goldRockMaterial;
                break;
            default:
                Debug.Log("Couldn't find the appropriate material for the asteroid.");
                break;
        }

        renderer.materials = mats;

    }


    private bool GeneratePowerup(List<GameObject> levelPowerups, List<GameObject> hits)
    {
        bool generated = false;
        
        foreach (GameObject powerup in levelPowerups)
        {
            Debug.Log($"{powerup.gameObject.name}");
            IPowerup thisPowerup = powerup.GetComponent<IPowerup>();
            float randomInt = Utility.GenerateRandomInt(0, 100);
            if (thisPowerup.ChanceToSpawn != 0f && randomInt <= thisPowerup.ChanceToSpawn)
            {
                hits.Add(powerup);
                generated = true;
            }
        }

        if(generated)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    private AsteroidType GenerateTypeFromData()
    {
        if (level.goldChance > 0f)
        {
            float diceRoll = Utility.GenerateRandomFloat(0f, 1f);
            if(diceRoll <= level.goldChance / 100)
            {
                return AsteroidType.Gold;
            }
        }

        if (level.silverChance > 0f)
        {
            float diceRoll = Utility.GenerateRandomFloat(0f, 1f);
            if (diceRoll <= level.silverChance / 100)
            {
                return AsteroidType.Silver;
            }
        }

        if(level.ironChance > 0f)
        {
            float diceRoll = Utility.GenerateRandomFloat(0f, 1f);
            if (diceRoll <= level.ironChance / 100)
            {
                return AsteroidType.Iron;
            }
        }

        return AsteroidType.Barren;
    }
    #endregion
}
