using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPointFix : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField] 
    private GameObject[] trackedObjects;

    private void Update()
    {
        if(player.transform.position.z >= 50000)
        {
            //Vector3 newPos = new Vector3(

            foreach(GameObject go in trackedObjects)
            {

            }
        }
    }
    
   private void CheckReset()
    {

    }
}
