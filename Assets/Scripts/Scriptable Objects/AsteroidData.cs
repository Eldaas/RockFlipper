using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hazard Data/Asteroid Data")]
public class AsteroidData : ScriptableObject
{
    [Header("Inspector References")]
    public Material barrenAsteroidMaterial;
    public Material ironRockMaterial;
    public Material silverRockMaterial;
    public Material goldRockMaterial;
    public float collisionSensitivity;
    public float collisionMaxForce;
    

    /// <summary>
    /// Base health is to be set for all asteroids, and then this is multiplied by the scale the asteroid has at runtime.
    /// </summary>
    public float baseHealth;
}
