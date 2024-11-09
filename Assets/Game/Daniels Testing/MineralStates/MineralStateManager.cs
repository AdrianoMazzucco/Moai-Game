using UnityEngine;

public class MineralStateManager
{
    MineralBaseState currentState;
    public MineralActivePhysicsState MineralActivePhysicsState = new MineralActivePhysicsState();
    public MineralIdleState MineralIdleState = new MineralIdleState();

    public void Start()
    {
        //sets the first state
        currentState = MineralActivePhysicsState;
        //starts the first state
        currentState.EnterState(this);
    }

    public void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(MineralBaseState state)
    {
        //sets the state to switch to
        currentState = state;
        //switches to that state
        state.EnterState(this);
    }

}
