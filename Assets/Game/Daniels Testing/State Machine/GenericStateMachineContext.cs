using MoreMountains.Feedbacks;
using UnityEngine;

public class GenericStateMachineContext
{
    private Rigidbody _rigidbody;
    private Collider _rootCollider;
    //*Add all member variable declerations (serialized fields) from concrete state here

    //Constructor - Adds values to all above member variables
    //*Pass all member variables above into StateMachineContext below
    public GenericStateMachineContext(Rigidbody rigidbody, Collider rootCollider)
    {
        //Assigns all variables to the context
        _rigidbody = rigidbody; 
        _rootCollider = rootCollider;
        //*Add all passed through variables here that are in the Base State

    }

    public Rigidbody Rigidbody => _rigidbody;
    public Collider RootCollider => _rootCollider;

}
