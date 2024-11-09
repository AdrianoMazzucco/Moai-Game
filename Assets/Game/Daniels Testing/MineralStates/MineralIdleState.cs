using UnityEngine;

public class MineralIdleState : MineralBaseState
{
    public override void EnterState(MineralStateManager mineral)
    {

    }

    public override void UpdateState(MineralStateManager mineral)
    {
        

        
        

        //exit state
        mineral.SwitchState(mineral.MineralActivePhysicsState);
    }

    public override void OnCollisionEnter(MineralStateManager mineral, Collider collider)
    {

    }
}
