using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject loadScreen;


    private void Awake()
    {
        #region Singleton
        UIManager[] list = FindObjectsOfType<UIManager>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the UIManager component detected. Destroying an instance.");
        }
        else
        {
            instance = this;
        }
        #endregion
    }



}
