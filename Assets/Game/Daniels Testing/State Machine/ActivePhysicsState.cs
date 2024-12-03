using UnityEngine;
using static MineralStateMachine;

public class ActivePhysicsState : MineralBaseState 
{

    public float currentSpeed;
    public float currentVelocity;
    public Vector3 previous;

    public float startDrag;

    public Vector3 currentRotation;
    public Quaternion upwardRotation;
    public float rotationSpeed = 10.0f;
    private float timeToTransition;

    private bool transitionTimerActive = false;
    private bool isKineticTransitionCooldown = false;
    private bool speedRegistered = false;
    private bool isUpright = false;
    private bool isGrounded;
    private bool isSucked;


    public ActivePhysicsState(MineralStateMachineContext context, MineralStateMachine.EStateMineral 
        estate) : base (context, estate)
    {
        MineralStateMachineContext Context = context;
    }
    public override void EnterState() 
    {
        startDrag = Context.Rigidbody.linearDamping;

        timeToTransition = Context.TimeToTransition;
        transitionTimerActive = true;

        isGrounded = Context.IsGrounded;
    }
    public override void ExitState() { }
    public override void UpdateState() 
    {
        
        
        // Register the initial velocity when it is first applied (non-zero velocity)
        if (!speedRegistered && Context.Rigidbody.linearVelocity.magnitude > 0)
        {
            speedRegistered = true; // Mark that the velocity has been registered
            //Debug.Log("Speed Was Registered");

            
        }

        if (timeToTransition <= 0)
        {
            timeToTransition = 0;
            transitionTimerActive = false;
            isKineticTransitionCooldown = true;
            //Debug.Log("Timer Done");
        }

    }
    public override void FixedUpdateState() 
    {

        
            //Debug.Log("accessed timer");
            //calculates current velocity, sets new previous velocity
            currentSpeed = ((Context.Transform.position - previous).magnitude) / Time.deltaTime;
            previous = Context.Transform.position;

            if (transitionTimerActive && timeToTransition > 0)
            {
                //Debug.Log(timeToTransition);
                timeToTransition -= Time.fixedDeltaTime;

            }
                
        
        //if we are slower than the slow down speed, but faster than the stop speed, adds increase drag
        if ( currentSpeed <= Context.SlowDownSpeed && currentSpeed >= Context.StopSpeed)
        {
            if (Context.StartDrag != Context.Rigidbody.linearDamping)
            {
                //sets higher drag on mineral when slowing down
                Context.Rigidbody.linearDamping += Context.DragIncrement * Time.deltaTime;
                //Debug.Log("We enter slowing down");
            }

        }
       

        if (currentSpeed <= Context.StopSpeed)
        {

            // Calculate the target rotation to make the GameObject's Y-axis point upward
            upwardRotation = Quaternion.FromToRotation(Context.Transform.up, Vector3.up) * Context.Transform.rotation;

            // Smoothly interpolate between current rotation and target rotation
            Quaternion newRotation = Quaternion.Slerp(Context.Rigidbody.rotation, upwardRotation, rotationSpeed * Time.deltaTime);

            // Apply the rotation to the Rigidbody using MoveRotation
            Context.Rigidbody.MoveRotation(newRotation);


            if (Vector3.Dot(Context.Transform.up, Vector3.up) > 0.99f)
            {
                isUpright = true;
                //Debug.Log("We are upright");
            }
            else
            {
                isUpright = false;
            }
        }
    }
    public override MineralStateMachine.EStateMineral GetNextState() 
    {
        Debug.Log("isKineticTransitionCooldown is "+ isKineticTransitionCooldown);
        Debug.Log("isUpright is "+ isUpright);
        Debug.Log("isGrounded is "+ isGrounded);
        //Debug.Log(isSucked);

        //if (isKineticTransitionCooldown && isUpright && isGrounded && !isSucked)
        if (isKineticTransitionCooldown && isUpright && isGrounded)
        {
            speedRegistered = false;
            Context.Rigidbody.isKinematic = true;
            Context.Rigidbody.linearDamping = startDrag;
            isUpright = false;
            isKineticTransitionCooldown = false;

            //Debug.Log("We transitioning to next state");
            return MineralStateMachine.EStateMineral.IsKinetic;
        }        
        return StateKey; 
    }
    public override void OnTriggerEnter(Collider other) 
    {
        /*
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            isGrounded = true;
            //Debug.Log("isGrounded " + isGrounded);
        }
        */
        if (other.gameObject.layer == LayerMask.NameToLayer("Suck"))
        {
            isSucked = true;
            timeToTransition = Context.TimeToTransition;
            //Debug.Log("isSucked is " + isSucked);
        }

    }
    public override void OnTriggerStay(Collider other) 
    {
        /*
        if (other.gameObject.layer == LayerMask.NameToLayer("Suck"))
        {
            isSucked = true;
            Debug.Log("isSucked is " + isSucked);
        }
        else 
        {
            isSucked = false;
            Debug.Log("isSucked is " + isSucked);
        }
        */

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Lava"))
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
        /*
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            isGrounded = false;
        }
        */
        if (other.gameObject.layer == LayerMask.NameToLayer("Suck"))
        {
            isSucked = false;
        }

    }
    public override void OnCollisionEnter(Collision other) { }
    public override void OnCollisionStay(Collision other) { }
    public override void OnCollisionExit(Collision other) { }

    public void TransitionStateCooldownTimer()
    {
        if (transitionTimerActive && timeToTransition > 0)
        {
            timeToTransition -= Time.deltaTime;
            if (timeToTransition <= 0)
            {
                timeToTransition = 0;
                transitionTimerActive = false;
                isKineticTransitionCooldown = true;
               //Debug.Log(timeToTransition);
            }
        }
    }
}

/*
 INSTRUCTIONS:

Use NewGenericState as a template for new states.
Update Generic State Machine context to new state machine context.
Update GenericStateMachine to new state machine name.
Update EStateNameHere to EStateNeMachineName

*/
