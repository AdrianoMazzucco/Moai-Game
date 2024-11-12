using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState
{
    walking, charging, flying 
}

public class PhysicsBasedPlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction movement;
    private InputAction chargeAttack;

    private Rigidbody playerRB;
    private float chargeAmount = 0;
    private MovementState currentState = MovementState.walking;
    private float flightTime = 0;
    [SerializeField] private float flightDuration = 3;
    [SerializeField] private float movementForceMultiplier = 30;
    [SerializeField] private float chargeForcecMultiplier = 50;
    [SerializeField] private float tiltBackAngle = 45;

    [SerializeField] private GameObject currentCamera;


    private void OnEnable()
    {
        chargeAttack.performed += StartCharge;
        chargeAttack.canceled += ChargeAttack;
    }

    private void OnDisable()
    {
        chargeAttack.performed -= StartCharge;
    }

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();

        var playerActionMap = inputActions.FindActionMap("Player");

        movement = playerActionMap?.FindAction("Move");
        chargeAttack = playerActionMap?.FindAction("Fire");
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

            playerRB.AddForce(directionTotal);

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
}
