using System.Collections;
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

    public ScoreRecord record;

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
    // Add all of your events in here so we've got a good idea of what events exist in the game
    private void AddEvents()
    {
        EventManager.AddEvent("asteroidCollision");
        EventManager.AddEvent("takeHit");
        EventManager.AddEvent("shieldsRecharged");
        EventManager.AddEvent("shieldsHit");
        EventManager.AddEvent("shieldsDestroyed");
        EventManager.AddEvent("shieldsOnline");
        EventManager.AddEvent("armourHit");
        EventManager.AddEvent("armourDestroyed");
        EventManager.AddEvent("hullHit");
        EventManager.AddEvent("healthLow");
        EventManager.AddEvent("playerDeath");
        EventManager.AddEvent("powerupCollected");
        EventManager.AddEvent("shieldOvercharge");
    }
    #endregion
}
