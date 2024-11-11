using UnityEngine;

public class IsKineticState : MineralBaseState 
{
    public AnimationClip mineralIdle;
    public Animation anim;
    
    public IsKineticState(MineralStateMachineContext context, MineralStateMachine.EStateMineral 
        estate) : base (context, estate)
    {
        MineralStateMachineContext Context = context;
    }
    
    public override void EnterState() 
    {
        Debug.Log("Made it to Kinematic State");
        mineralIdle = Context.MineralIdle;
        anim.clip = mineralIdle;
    
    }
    public override void ExitState() { }
    public override void UpdateState() 
    {
        anim.Play();
    }
    public override void FixedUpdateState() { }
    public override MineralStateMachine.EStateMineral GetNextState() {
        if (Context.IsSucked)
        {
            Context.Rigidbody.isKinematic = true;
            return MineralStateMachine.EStateMineral.IsKinetic;
        }
        return StateKey; 
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }

}

/*
 INSTRUCTIONS:

Use NewGenericState as a template for new states.
Update Generic State Machine context to new state machine context.
Update GenericStateMachine to new state machine name.
Update EStateNameHere to 

*/
