using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangerCam : MonoBehaviour
{
    private GameObject cam;
    public List<Transform> camSpots = new List<Transform>();
    [SerializeField]
    private int spotInd = 0;
    private Vector3 vel = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        cam = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ChangeCam());
    }

    IEnumerator ChangeCam()
    {
        if (Input.GetKeyDown("right"))
        {
            spotInd += 1;
            if (spotInd > 3)
            {
                spotInd = 0;
            }
            
        }
        else if (Input.GetKeyDown("left"))
        {
            spotInd -= 1;
            if (spotInd < 0)
            {
                spotInd = 3;
            }
        }
        if (cam.transform.position != camSpots[spotInd].position || cam.transform.rotation != camSpots[spotInd].rotation)
        {
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, camSpots[spotInd].position, ref vel, 0.3f);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camSpots[spotInd].rotation, Time.deltaTime + .028f);
        }
        else
        {
            yield return null;
        }
    }

}
