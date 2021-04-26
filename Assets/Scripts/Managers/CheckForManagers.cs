using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForManagers : MonoBehaviour
{
    public GameObject managersPrefab;
    public GameStates startingState;

    private void Awake()
    {
        // Looks for game objects with the Managers tag
        GameObject[] children = GameObject.FindGameObjectsWithTag("Managers");
        bool managersFound = false;

        // If managers exist in the scene, nothing else needs to happen
        if(children.Length > 0)
        {
            managersFound = true;
        }

        // If managers are not found, they need to be instantiated and the new GameManager needs to execute the state relevant to this particular scene.
        if(!managersFound)
        {
            Debug.Log("Managers not found. Creating new Persistent Managers object.");
            GameObject managers = Instantiate(managersPrefab);

            Transform[] newManagers = managers.GetComponentsInChildren<Transform>();

            for (int i = 0; i < newManagers.Length; i++)
            {
                if(newManagers[i].CompareTag("GameController"))
                {
                    GameManager thisManager = newManagers[i].GetComponent<GameManager>();
                    thisManager.startingState = startingState;
                }
            }
        }
    }
}
