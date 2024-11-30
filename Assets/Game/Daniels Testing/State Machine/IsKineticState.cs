using UnityEngine;

public class IsKineticState : MineralBaseState
{
    public AnimationClip mineralIdle;
    public Animation anim;

    public bool isGrounded;
    public bool isSucked;

    public IsKineticState(MineralStateMachineContext context, MineralStateMachine.EStateMineral
        estate) : base(context, estate)
    {
        MineralStateMachineContext Context = context;
    }

    public override void EnterState()
    {
        isGrounded = true;
        isSucked = Context.IsSucked;

        //Debug.Log("Made it to Kinematic State");
        anim = Context.Animation;
        
        mineralIdle = Context.MineralIdle;
        //Debug.Log(Context.MineralIdle);

        anim.clip = mineralIdle;
       // Debug.Log(mineralIdle);

        anim.Play();

    }
    public override void ExitState() { }
    public override void UpdateState()
    {
        if (Input.GetMouseButtonDown(1)) // 0 is for the left mouse button
        {
            isSucked = true;
            Debug.Log("isSucked is now: " + isSucked);
        }
        else if (Input.GetMouseButtonUp(1)) 
        {
            isSucked = false;
            Debug.Log("isSucked is now: " + isSucked);
        }
        
    }
    public override void FixedUpdateState() { }
    public override MineralStateMachine.EStateMineral GetNextState() {
        
        if (isSucked == true || isGrounded == false)
        {
            Debug.Log("we gonna switch to physics again");
            anim.Stop();
            Context.Rigidbody.isKinematic = false;

            return MineralStateMachine.EStateMineral.ActivePhysics;
        }
        
        return StateKey;

        
    }
    public override void OnTriggerEnter(Collider other) 
    {
        /*if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }*/
    }
    public override void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    public override void OnTriggerExit(Collider other) 
    {
        /*if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }*/
    }
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
