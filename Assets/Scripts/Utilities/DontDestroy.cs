using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        if (objects.Length > 1)
        {
            foreach(GameObject go in objects)
            {
                if(go != this)
                {
                    Destroy(gameObject);
                }
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}
