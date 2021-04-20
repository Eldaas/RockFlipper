using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerCam : MonoBehaviour
{
    private GameObject cam;
    public List<Transform> camSpots = new List<Transform>();
    [SerializeField]
    private int spotInd = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("right"))
        {
            spotInd += 1;
            if (spotInd > 3)
            {
                spotInd = 0;
            }
            cam.transform.position = camSpots[spotInd].position;
            cam.transform.rotation = camSpots[spotInd].rotation;
        }
        else if (Input.GetKeyDown("left"))
        {
            spotInd -= 1;
            if (spotInd < 0)
            {
                spotInd = 3;
            }
            cam.transform.position = camSpots[spotInd].position;
            cam.transform.rotation = camSpots[spotInd].rotation;
        }
    }


}
