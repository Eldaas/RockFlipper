using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [HideInInspector]
    public static SceneController instance;
    public LevelData data;
    public LevelModifier levelMods;

    private void Awake()
    {
        #region Singleton
        SceneController[] list = FindObjectsOfType<SceneController>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the Game Manager component detected. Destroying an instance.");
        }
        else
        {
            instance = this;
        }
        #endregion
    }
}
