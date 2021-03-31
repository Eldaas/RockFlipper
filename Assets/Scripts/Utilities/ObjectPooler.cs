﻿using System.Collections;
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
    private GameObject[] powerupPrefabs;
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
    private List<GameObject> pooledBackgroundAsteroids = new List<GameObject>();

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

        pooledGasClouds = new List<GameObject>();
        for (int i = 0; i < gasCloudCount; i++)
        {
            GameObject go = Instantiate(gasCloudPrefabs[Utility.GenerateRandomInt(0, gasCloudPrefabs.Length)]);
            go.transform.parent = gasCloudsParent.transform;
            go.SetActive(false);
            pooledGasClouds.Add(go);
        }

        pooledBlackHoles = new List<GameObject>();
        for (int i = 0; i < blackHoleCount; i++)
        {
            GameObject go = Instantiate(blackHolePrefabs[Utility.GenerateRandomInt(0, blackHolePrefabs.Length)]);
            go.transform.parent = blackHolesParent.transform;
            go.SetActive(false);
            pooledBlackHoles.Add(go);
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
}