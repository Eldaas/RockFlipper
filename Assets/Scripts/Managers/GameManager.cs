using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public GameStates startingState = GameStates.IntroMenu;

    private bool alreadyLoaded = false;

    private GameStateMachine gameSM;
    private GameIntroMenuState introState;
    private GameLevelOneState levelOneState;

    private void Awake()
    {
        if(!alreadyLoaded)
        {
            instance = this;

            /* Singleton may not be necessary any more with alreadyLoaded check
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
            */

            #region State Machine

            gameSM = new GameStateMachine();
            introState = new GameIntroMenuState(this, gameSM);
            levelOneState = new GameLevelOneState(this, gameSM);

            #endregion

            #region Events

            SceneManager.sceneLoaded += OnSceneLoaded;
            AddEvents();

            #endregion
        }
    }

    private void Start()
    {
        if(!alreadyLoaded)
        {
            #region State Machine

            InitialiseGameState();

            #endregion
        }
    }

    #region State Machine

    /// <summary>
    /// In case the game commences from a scene which is not the intro menu, this method relies on the CheckForManagers script in the starting scene to tell this GameManager script what state it should start with. This method interprets and initialises that state.
    /// </summary>
    private void InitialiseGameState()
    {
        switch(startingState)
        {
            case GameStates.IntroMenu:
                gameSM.Initialise(introState);
                break;
            case GameStates.Hangar:
                //gameSM.Initialise(hangarState);
                break;
            case GameStates.LevelOne:
                gameSM.Initialise(levelOneState);
                break;
            default:
                gameSM.Initialise(introState);
                break;
        }

        alreadyLoaded = true;
    }

    #endregion

    #region Level Loading Methods

    /// <summary>
    /// Game state machine method which can be called from other scripts. Triggers a change of state relative to the scene called.
    /// </summary>
    /// <param name="state">The relevant GameStates enum index pertaining to the level to be loaded.</param>
    public void LoadLevel(GameStates state)
    {
        UIManager.instance.loadScreen.SetActive(true);

        switch (state)
        {
            case GameStates.IntroMenu:
                gameSM.ChangeState(introState);
                break;
            case GameStates.Hangar:
                //gameSM.Initialise(hangarState);
                break;
            case GameStates.LevelOne:
                gameSM.ChangeState(levelOneState);
                break;
            default:
                gameSM.ChangeState(introState);
                break;
        }
    }

    /// <summary>
    /// Performs all common functionality when ANY scene is loaded. Registered in the Awake step on the Game Manager.
    /// For scene-specific functionality, use the OnSceneLoaded function in the associated state class.
    /// </summary>
    public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        UIManager.instance.loadScreen.SetActive(false);
    }

    #endregion

    #region Events
    // Create all game events here. This pushes them to the EventManager dictionary, where other scripts/classes can trigger the events and/or register listeners.
    private void AddEvents()
    {
        // Global level events
        EventManager.AddEvent("AsteroidCollision");
        EventManager.AddEvent("LargeAsteroidExplosion");
        EventManager.AddEvent("MediumAsteroidExplosion");
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
        EventManager.AddEvent("IntroSceneLoaded");
        EventManager.AddEvent("HangarSceneLoaded");
        EventManager.AddEvent("EndLevelSceneLoaded");
        EventManager.AddEvent("AsteroidFieldSceneLoaded");

        // UI Events
        EventManager.AddEvent("UIButtonOptionSelected");
        EventManager.AddEvent("UISuccess");
        EventManager.AddEvent("LoadProfiles");
        EventManager.AddEvent("UpdateProfileSelection");
    }
    #endregion
}

public enum GameStates { None, IntroMenu, Hangar, LevelOne, LevelTwo, LevelThree }