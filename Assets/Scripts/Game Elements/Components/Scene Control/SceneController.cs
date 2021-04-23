using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [HideInInspector]
    public static SceneController instance;
    public LevelData levelData;
    public LevelModifier levelMods;
    public LevelPowerups levelPowerups;
    public Player player;

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

        if(MissingDataContainers())
        {
            Debug.LogError("SceneController requires assignment of levelData, levelMods and levelPowerups.");
        }

    }

    #region Private Methods
    private bool MissingDataContainers()
    {
        if(levelData == null || levelMods == null || levelPowerups == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
