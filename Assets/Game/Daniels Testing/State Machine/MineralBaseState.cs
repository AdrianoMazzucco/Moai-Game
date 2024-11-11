using UnityEngine;

public abstract class MineralBaseState : BaseState<MineralStateMachine.EStateMineral>
{
    protected MineralStateMachineContext Context;

    public MineralBaseState(MineralStateMachineContext context, MineralStateMachine.EStateMineral 
        stateKey) : base(stateKey)
    {
        Context = context;
    }
}

