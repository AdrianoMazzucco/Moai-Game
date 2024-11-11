using MoreMountains.Feedbacks;
using UnityEngine;

public class MineralStateMachineContext
{
    private Rigidbody _rigidbody;
    private Collider _rootCollider;
    private Transform _transform;

    private float _slowDownSpeed;
    private float _stopSpeed;
    private float _startDrag;
    private float _dragIncrement;

    private bool _isSucked;
    private bool _upright;

    private AnimationClip _mineralIdle;



    //*Add all member variable declerations (serialized fields) from concrete state here

    //Constructor - Adds values to all above member variables
    //*Pass all member variables above into StateMachineContext below
    public MineralStateMachineContext(Rigidbody rigidbody, Collider rootCollider, Transform transform, float slowDownSpeed,
        float stopSpeed, float slowDownDrag, float dragIncrement, bool isSucked, bool upright, AnimationClip mineralIdle)
    {
        //Assigns all variables to the context
        _rigidbody = rigidbody; 
        _rootCollider = rootCollider;
        _transform = transform;
        _slowDownSpeed = slowDownSpeed;
        _stopSpeed = stopSpeed;
        _startDrag = slowDownDrag;
        _dragIncrement = dragIncrement;
        _isSucked = isSucked;
        _upright = upright;
        _mineralIdle = mineralIdle;
        //*Add all passed through variables here that are in the Base State

    }

    public Rigidbody Rigidbody => _rigidbody;
    public Collider RootCollider => _rootCollider;
    public Transform Transform => _transform;
    public float SlowDownSpeed => _slowDownSpeed;
    public float StopSpeed => _stopSpeed;
    public float StartDrag => _startDrag;
    public float DragIncrement => _dragIncrement;
    public bool IsSucked => _isSucked;
    public bool Upright => _upright;
    public AnimationClip MineralIdle => _mineralIdle;
}
