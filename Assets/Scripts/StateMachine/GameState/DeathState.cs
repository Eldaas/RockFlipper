using UnityEngine;

public class DeathState : GameState
{
    public DeathState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        UIManager.instance.errorModalOkButton.onClick.RemoveAllListeners();
        UIManager.instance.errorModalOkButton.onClick.AddListener(EmbracesDeath);
        UIManager.instance.errorModalCloseButton.onClick.AddListener(EmbracesDeath);
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

    private void EmbracesDeath()
    {
        GameManager.instance.levelRecord = null;
        UIManager.instance.errorModal.SetActive(false);
        GameManager.instance.LoadLevel(GameStates.Hangar);
    }
}
