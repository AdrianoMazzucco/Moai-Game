using MoreMountains.Feedbacks;
using UnityEngine;

public class MineralStateMachineContext
{
    private Rigidbody _rigidbody;
    private Collider _rootCollider;
    private Transform _transform;
    private Animation _animation;

    private float _slowDownSpeed;
    private float _stopSpeed;
    private float _startDrag;
    private float _dragIncrement;
    private float _timeToTransition;

    private bool _isKineticTransitionCooldown;
    private bool _isSucked;
    private bool _isUpright;
    private bool _isGrounded;
    public bool _willNotDestroyFromIdle = false;

    private AnimationClip _mineralIdle;



    //*Add all member variable declerations (serialized fields) from concrete state here

    //Constructor - Adds values to all above member variables
    //*Pass all member variables above into StateMachineContext below
    public MineralStateMachineContext(Rigidbody rigidbody, Collider rootCollider, Transform transform, Animation animation, 
        float slowDownSpeed, float stopSpeed, float slowDownDrag, float dragIncrement, float timeToTransition,
        bool isKineticTransitionCooldown, bool isSucked, bool isUpright, bool isGrounded, bool willNotDestroyFromIdle,
        AnimationClip mineralIdle)
    {
        //Assigns all variables to the context
        _rigidbody = rigidbody; 
        _rootCollider = rootCollider;
        _transform = transform;
        _animation = animation;
        _slowDownSpeed = slowDownSpeed;
        _stopSpeed = stopSpeed;
        _startDrag = slowDownDrag;
        _dragIncrement = dragIncrement;
        _timeToTransition = timeToTransition;
        _isKineticTransitionCooldown = isKineticTransitionCooldown;
        _isSucked = isSucked;
        _isUpright = isUpright;
        _isGrounded = isGrounded;
        _mineralIdle = mineralIdle;
        _willNotDestroyFromIdle = willNotDestroyFromIdle;
        //*Add all passed through variables here that are in the Base State

    }

    public Rigidbody Rigidbody => _rigidbody;
    public Collider RootCollider => _rootCollider;
    public Transform Transform => _transform;
    public Animation Animation => _animation;
    public float SlowDownSpeed => _slowDownSpeed;
    public float StopSpeed => _stopSpeed;
    public float StartDrag => _startDrag;
    public float DragIncrement => _dragIncrement;
    public float TimeToTransition => _timeToTransition;
    public bool IsKineticTransitionCooldown => _isKineticTransitionCooldown;
    public bool IsSucked => _isSucked;
    public bool IsUpright => _isUpright;
    public bool IsGrounded => _isGrounded;

    public bool WillNotDestroyFromIdle => _willNotDestroyFromIdle;
    public AnimationClip MineralIdle => _mineralIdle;
}
