using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    protected GameManager manager;
    protected GameStateMachine stateMachine;

    protected GameState(GameManager manager, GameStateMachine stateMachine)
    {
        this.manager = manager;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
