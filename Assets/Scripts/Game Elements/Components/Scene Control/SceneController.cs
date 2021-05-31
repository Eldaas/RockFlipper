using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    public bool testingMode;
    public LevelData levelData;
    public LevelModifier levelMods;
    public LevelPowerups levelPowerups;
    public Player player;
    public int resourcesCollected;
    public Canvas hpBarCanvas;
    
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

        if (MissingDataContainers())
        {
            Debug.LogError("SceneController requires assignment of levelData, levelMods and levelPowerups.");
        }

    }

    private void Start()
    {
        EventManager.TriggerEvent("SpaceSceneLoaded");

        if (levelData.levelName == "Asteroid Field")
        {
            EventManager.TriggerEvent("AsteroidFieldSceneLoaded");
            Debug.Log("Asteroid field scene loaded.");
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
