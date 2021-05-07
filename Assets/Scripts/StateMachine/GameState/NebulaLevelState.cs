using UnityEngine.SceneManagement;

public class NebulaLevelState : GameState
{

    public NebulaLevelState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadScene("NebulaLevel");
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
