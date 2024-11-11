using UnityEngine;

public abstract class IntermediateBaseState : BaseState<GenericStateMachine.EStateGeneric>
{
    protected GenericStateMachineContext Context;

    public IntermediateBaseState (GenericStateMachineContext context, GenericStateMachine.EStateGeneric 
        stateKey) : base(stateKey)
    {
        Context = context;
    }
}

