using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForManagers : MonoBehaviour
{
    public GameObject managersPrefab;
    public GameStates startingState;

    private void Awake()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        bool managersFound = false;

        for (int i = 0; i < children.Length; i++)
        {
            if(children[i].CompareTag("Managers"))
            {
                managersFound = true;
                break;
            }
        }

        if(!managersFound)
        {
            GameObject managers = Instantiate(managersPrefab, transform);

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
