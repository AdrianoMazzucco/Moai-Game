using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState
{
    walking, charging, flying, sucking 
}

public class PhysicsBasedPlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction movement;
    private InputAction chargeAttack;
    private InputAction jumpInput;
    private InputAction suckAbility;

    private Rigidbody playerRB;
    private float chargeAmount = 0;
    private MovementState currentState = MovementState.walking;
    private float flightTime = 0;
    [SerializeField] private float flightDuration = 3;
    [SerializeField] private float movementForceMultiplier = 30;
    [SerializeField] private float chargeForcecMultiplier = 50;
    [SerializeField] private float tiltBackAngle = 45;
    [SerializeField] private float maxSpeed = 10;

    bool jumpCharging = false;
    float jumpChargeForce = 0;
    float jumpChargeTime = 0;
    [SerializeField] private float time4FullJumpCharge = 1.5f;
    [SerializeField] private float minimumJumpForce = 10;
    [SerializeField] private float maximumJumpForce = 50;

    [SerializeField] private GameObject currentCamera;


    private void OnEnable()
    {
        chargeAttack.performed += StartCharge;
        chargeAttack.canceled += ChargeAttack;

        jumpInput.performed += StartJump;
        jumpInput.canceled += Jump;

        suckAbility.performed += StartSuck;
        suckAbility.canceled += EndSuck;
    }

    private void OnDisable()
    {
        chargeAttack.performed -= StartCharge;
        chargeAttack.canceled -= ChargeAttack;

        jumpInput.performed -= StartJump;
        jumpInput.canceled -= Jump;

        suckAbility.performed -= StartSuck;
        suckAbility.canceled -= EndSuck;
    }

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();

        var playerActionMap = inputActions.FindActionMap("Player");

        movement = playerActionMap?.FindAction("Move");
        chargeAttack = playerActionMap?.FindAction("Fire");
        jumpInput = playerActionMap?.FindAction("Jump");

        suckAbility = playerActionMap?.FindAction("Suck");
    }

    private void Update()
    {
        switch(currentState)
        {
            case MovementState.walking:
                Movement();
                break;
            case MovementState.charging:
                ChargeUpdate();
                break; 
            case MovementState.flying:
                Flight();
                break;
            case MovementState.sucking:
                SuckUpdate();
                break;
        }

        if(jumpCharging) 
        {
            jumpChargeTime += Time.deltaTime;
            if(jumpChargeTime >  time4FullJumpCharge) { jumpChargeTime = time4FullJumpCharge; }
            jumpChargeForce = minimumJumpForce + (maximumJumpForce - minimumJumpForce) * (jumpChargeTime / time4FullJumpCharge);
            Debug.Log(jumpChargeForce);
        }
    }

    private void Movement()
    {
        Vector2 inputDirection = movement.ReadValue<Vector2>();
        if (inputDirection != Vector2.zero)
        {
            playerRB.freezeRotation = true;

            Vector3 camForward = transform.position - currentCamera.transform.position;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = Vector3.Cross(new Vector3(0, 1, 0), camForward);

            Vector3 directionTotal = camForward * inputDirection.y + camRight * inputDirection.x;
            directionTotal.Normalize();
            directionTotal *= movementForceMultiplier;
            if (jumpCharging) { directionTotal *= 0.5f; }

            playerRB.AddForce(directionTotal);
            
            if (playerRB.linearVelocity.magnitude > maxSpeed)
            {
                playerRB.linearVelocity = playerRB.linearVelocity.normalized * maxSpeed;
            }
            Vector3 turnAngle = new Vector3(directionTotal.x, directionTotal.z, directionTotal.y);
            Quaternion deltaRotation = Quaternion.Euler(turnAngle * Time.fixedDeltaTime);
            transform.forward = Vector3.Lerp(transform.forward, directionTotal, 0.005f);
        }
    }

    private void Flight()
    {
        transform.forward = Vector3.Lerp(transform.forward, new Vector3(0, -1, 0), 0.05f);
        flightTime += Time.deltaTime;
        if( flightTime > flightDuration ) 
        {
            flightTime = 0;
            currentState = MovementState.walking;
        }
    }

    private void ChargeUpdate()
    {
        if(chargeAmount < 1)
            chargeAmount += Time.deltaTime;

        Vector3 euler = transform.localEulerAngles;
        euler.x = Mathf.Lerp(0, -tiltBackAngle, chargeAmount);
        transform.localEulerAngles = euler;
    }

    private void StartCharge(InputAction.CallbackContext obj)
    {
        if (currentState == MovementState.walking)
        {
            currentState = MovementState.charging;
        }
    }

    private void ChargeAttack(InputAction.CallbackContext obj) 
    {
        if (currentState != MovementState.charging) return;

        currentState = MovementState.flying;
        chargeAmount = 0;
        // playerRB.AddForce(transform.forward * 100, ForceMode.Impulse);
        playerRB.AddForce(transform.forward * chargeForcecMultiplier, ForceMode.Impulse);
    }

    private void StartJump(InputAction.CallbackContext obj)
    {
        jumpCharging = true;
    }

    private void Jump(InputAction.CallbackContext obj) 
    {
        jumpCharging = false;
        jumpChargeTime = 0;
        playerRB.AddForce(new Vector3(0, jumpChargeForce, 0), ForceMode.Impulse);
    }

    private void StartSuck(InputAction.CallbackContext obj)
    {
        currentState = MovementState.sucking;
    }

    private void EndSuck(InputAction.CallbackContext obj)
    {
       currentState = MovementState.walking;
    }

    private void SuckUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10);

        foreach(var hitCollider in hitColliders) 
        {
            if(hitCollider.gameObject.GetComponent < MineralStateMachine > () )
            {
                Vector3 direction = transform.position - hitCollider.transform.position;
                hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(direction);
            }
        }
    }
}
