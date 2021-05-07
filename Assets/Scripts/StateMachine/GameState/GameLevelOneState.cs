﻿using UnityEngine.SceneManagement;

public class GameLevelOneState : GameState
{
<<<<<<< Updated upstream:Assets/Scripts/StateMachine/GameState/GameLevelOneState.cs

    public GameLevelOneState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }
=======
    public AsteroidLevelState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }
>>>>>>> Stashed changes:Assets/Scripts/StateMachine/GameState/AsteroidLevelState.cs

    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadScene("PrototypeLevel");
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.instance.levelRecord = new LevelRecord();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    
}