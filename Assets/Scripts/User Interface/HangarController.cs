using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarController : MonoBehaviour
{
    public static HangarController instance;

    public GameObject endLevelScreen;
    public GameObject hangarScreen;
    public Tallying tallying;
    public bool isTallying = false;

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
        RegisterListeners();
    }

    public void ActivateUI()
    {
        if(GameManager.instance.levelRecord != null)
        {
            isTallying = true;
            hangarScreen.SetActive(false);
            endLevelScreen.SetActive(true);
            tallying.StartSequence();
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
}
