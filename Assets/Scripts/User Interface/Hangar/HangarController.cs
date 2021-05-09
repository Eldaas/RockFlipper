using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarController : MonoBehaviour
{
    public static HangarController instance;

    public bool testingMode = false;
    public GameObject endLevelScreen;
    public GameObject hangarScreen;
    

    private void Awake()
    {
        #region Singleton
        HangarController[] list = FindObjectsOfType<HangarController>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the HangarController component detected. Destroying an instance.");
        }
        else
        {
            instance = this;
        }
        #endregion
    }

    private void Start()
    {
        if (testingMode)
        {
            Debug.Log("Testing mode is active.");
            GameManager.instance.levelRecord = new LevelRecord();
            GameManager.instance.levelRecord.ironCollected = 100;
            GameManager.instance.levelRecord.silverCollected = 100;
            GameManager.instance.levelRecord.goldCollected = 100;

        }

        RegisterListeners();
        ActivateUI();
    }

    public void ActivateUI()
    {
        if(GameManager.instance.levelRecord != null)
        {
            hangarScreen.SetActive(false);
            endLevelScreen.SetActive(true);
        }
        else
        {
            hangarScreen.SetActive(true);
        }
    }

    #region Private Methods
    private void RegisterListeners()
    {

    }
    #endregion

    private void TestingMode()
    {

    }
}
