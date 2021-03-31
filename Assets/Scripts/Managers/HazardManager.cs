using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    public static HazardManager instance;

    public AsteroidData asteroidData;

    private void Awake()
    {
        #region Singleton
        HazardManager[] list = FindObjectsOfType<HazardManager>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the Game Manager component detected. Destroying an instance.");
        }
        else
        {
            instance = this;
        }
        #endregion
    }
}
