using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner")]
    public GameObject player;
    public float offsetFromPlayer;
    public float cleanupDistanceFromPlayer = 200f;

    [Header("General Hazard Settings")]
    public Bounds hazardBounds = new Bounds();
    private Vector3 hazardBoundsOffset;

    [Header("Hazard: Asteroids")]
    public List<GameObject> activeAsteroids = new List<GameObject>();
    public float minAsteroidScaleFactor;
    public float maxAsteroidScaleFactor;
    public int currentAsteroidCap = 0;
    // Raise asteroid density above 1f to increase spacing between asteroid spawns.
    public float asteroidDensity = 1f;

    [Header("Hazard: Gas Clouds")]
    public List<GameObject> activeGasClouds = new List<GameObject>();
    public float minGasCloudScaleFactor;
    public float maxGasCloudScaleFactor;
    public int currentGasCloudCap = 0;
    public float gasCloudDensity = 1;

    [Header("Hazard: Black Holes")]
    public List<GameObject> activeBlackHoles = new List<GameObject>();
    public float minBlackHoleScaleFactor;
    public float maxBlackHoleScaleFactor;
    public int currentBlackHoleCap = 0;
    public float blackHoleDensity = 1;

    [Header("Collectables: Powerups")]
    public Bounds powerupBounds = new Bounds();
    private Vector3 powerupBoundsOffset;
    public List<GameObject> activePowerups = new List<GameObject>();

    [Header("Background Asteroids")]
    public Bounds backgroundAsteroidBounds = new Bounds();
    private Vector3 backgroundAsteroidBoundsOffset;
    public List<GameObject> activeBackgroundAsteroids = new List<GameObject>();
    public float minBackgroundScaleFactor;
    public float maxBackgroundScaleFactor;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        powerupBoundsOffset = powerupBounds.center;
        backgroundAsteroidBoundsOffset = backgroundAsteroidBounds.center;
    }

    private void Start()
    {
        PopulateBackgroundField();
        InvokeRepeating("IncreaseAsteroidCap", 1f, 1f);
    }

    private void Update()
    {
        CleanUp();
        transform.position = (player.transform.position.z + offsetFromPlayer) * Vector3.forward;
        hazardBounds.center = transform.position;
        powerupBounds.center = transform.position + powerupBoundsOffset;
        backgroundAsteroidBounds.center = transform.position + backgroundAsteroidBoundsOffset;

        if(activeAsteroids.Count < currentAsteroidCap)
        {
            GameObject asteroid = ObjectPooler.instance.GetPooledAsteroid();
            Vector3 spawnPoint;
            float randomScaleFactor = Utility.GenerateRandomFloat(minAsteroidScaleFactor, maxAsteroidScaleFactor);
            asteroid.transform.localScale = Vector3.one * randomScaleFactor;
            Asteroid roid = asteroid.GetComponent<Asteroid>();
            Bounds roidBounds = roid.mainObject.GetComponent<MeshRenderer>().bounds;

            bool boundsCheck = false;
            int maxAttempts = 3;
            while(!boundsCheck && maxAttempts >= 0)
            {
                spawnPoint = GetHazardSpawnPoint();
                Collider[] list = Physics.OverlapBox(spawnPoint, roidBounds.extents * asteroidDensity);
                if (list.Length == 0)
                {
                    activeAsteroids.Add(asteroid);
                    asteroid.transform.position = spawnPoint;
                    asteroid.SetActive(true);
                    boundsCheck = true;
                }
                else
                {
                    maxAttempts--;
                }
            }
            
 
        }

        if (activeGasClouds.Count < currentGasCloudCap)
        {
            GameObject gasCloud = ObjectPooler.instance.GetPooledGasCloud();
            Vector3 spawnPoint = GetHazardSpawnPoint();
            float randomScaleFactor = Utility.GenerateRandomFloat(minGasCloudScaleFactor, maxGasCloudScaleFactor);
            activeGasClouds.Add(gasCloud);
            gasCloud.transform.position = spawnPoint;
            gasCloud.transform.localScale = Vector3.one * randomScaleFactor;
            gasCloud.SetActive(true);
        }

        if (activeBlackHoles.Count < currentBlackHoleCap)
        {
            GameObject blackHole = ObjectPooler.instance.GetPooledBlackHole();
            Vector3 spawnPoint = GetHazardSpawnPoint();
            float randomScaleFactor = Utility.GenerateRandomFloat(minBlackHoleScaleFactor, maxBlackHoleScaleFactor);
            activeBlackHoles.Add(blackHole);
            blackHole.transform.position = spawnPoint;
            blackHole.transform.localScale = Vector3.one * randomScaleFactor;
            blackHole.SetActive(true);
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

    private Vector3 GetHazardSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(
            Random.Range(hazardBounds.center.x - hazardBounds.extents.x, hazardBounds.center.x + hazardBounds.extents.x), 
            Random.Range(hazardBounds.center.y - hazardBounds.extents.y, hazardBounds.center.y + hazardBounds.extents.y), 
            Random.Range(hazardBounds.center.z - hazardBounds.extents.z, hazardBounds.center.z + hazardBounds.extents.z)
            );
        return spawnPoint;
    }

    private Vector3 GetCollectableSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(
            Random.Range(powerupBounds.center.x - powerupBounds.extents.x, powerupBounds.center.x + powerupBounds.extents.x),
            Random.Range(powerupBounds.center.y - powerupBounds.extents.y, powerupBounds.center.y + powerupBounds.extents.y),
            Random.Range(powerupBounds.center.z - powerupBounds.extents.z, powerupBounds.center.z + powerupBounds.extents.z)
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

    private void IncreaseGasCloudCap()
    {
        if (currentGasCloudCap < ObjectPooler.instance.gasCloudCount)
        {
            currentGasCloudCap++;
        }
    }

    private void IncreaseBlackHoleCap()
    {
        if (currentBlackHoleCap < ObjectPooler.instance.blackHoleCount)
        {
            currentBlackHoleCap++;
        }
    }

    private void CleanUp()
    {
        if(activeAsteroids.Count > 0)
        {
            foreach (GameObject asteroid in activeAsteroids.ToArray())
            {
                if (asteroid.transform.position.z < player.transform.position.z - cleanupDistanceFromPlayer)
                {
                    Rigidbody rb = asteroid.GetComponent<Rigidbody>();
                    Asteroid roid = asteroid.GetComponent<Asteroid>();
                    activeAsteroids.Remove(asteroid);
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    roid.ResetHazard();
                    asteroid.SetActive(false);
                }
            }
        }
        

        if(activeGasClouds.Count > 0)
        {
            foreach (GameObject gasCloud in activeGasClouds.ToArray())
            {
                if (gasCloud.transform.position.z < player.transform.position.z - cleanupDistanceFromPlayer)
                {
                    Rigidbody rb = gasCloud.GetComponent<Rigidbody>();
                    // TO DO: Add gas cloud class
                    activeGasClouds.Remove(gasCloud);
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    // TO DO: Add gas cloud class
                    gasCloud.SetActive(false);
                }
            }
        }
        
        if(activeBlackHoles.Count > 0)
        {
            foreach (GameObject blackHole in activeBlackHoles.ToArray())
            {
                if (blackHole.transform.position.z < player.transform.position.z - cleanupDistanceFromPlayer)
                {
                    Rigidbody rb = blackHole.GetComponent<Rigidbody>();
                    // TO DO: Add black hole class
                    activeGasClouds.Remove(blackHole);
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    // TO DO: Add black hole class
                    blackHole.SetActive(false);
                }
            }
        }
        
        if(activeBackgroundAsteroids.Count > 0)
        {
            foreach (GameObject asteroid in activeBackgroundAsteroids.ToArray())
            {
                if (asteroid.transform.position.z < player.transform.position.z - 200f)
                {
                    activeBackgroundAsteroids.Remove(asteroid);
                    asteroid.SetActive(false);
                }
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
        Gizmos.DrawCube(hazardBounds.center, hazardBounds.size);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(powerupBounds.center, powerupBounds.size);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(backgroundAsteroidBounds.center, backgroundAsteroidBounds.size);
    }


}
