using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject loadScreen;

    private void Awake()
    {
        #region Singleton
        AudioManager[] list = FindObjectsOfType<AudioManager>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the Audio Manager component detected. Destroying an instance.");
        }
        else
        {
            instance = this;
        }
        #endregion
    }
}
