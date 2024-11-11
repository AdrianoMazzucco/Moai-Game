using UnityEngine;
using UnityEngine.Assertions;

public class MineralStateMachine : BaseStateMachine <MineralStateMachine. EStateMineral>
    //replace State machine to your desired state name 
{
    public enum EStateMineral
    {
        ActivePhysics,
        IsKinetic,
        
    }
    //*change below name to state machine context name
    private MineralStateMachineContext _context;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _rootCollider;
    [SerializeField] private Transform _transform;
    [SerializeField] private float _slowDownSpeed;
    [SerializeField] private float _stopSpeed;
    [SerializeField] private float _startDrag;
    [SerializeField] private float _dragIncrement;
    [SerializeField] private bool _isSucked;
    [SerializeField] private bool _upright;
    [SerializeField] private AnimationClip _mineralIdle;
    //[SerializeField] private AddMoreComponentsHere _addMoreComponentsHere;

    void Awake()
    {
        ValidateConstraints();
        _context = new MineralStateMachineContext(_rigidbody, _rootCollider, _transform, 
            _slowDownSpeed, _stopSpeed, _startDrag, _dragIncrement, _isSucked, _upright, _mineralIdle);
        //*Pass all private member variables above through _context

        InitializeStates();
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_rigidbody, "Rigidbody is not assigned.");
        Assert.IsNotNull(_rootCollider, "Root Collider is not null.");
        Assert.IsNotNull(_transform, "Transform is not null.");
        Assert.IsNotNull(_mineralIdle, "No Idle Animation on mineral");
        //Assert.IsNotNull(_slowDownVelocity, "XXX is not null.");
        //Assert.IsNotNull(_stopVelocity, "XXX is not null.");
        //Assert.IsNotNull(_addMoreComponentsHere, "XXX is not null.");
    }

    private void InitializeStates()
    {
        //*Add All States to inherited BaseStateMachine "States" dictionary and Set Initial State
        States.Add(EStateMineral.ActivePhysics, new ActivePhysicsState(_context, EStateMineral.ActivePhysics));
        States.Add(EStateMineral.IsKinetic, new IsKineticState(_context, EStateMineral.IsKinetic));
        // cont. States.Add(EStateGeneric.NewGenericState, new NewGenericState(_context, EStateGeneric.NewGenericState));
        //Replace EGenericState with public enum name above
        //Replace NewGenericState with each new state

        CurrentState = States[EStateMineral.ActivePhysics];
    }
}
