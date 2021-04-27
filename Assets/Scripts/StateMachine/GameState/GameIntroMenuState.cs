using UnityEngine;
using UnityEngine.SceneManagement;

public class GameIntroMenuState : GameState
{
    public GameIntroMenuState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entering intro menu scene");
        EventManager.TriggerEvent("IntroSceneLoaded");

        if (SceneManager.GetActiveScene().name != "IntroScene")
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("IntroScene");   
        }
        
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
