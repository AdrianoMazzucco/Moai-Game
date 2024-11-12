using System;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{
    public BaseState(EState key) {
        StateKey = key;
    }
    public EState StateKey {get; private set; }
    
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();

    public abstract EState GetNextState();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
    public abstract void OnCollisionEnter(Collision other);
    public abstract void OnCollisionStay(Collision other);
    public abstract void OnCollisionExit(Collision other);

}
