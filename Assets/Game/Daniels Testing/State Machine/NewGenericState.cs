using UnityEngine;

public class NewGenericState : IntermediateBaseState
{
    public NewGenericState(GenericStateMachineContext context, GenericStateMachine.EStateGeneric
        estate) : base(context, estate)
    {
        GenericStateMachineContext Context = context;
    }
    public override void EnterState() { }
    public override void ExitState() { }
    public override void UpdateState() { }
    public override void FixedUpdateState() { }
    public override GenericStateMachine.EStateGeneric GetNextState() { return StateKey; }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
    public override void OnCollisionEnter(Collision other) { }
    public override void OnCollisionStay(Collision other) { }
    public override void OnCollisionExit(Collision other) { }

}

/*
 INSTRUCTIONS:

Use NewGenericState as a template for new states.
Update Generic State Machine context to new state machine context.
Update GenericStateMachine to new state machine name.
Update EStateNameHere to 

*/
