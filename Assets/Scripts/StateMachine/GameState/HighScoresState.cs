using UnityEngine.SceneManagement;

public class HighScoresState : GameState
{
    public HighScoresState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadScene("HighScoresScene");
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
        EventManager.TriggerEvent("HighScoresSceneLoaded");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
