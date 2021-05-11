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
    public bool inMotion { get; private set; } = false;

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

    public void SetPoint(int locInd)
    {
        inMotion = true;
        spotInd = locInd;
    }

    IEnumerator ChangeCam()
    {
        int targetSpot = spotInd;
        if (Input.GetKeyDown("right"))
        {
            targetSpot += 1;
            if (targetSpot > 3)
            {
                targetSpot = 0;
            }
            inMotion = true;
            HangerHud.instance.OnScreenButtonPressed(targetSpot);
        }
        else if (Input.GetKeyDown("left"))
        {
            targetSpot -= 1;
            if (targetSpot < 0)
            {
                targetSpot = 3;
            }
            inMotion = true;
            HangerHud.instance.OnScreenButtonPressed(targetSpot);
        }
        if (cam.transform.position != camSpots[spotInd].position || cam.transform.rotation != camSpots[spotInd].rotation)
        {
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, camSpots[spotInd].position, ref vel, 0.3f);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camSpots[spotInd].rotation, Time.deltaTime + .028f);
        }
        if (cam.transform.position == camSpots[spotInd].position || cam.transform.rotation == camSpots[spotInd].rotation)
        {
            if (inMotion == true)
            {
                inMotion = false;
                HangerHud.instance.CanvasGroupManage(targetSpot);
            }
            yield return null;
        }
        Debug.Log("inMotion is " + inMotion);

    }

}
