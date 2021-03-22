using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler instance;

    public List<GameObject> pooledAsteroids = new List<GameObject>();
    public GameObject[] asteroidPrefabs;
    public GameObject asteroidsParent;
    public int asteroidCount;

    public List<GameObject> pooledCrystals = new List<GameObject>();
    public GameObject[] crystalPrefabs;
    public GameObject crystalsParent;
    public int crystalCount;

    public List<GameObject> pooledBackgroundAsteroids = new List<GameObject>();
    public GameObject[] backgroundAsteroidPrefabs;
    public GameObject backgroundAsteroidsParent;
    public int backgroundAsteroidCount;

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
        pooledAsteroids = new List<GameObject>();
        for (int i = 0; i < asteroidCount; i++)
        {
            GameObject go = Instantiate(asteroidPrefabs[Utility.GenerateRandomInt(0, asteroidPrefabs.Length)]);
            go.transform.parent = asteroidsParent.transform;
            go.SetActive(false);
            pooledAsteroids.Add(go);
        }

        pooledCrystals = new List<GameObject>();
        for (int i = 0; i < crystalCount; i++)
        {
            GameObject go = Instantiate(crystalPrefabs[Utility.GenerateRandomInt(0, crystalPrefabs.Length)]);
            go.transform.parent = crystalsParent.transform;
            go.SetActive(false);
            pooledCrystals.Add(go);
        }

        pooledBackgroundAsteroids = new List<GameObject>();
        for (int i = 0; i < backgroundAsteroidCount; i++)
        {
            GameObject go = Instantiate(backgroundAsteroidPrefabs[Utility.GenerateRandomInt(0, backgroundAsteroidPrefabs.Length)]);
            go.transform.parent = backgroundAsteroidsParent.transform;
            go.SetActive(false);
            pooledBackgroundAsteroids.Add(go);
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
    /// Returns first inactive crystal in the hierarchy.
    /// </summary>
    public GameObject GetPooledCrystal()
    {
        for (int i = 0; i < pooledCrystals.Count; i++)
        {
            if (!pooledCrystals[i].activeInHierarchy)
            {
                return pooledCrystals[i];
            }
        }
        return null;
    }

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
}
