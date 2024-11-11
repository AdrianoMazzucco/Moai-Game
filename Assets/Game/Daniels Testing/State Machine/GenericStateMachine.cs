using UnityEngine;
using UnityEngine.Assertions;

public class GenericStateMachine : BaseStateMachine <GenericStateMachine. EStateGeneric>
    //replace State machine to your desired state name 
{
    public enum EStateGeneric
    {
        Reset,
        NewGenericState,
        Each,
        Line,
        Is,
        A,
        State,
        Name,
    }
    //*change below name to state machine context name
    private GenericStateMachineContext _context;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _rootCollider;
    //[SerializeField] private AddMoreComponentsHere _addMoreComponentsHere;

    void Awake()
    {
        ValidateConstraints();
        _context = new GenericStateMachineContext(_rigidbody, _rootCollider);
        //*Pass all private member variables above through _context

        InitializeStates();
    }

    private void ValidateConstraints()
    {
        Assert.IsNotNull(_rigidbody, "Rigidbody is not assigned.");
        Assert.IsNotNull(_rootCollider, "Root Collider is not null.");
        //Assert.IsNotNull(_addMoreComponentsHere, "XXX is not null.");
    }

    private void InitializeStates()
    {
        //*Add All States to inherited BaseStateMachine "States" dictionary and Set Initial State
        States.Add(EStateGeneric.Reset, new NewGenericState(_context, EStateGeneric.Reset));
        States.Add(EStateGeneric.NewGenericState, new NewGenericState(_context, EStateGeneric.NewGenericState));
        // cont. States.Add(EStateGeneric.NewGenericState, new NewGenericState(_context, EStateGeneric.NewGenericState));
        //Replace EGenericState with public enum name above
        //Replace NewGenericState with each new state

        CurrentState = States[EStateGeneric.Reset];
    }
}
