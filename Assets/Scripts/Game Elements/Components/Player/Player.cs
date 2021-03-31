using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerHealth health;
    public PlayerStats stats;
    public PlayerEquipment equipment;

    private void Awake()
    {
        CheckForMissingDataContainers();
    }


    #region Public Methods



    #endregion

    #region Private Methods
    private void CheckForMissingDataContainers()
    {
        if (!health || !stats || !equipment)
        {
            Debug.LogError("Missing a critical component on the Player object!");
        }
    }
    #endregion

    #region Unity Methods
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    #endregion
}
