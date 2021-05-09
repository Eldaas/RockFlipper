using UnityEngine.SceneManagement;

public class HangarState : GameState
{
    public HangarState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadScene("HangarScene");
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
        EventManager.TriggerEvent("HangarSceneLoaded");
        HangarController.instance.ActivateUI();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
