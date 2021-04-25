using UnityEngine.SceneManagement;

public class GameLevelOneState : GameState
{

    public GameLevelOneState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        //SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.LoadScene(2);
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
        
    }

    
}
