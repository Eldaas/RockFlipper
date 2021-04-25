﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public bool bypassIntroMenu = false;

    private GameStateMachine gameSM;
    private GameLoadState loadState;
    private GameIntroMenuState introState;
    private GameLevelOneState levelOneState;

    private void Awake()
    {

        #region Singleton
        GameManager[] list = FindObjectsOfType<GameManager>();
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

        #region State Machine

        gameSM = new GameStateMachine();
        loadState = new GameLoadState(this, gameSM);
        introState = new GameIntroMenuState(this, gameSM);
        levelOneState = new GameLevelOneState(this, gameSM);

        #endregion

        #region Events

        SceneManager.sceneLoaded += OnSceneLoaded;
        AddEvents();

        #endregion

    }

    private void Start()
    {
        #region State Machine

        if (bypassIntroMenu)
        {
            gameSM.Initialize(introState);
        }
        else
        {
            gameSM.Initialize(levelOneState);
        }

        #endregion
    }

    #region Level Loading Methods

    /// <summary>
    /// Game state machine interface which can be called from other scripts. Triggers a change of state relative to the scene called.
    /// </summary>
    /// <param name="buildID">The build ID of the scene as displayed in Unity build settings window.</param>
    public void LoadLevel(int buildID)
    {
        UIManager.instance.loadScreen.SetActive(true);

        if (buildID == 1)
        {
            gameSM.ChangeState(introState);
        }
        else if (buildID == 2)
        {
            gameSM.ChangeState(levelOneState);
        }
    }

    /// <summary>
    /// Performs all common functionality when ANY scene is loaded. Registered in the Awake step on the Game Manager.
    /// For scene-specific functionality, use the OnSceneLoaded function in the associated state class.
    /// </summary>
    public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        //UIManager.instance.loadScreen.SetActive(false);
    }

    #endregion

    #region Events
    // Create all game events here. This pushes them to the EventManager dictionary, where other scripts/classes can trigger the events and/or register listeners.
    private void AddEvents()
    {
        // Global level events
        EventManager.AddEvent("AsteroidCollision");
        EventManager.AddEvent("AsteroidExplosion");
        EventManager.AddEvent("TakeHit");
        EventManager.AddEvent("ShieldsRecharged");
        EventManager.AddEvent("ShieldsHit");
        EventManager.AddEvent("ShieldsDestroyed");
        EventManager.AddEvent("ShieldsOnline");
        EventManager.AddEvent("ArmourHit");
        EventManager.AddEvent("ArmourDestroyed");
        EventManager.AddEvent("HullHit");
        EventManager.AddEvent("HealthLow");
        EventManager.AddEvent("PlayerDeath");
        EventManager.AddEvent("PowerupCollected");
        EventManager.AddEvent("ShieldOvercharge");
        EventManager.AddEvent("ManeuveringBoost");
        EventManager.AddEvent("HullRepair");
        EventManager.AddEvent("ArmourRepair");
        EventManager.AddEvent("SpeedMitigation");
        EventManager.AddEvent("ResourceCollected");
        EventManager.AddEvent("ProjectileHit");
        EventManager.AddEvent("ProjectileShot");
        EventManager.AddEvent("SpaceSceneLoaded");

        // Scene-specific events
        EventManager.AddEvent("IntroMenuSceneLoaded");
        EventManager.AddEvent("HangarSceneLoaded");
        EventManager.AddEvent("EndLevelSceneLoaded");
        EventManager.AddEvent("AsteroidFieldSceneLoaded");
    }
    #endregion
}
