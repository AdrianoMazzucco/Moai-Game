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
    [SerializeField] private Animation _animation;
    [SerializeField] private float _slowDownSpeed;
    [SerializeField] private float _stopSpeed;
    [SerializeField] private float _startDrag;
    [SerializeField] private float _dragIncrement;
    [SerializeField] private float _timeToTransition;
    [SerializeField] private bool _isKineticTransitionCooldown;
    [SerializeField] private bool _isSucked;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isUpright;
    [SerializeField] private bool _willNotDestroyFromIdle;
    [SerializeField] private AnimationClip _mineralIdle;
    //[SerializeField] private AddMoreComponentsHere _addMoreComponentsHere;

    void Awake()
    {
        ValidateConstraints();
        _context = new MineralStateMachineContext(_rigidbody, _rootCollider, _transform, _animation,
            _slowDownSpeed, _stopSpeed, _startDrag, _dragIncrement, _timeToTransition, _isKineticTransitionCooldown,
            _isSucked, _isUpright, _isGrounded, _willNotDestroyFromIdle, _mineralIdle);
        //*Pass all private member variables above through _context

        InitializeStates();
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_rigidbody, "Rigidbody is not assigned.");
        Assert.IsNotNull(_rootCollider, "Root Collider is not assigned.");
        Assert.IsNotNull(_transform, "Transform is not assigned.");
        Assert.IsNotNull(_animation, "Animation is not assigned");
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

    public void SetToNotDestroyFromIdle(bool b)
    {
        _context._willNotDestroyFromIdle = b;
    }

    public void OnDisable()
    {
        _context._willNotDestroyFromIdle = false;
    }
}
