public class HangarState : GameState
{
    public HangarState(GameManager manager, GameStateMachine stateMachine) : base(manager, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        HangarController.instance.ActivateUI();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!HangarController.instance.isTallying)
        {
            // Put all state execution in here
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
