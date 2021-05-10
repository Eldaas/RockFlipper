using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HangerHud : MonoBehaviour
{
    public static HangerHud instance;

    public List<CanvasGroup> canvasGroups;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnScreenButtonPressed(int buttonPressed)
    {
        Camera.main.GetComponent<HangerCam>().SetPoint(buttonPressed);
        CanvasGroupManage(buttonPressed);
    }

    public void CanvasGroupManage(int spotId)
    {
        if (spotId == 0)
        {
            if (Camera.main.GetComponent<HangerCam>().inMotion == false)
            {
                canvasGroups[0].alpha = 1;
                canvasGroups[0].interactable = true;
                canvasGroups[0].blocksRaycasts = true;
            }
            canvasGroups[1].alpha = 0;
            canvasGroups[1].interactable = false;
            canvasGroups[1].blocksRaycasts = false;
            canvasGroups[2].alpha = 0;
            canvasGroups[2].interactable = false;
            canvasGroups[2].blocksRaycasts = false;
            canvasGroups[3].alpha = 0;
            canvasGroups[3].interactable = false;
            canvasGroups[3].blocksRaycasts = false;
        }
        else if (spotId == 1)
        {
            canvasGroups[0].alpha = 0;
            canvasGroups[0].interactable = false;
            canvasGroups[0].blocksRaycasts = false;
            if (Camera.main.GetComponent<HangerCam>().inMotion == false)
            {
                canvasGroups[1].alpha = 1;
                canvasGroups[1].interactable = true;
                canvasGroups[1].blocksRaycasts = true;
            }
            canvasGroups[2].alpha = 0;
            canvasGroups[2].interactable = false;
            canvasGroups[2].blocksRaycasts = false;
            canvasGroups[3].alpha = 0;
            canvasGroups[3].interactable = false;
            canvasGroups[3].blocksRaycasts = false;
        }
        else if (spotId == 2)
        {
            canvasGroups[0].alpha = 0;
            canvasGroups[0].interactable = false;
            canvasGroups[0].blocksRaycasts = false;
            canvasGroups[1].alpha = 0;
            canvasGroups[1].interactable = false;
            canvasGroups[1].blocksRaycasts = false;
            if (Camera.main.GetComponent<HangerCam>().inMotion == false)
            {
                canvasGroups[2].alpha = 1;
                canvasGroups[2].interactable = true;
                canvasGroups[2].blocksRaycasts = true;
            }
            canvasGroups[3].alpha = 0;
            canvasGroups[3].interactable = false;
            canvasGroups[3].blocksRaycasts = false;
        }
        else if (spotId == 3)
        {
            canvasGroups[0].alpha = 0;
            canvasGroups[0].interactable = false;
            canvasGroups[0].blocksRaycasts = false;
            canvasGroups[1].alpha = 0;
            canvasGroups[1].interactable = false;
            canvasGroups[1].blocksRaycasts = false;
            canvasGroups[2].alpha = 0;
            canvasGroups[2].interactable = false;
            canvasGroups[2].blocksRaycasts = false;
            if (Camera.main.GetComponent<HangerCam>().inMotion == false)
            {
                canvasGroups[3].alpha = 1;
                canvasGroups[3].interactable = true;
                canvasGroups[3].blocksRaycasts = true;
            }
        }
    }
}
