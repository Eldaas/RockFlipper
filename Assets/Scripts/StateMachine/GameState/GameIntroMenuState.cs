using UnityEngine.SceneManagement;

public class GameIntroMenuState : GameState
{
    public GameIntroMenuState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(1);
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
