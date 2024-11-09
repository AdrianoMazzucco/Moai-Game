using UnityEngine;

public abstract class MineralBaseState
{

    public abstract void EnterState(MineralStateManager mineral);

    public abstract void UpdateState(MineralStateManager mineral);
    
    public abstract void OnCollisionEnter(MineralStateManager mineral, Collider collider); 


}
