using UnityEngine.SceneManagement;

public class AsteroidLevelState : GameState
{

    public AsteroidLevelState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadScene("PrototypeLevel");
        SceneManager.sceneLoaded += OnSceneLoaded;
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
