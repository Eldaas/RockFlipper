using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDemo : MonoBehaviour
{
    [SerializeField]
    private float force;
    [SerializeField]
    private float radius;
    [SerializeField]
    private GameObject asteroidsParent;
    [SerializeField]
    private List<Transform> asteroids;


    private void Awake()
    {
        Transform[] asteroidsArray = asteroidsParent.GetComponentsInChildren<Transform>();
        asteroids = new List<Transform>();
        foreach(Transform child in asteroidsArray)
        {
            if(child.CompareTag("Asteroid"))
            {
                asteroids.Add(child);
            }
        }
    }

    private void Start()
    {
        Invoke("ExplodeAsteroids", 2f);
    }

    public void ExplodeAsteroids()
    {
        foreach (Transform asteroid in asteroids)
        {
            asteroid.GetComponent<Asteroid>().ExplodeAsteroid(force, radius);
        }
    }
}
