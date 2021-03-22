using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner")]
    public GameObject player;
    public float offsetFromPlayer;

    [Header("Collidable Asteroids")]
    public Bounds asteroidBounds = new Bounds();
    private Vector3 asteroidBoundsOffset;
    public List<GameObject> activeAsteroids = new List<GameObject>();
    public float minAsteroidScaleFactor;
    public float maxAsteroidScaleFactor;
    public int currentAsteroidCap = 1;

    [Header("Collectables")]
    public Bounds collectableBounds = new Bounds();
    private Vector3 collectableBoundsOffset;
    public List<GameObject> activeCrystals = new List<GameObject>();
    public int currentCrystalCap = 1;

    [Header("Background Asteroids")]
    public Bounds backgroundAsteroidBounds = new Bounds();
    private Vector3 backgroundAsteroidBoundsOffset;
    public List<GameObject> activeBackgroundAsteroids = new List<GameObject>();
    public float minBackgroundScaleFactor;
    public float maxBackgroundScaleFactor;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        asteroidBoundsOffset = asteroidBounds.center;
        collectableBoundsOffset = collectableBounds.center;
        backgroundAsteroidBoundsOffset = backgroundAsteroidBounds.center;
    }

    private void Start()
    {
        PopulateBackgroundField();
        InvokeRepeating("IncreaseAsteroidCap", 2.5f, 2.5f);
        InvokeRepeating("IncreaseCrystalCap", 5f, 2f);
    }

    private void Update()
    {
        CleanUp();
        transform.position = (player.transform.position.z + offsetFromPlayer) * Vector3.forward;
        asteroidBounds.center = transform.position + asteroidBoundsOffset;
        collectableBounds.center = transform.position + collectableBoundsOffset;
        backgroundAsteroidBounds.center = transform.position + backgroundAsteroidBoundsOffset;

        if(activeAsteroids.Count < currentAsteroidCap)
        {
            GameObject asteroid = ObjectPooler.instance.GetPooledAsteroid();
            Vector3 spawnPoint = GetAsteroidSpawnPoint();
            float randomScaleFactor = Utility.GenerateRandomFloat(minAsteroidScaleFactor, maxAsteroidScaleFactor);
            activeAsteroids.Add(asteroid);
            asteroid.transform.position = spawnPoint;
            asteroid.transform.localScale = Vector3.one * randomScaleFactor;
            asteroid.SetActive(true);
        }

        if (activeBackgroundAsteroids.Count < ObjectPooler.instance.backgroundAsteroidCount)
        {
            GameObject asteroid = ObjectPooler.instance.GetPooledBackgroundAsteroid();
            Vector3 spawnPoint = GetBackgroundSpawnPoint();
            float randomScaleFactor = Utility.GenerateRandomFloat(minBackgroundScaleFactor, maxBackgroundScaleFactor);
            activeBackgroundAsteroids.Add(asteroid);
            asteroid.transform.position = spawnPoint;
            asteroid.transform.localScale = Vector3.one * randomScaleFactor;
            asteroid.SetActive(true);
        }

    }

    private Vector3 GetAsteroidSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(
            Random.Range(asteroidBounds.center.x - asteroidBounds.extents.x, asteroidBounds.center.x + asteroidBounds.extents.x), 
            Random.Range(asteroidBounds.center.y - asteroidBounds.extents.y, asteroidBounds.center.y + asteroidBounds.extents.y), 
            Random.Range(asteroidBounds.center.z - asteroidBounds.extents.z, asteroidBounds.center.z + asteroidBounds.extents.z)
            );
        return spawnPoint;
    }

    private Vector3 GetCollectableSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(
            Random.Range(collectableBounds.center.x - collectableBounds.extents.x, collectableBounds.center.x + collectableBounds.extents.x),
            Random.Range(collectableBounds.center.y - collectableBounds.extents.y, collectableBounds.center.y + collectableBounds.extents.y),
            Random.Range(collectableBounds.center.z - collectableBounds.extents.z, collectableBounds.center.z + collectableBounds.extents.z)
            );
        return spawnPoint;
    }

    private Vector3 GetBackgroundSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(
            Random.Range(backgroundAsteroidBounds.center.x - backgroundAsteroidBounds.extents.x, backgroundAsteroidBounds.center.x + backgroundAsteroidBounds.extents.x),
            Random.Range(backgroundAsteroidBounds.center.y - backgroundAsteroidBounds.extents.y, backgroundAsteroidBounds.center.y + backgroundAsteroidBounds.extents.y),
            Random.Range(backgroundAsteroidBounds.center.z - backgroundAsteroidBounds.extents.z, backgroundAsteroidBounds.center.z + backgroundAsteroidBounds.extents.z)
            );
        return spawnPoint;
    }

    private void IncreaseAsteroidCap()
    {
        if(currentAsteroidCap < ObjectPooler.instance.asteroidCount)
        {
            currentAsteroidCap++;
        }
    }

    private void IncreaseCrystalCap()
    {
        if (currentCrystalCap < ObjectPooler.instance.crystalCount)
        {
            currentCrystalCap++;
        }
    }

    private void CleanUp()
    {
        foreach (GameObject asteroid in activeAsteroids.ToArray())
        {
            if (asteroid.transform.position.z < player.transform.position.z - 100f)
            {
                Rigidbody rb = asteroid.GetComponent<Rigidbody>();
                activeAsteroids.Remove(asteroid);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                asteroid.SetActive(false);
            }
        }

        foreach (GameObject asteroid in activeBackgroundAsteroids.ToArray())
        {
            if (asteroid.transform.position.z < player.transform.position.z - 200f)
            {
                activeBackgroundAsteroids.Remove(asteroid);
                asteroid.SetActive(false);
            }
        }
    }

    private void PopulateBackgroundField()
    {
        Vector3 previousCenter = backgroundAsteroidBounds.center;
        Vector3 previousExtents = backgroundAsteroidBounds.extents;

        Vector3 newCenter = new Vector3(previousCenter.x, previousCenter.y, previousCenter.z / 2);
        backgroundAsteroidBounds.center = newCenter;
        Vector3 newExtents = new Vector3(previousExtents.x, previousExtents.y, previousCenter.z / 2);
        backgroundAsteroidBounds.extents = newExtents;

        // Refactor this later - not DRY code
        for (int i = 0; i < ObjectPooler.instance.backgroundAsteroidCount; i++)
        {
            GameObject asteroid = ObjectPooler.instance.GetPooledBackgroundAsteroid();
            Vector3 spawnPoint = GetBackgroundSpawnPoint();
            float randomScaleFactor = Utility.GenerateRandomFloat(minBackgroundScaleFactor, maxBackgroundScaleFactor);
            activeBackgroundAsteroids.Add(asteroid);
            asteroid.transform.position = spawnPoint;
            asteroid.transform.localScale = Vector3.one * randomScaleFactor;
            asteroid.SetActive(true);
        }

        backgroundAsteroidBounds.center = previousCenter;
        backgroundAsteroidBounds.extents = previousExtents;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(asteroidBounds.center, asteroidBounds.size);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(collectableBounds.center, collectableBounds.size);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(backgroundAsteroidBounds.center, backgroundAsteroidBounds.size);
    }


}
