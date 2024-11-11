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
    public float rotationSpeed = 1.0f;

    private bool speedRegistered = false;
    private bool upright = false;

    public ActivePhysicsState(MineralStateMachineContext context, MineralStateMachine.EStateMineral 
        estate) : base (context, estate)
    {
        MineralStateMachineContext Context = context;
    }
    public override void EnterState() 
    {
        startDrag = Context.Rigidbody.linearDamping;

        // Define the target rotation to be upright, preserving only the Y rotation
        upwardRotation = Quaternion.Euler(0, currentRotation.y, 0);
    }
    public override void ExitState() { }
    public override void UpdateState() 
    {
        // Register the initial velocity when it is first applied (non-zero velocity)
        if (!speedRegistered && Context.Rigidbody.linearVelocity.magnitude > 0)
        {
            speedRegistered = true; // Mark that the velocity has been registered
        }
        else
        {
            //calculates current velocity, sets new previous velocity
            currentSpeed = ((Context.Transform.position - previous).magnitude) / Time.deltaTime;
            previous = Context.Transform.position;
        }

        //if we are slower than the slow down speed, but faster than the stop speed, adds increase drag
        if (speedRegistered && currentSpeed <= Context.SlowDownSpeed && currentSpeed >= Context.StopSpeed)
        {
            if (Context.StartDrag != Context.Rigidbody.linearDamping)
            {
                //sets higher drag on mineral when slowing down
                Context.Rigidbody.linearDamping += Context.DragIncrement * Time.deltaTime;
            }
          
        }

       

        
    }
    public override void FixedUpdateState() 
    {
        if (speedRegistered && currentSpeed <= Context.StopSpeed)
        {

            // Calculate the target rotation to make the GameObject's Y-axis point upward
            upwardRotation = Quaternion.FromToRotation(Context.Transform.up, Vector3.up) * Context.Transform.rotation;

            // Smoothly interpolate between current rotation and target rotation
            Quaternion newRotation = Quaternion.Slerp(Context.Rigidbody.rotation, upwardRotation, rotationSpeed * Time.deltaTime);

            // Apply the rotation to the Rigidbody using MoveRotation
            Context.Rigidbody.MoveRotation(newRotation);

            if (Vector3.Dot(currentRotation, Vector3.up) > 0.99f)
            {
                upright = true;
            }
        }
    }
    public override MineralStateMachine.EStateMineral GetNextState() 
    {
        if (upright)
        {
            speedRegistered = false;
            Context.Rigidbody.isKinematic = true;
            Context.Rigidbody.linearDamping = startDrag;
            upright = false;


            return MineralStateMachine.EStateMineral.IsKinetic;
        }        
        return StateKey; 
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) 
    {
        //if player uses suck action, set IsSucked = true
    }
    public override void OnTriggerExit(Collider other) { }

}

/*
 INSTRUCTIONS:

Use NewGenericState as a template for new states.
Update Generic State Machine context to new state machine context.
Update GenericStateMachine to new state machine name.
Update EStateNameHere to EStateNeMachineName

*/
