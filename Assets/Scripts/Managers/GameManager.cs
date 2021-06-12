using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Developer Mode")]
    public bool devModeEnabled;

    [Header("Misc")]
    public ProgressionMapping progMap;

    #region Temporary/Runtime Data
    public LevelRecord levelRecord;
    #endregion

    #region Game State Definitions
    [HideInInspector]
    public GameStates startingState = GameStates.IntroMenu;
    private bool alreadyLoaded = false;

    #region State Machine & Level States
    private GameStateMachine gameSM;
    private GameIntroMenuState introState;
    private HangarState hangarState;
    private AsteroidLevelState asteroidLevelState;
    private NebulaLevelState nebulaLevelState;
    private BlackHoleLevelState blackHoleLevelState;
    private DeathState deathState;
    private HighScoresState highScoresState;

    #endregion

    #endregion

    #region Unity Methods
    private void Awake()
    {
        if(!alreadyLoaded)
        {
            instance = this;

            #region Instantiate State Machine & Level States

            gameSM = new GameStateMachine();
            introState = new GameIntroMenuState(this, gameSM);
            hangarState = new HangarState(this, gameSM);
            asteroidLevelState = new AsteroidLevelState(this, gameSM);
            nebulaLevelState = new NebulaLevelState(this, gameSM);
            blackHoleLevelState = new BlackHoleLevelState(this, gameSM);
            deathState = new DeathState(this, gameSM);
            highScoresState = new HighScoresState(this, gameSM);

            #endregion

            #region Create Events

            SceneManager.sceneLoaded += OnSceneLoaded;
            AddEvents();

            #endregion
        }
    }

    private void Start()
    {
        if(!alreadyLoaded)
        {
            #region Initialise State Machine

            InitialiseGameState();

            #endregion
        }
    }
    #endregion

    #region State Machine Methods

    #region Level Loading

    /// <summary>
    /// Game state machine method which can be called from other scripts. Triggers a change of state relative to the scene called.
    /// </summary>
    /// <param name="state">The relevant GameStates enum index pertaining to the level to be loaded.</param>
    public void LoadLevel(GameStates state)
    {
        UIManager.instance.loadScreen.SetActive(true);
        ProfileManager.instance.SaveProfile();
        Debug.Log("Current state: " + gameSM.CurrentState + ", now loading: " + state.ToString());

        switch (state)
        {
            case GameStates.IntroMenu:
                gameSM.ChangeState(introState);
                break;
            case GameStates.Hangar:
                gameSM.ChangeState(hangarState);
                break;
            case GameStates.AsteroidField:
                gameSM.ChangeState(asteroidLevelState);
                break;
            case GameStates.Nebula:
                gameSM.ChangeState(nebulaLevelState);
                break;
            case GameStates.BlackHoles:
                gameSM.ChangeState(blackHoleLevelState);
                break;
            case GameStates.HighScores:
                gameSM.ChangeState(highScoresState);
                break;
            default:
                gameSM.ChangeState(introState);
                break;
        }
    }

    /// <summary>
    /// Game state method which can be called from other scripts. Use this to implement non-scene states (such as player death). For scene states (such as triggering the loading of a scene, use LoadLevel instead).
    /// </summary>
    /// <param name="state">The relevant GameStates enum index pertaining to the state to be loaded.</param>
    public void SetState(GameStates state)
    {
        switch (state)
        {
            case GameStates.DeathState:
                gameSM.ChangeState(deathState);
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
        UIManager.instance.StartCoroutine("DeactivateLoadScreen");
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// In case the game commences from a scene which is not the intro menu, this method relies on the CheckForManagers script in the starting scene to tell this GameManager script what state it should start with. This method interprets and initialises that state.
    /// </summary>
    private void InitialiseGameState()
    {
        switch (startingState)
        {
            case GameStates.IntroMenu:
                gameSM.Initialise(introState);
                break;
            case GameStates.Hangar:
                gameSM.Initialise(hangarState);
                break;
            case GameStates.AsteroidField:
                gameSM.Initialise(asteroidLevelState);
                break;
            case GameStates.Nebula:
                gameSM.Initialise(nebulaLevelState);
                break;
            case GameStates.BlackHoles:
                gameSM.Initialise(asteroidLevelState);
                break;
            case GameStates.HighScores:
                gameSM.Initialise(highScoresState);
                break;
            default:
                gameSM.Initialise(introState);
                break;
        }

        alreadyLoaded = true;
    }
    #endregion

    #endregion

    #region Event Methods
    // Create all game events here. This pushes them to the EventManager dictionary, where other scripts/classes can trigger the events and/or register listeners.
    private void AddEvents()
    {
        // Global level events
        EventManager.AddEvent("Shoot");
        EventManager.AddEvent("AsteroidCollision");
        EventManager.AddEvent("LargeAsteroidExplosion");
        EventManager.AddEvent("MediumAsteroidExplosion");
        EventManager.AddEvent("TakeHit");
        EventManager.AddEvent("ShieldsRecharged");
        EventManager.AddEvent("ShieldsHit"); // TO DO: Add audio cue
        EventManager.AddEvent("ShieldsDestroyed"); // TO DO: Add audio cue
        EventManager.AddEvent("ShieldsOnline"); // TO DO: Add audio cue
        EventManager.AddEvent("ArmourDestroyed"); // TO DO: Add audio cue
        EventManager.AddEvent("ArmourHullHit"); // TO DO: Add audio cue
        EventManager.AddEvent("HealthLow"); // TO DO: Add audio cue
        EventManager.AddEvent("PlayerDeath"); // TO DO: Add audio cue
        EventManager.AddEvent("PowerupCollected"); // TO DO: Add audio cue
        EventManager.AddEvent("ShieldOvercharge"); // TO DO: Add audio cue
        EventManager.AddEvent("ManeuveringBoost"); // TO DO: Add audio cue
        EventManager.AddEvent("HullRepair"); // TO DO: Add audio cue
        EventManager.AddEvent("ArmourRepair"); // TO DO: Add audio cue
        EventManager.AddEvent("SpeedMitigation"); // TO DO: Add audio cue
        EventManager.AddEvent("BatteryRecharge"); // TO DO: Add audio cue
        EventManager.AddEvent("ResourceCollected"); 
        EventManager.AddEvent("ProjectileHit");
        EventManager.AddEvent("ProjectileShot");
        EventManager.AddEvent("SpaceSceneLoaded");
        EventManager.AddEvent("ReturningToBase"); // TO DO: Add audio cue
        EventManager.AddEvent("BatteryIsEmpty"); // TO DO: Add audio cue
        EventManager.AddEvent("StruckLucky"); // TO DO: Add audio cue

        // Scene load events
        EventManager.AddEvent("IntroSceneLoaded");
        EventManager.AddEvent("HangarSceneLoaded");
        EventManager.AddEvent("AsteroidFieldSceneLoaded");
        EventManager.AddEvent("NebulaSceneLoaded");
        EventManager.AddEvent("BlackHoleSceneLoaded");
        EventManager.AddEvent("HighScoresSceneLoaded");

        // UI Events
        EventManager.AddEvent("UIClick");
        EventManager.AddEvent("UISelect");
        EventManager.AddEvent("UIRelease");
        EventManager.AddEvent("UISuccess");
        EventManager.AddEvent("UIError");
        EventManager.AddEvent("UINotification");
        EventManager.AddEvent("UIPause");
        EventManager.AddEvent("UIResume");
        EventManager.AddEvent("LoadProfiles");
        EventManager.AddEvent("ProfileLoaded");

        // Error Modal Events
        EventManager.AddEvent("IncorrectInput");
        EventManager.AddEvent("NoResults");
        EventManager.AddEvent("PauseMenu");
        EventManager.AddEvent("InvalidProfileName");
        EventManager.AddEvent("CantAffordItem");
        EventManager.AddEvent("ReturnedFromDeath");
        EventManager.AddEvent("InactiveOnThisPlatform");

        // Hangar Scene Events
        EventManager.AddEvent("SellResources");
        EventManager.AddEvent("UpdateBalance");
        EventManager.AddEvent("ItemPurchased"); // TO DO: Add audio cue
        EventManager.AddEvent("UpdateInventory");
        EventManager.AddEvent("UpdateStats");
        EventManager.AddEvent("UpdateEquipmentSlots");
        EventManager.AddEvent("UpdateModulePrices");
        EventManager.AddEvent("ItemEquipped"); // TO DO: Add audio cue & tie in logic
        EventManager.AddEvent("ItemUnequipped");
        EventManager.AddEvent("ItemDestroyed"); // TO DO: Add audio cue
    }
    #endregion

    #region Miscellaneous Methods
    public float CalcDeathCost()
    {
        float baseCost = 1000f;
        int numOfDeaths = Mathf.Clamp(ProfileManager.instance.currentProfile.numOfDeaths, 0, progMap.deathIndexRate);
        float deathScale = progMap.deathCostScale.Evaluate(numOfDeaths);
        return (1 + deathScale) * baseCost;
    }
    #endregion
}

public enum GameStates { None, IntroMenu, Hangar, AsteroidField, Nebula, BlackHoles, DeathState, HighScores }
