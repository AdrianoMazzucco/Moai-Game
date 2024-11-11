using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsBasedPlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction movement;
    private InputAction chargeAttack;

    private Rigidbody playerRB;
    private bool charging = false;
    private float chargeAmount = 0;

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
        Movement();
        ChargeUpdate();
    }

    private void Movement()
    {
        if (charging) return;

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
            directionTotal *= 30;

            playerRB.AddForce(directionTotal);

            Vector3 turnAngle = new Vector3(directionTotal.x, directionTotal.z, directionTotal.y);
            Quaternion deltaRotation = Quaternion.Euler(turnAngle * Time.fixedDeltaTime);
            transform.forward = Vector3.Lerp(transform.forward, directionTotal, 0.005f);
        }
    }

    private void ChargeUpdate()
    {
        if (!charging) return;

        if(chargeAmount < 1)
            chargeAmount += Time.deltaTime;

        Vector3 euler = transform.localEulerAngles;
        euler.x = Mathf.Lerp(0, -45, chargeAmount);
        transform.localEulerAngles = euler;
    }

    private void StartCharge(InputAction.CallbackContext obj)
    {
        charging = true;
    }

    private void ChargeAttack(InputAction.CallbackContext obj) 
    {
        charging = false;
        chargeAmount = 0;
        playerRB.freezeRotation = false;
        // playerRB.AddForce(transform.forward * 100, ForceMode.Impulse);
        playerRB.AddForceAtPosition(transform.forward * 50, transform.position + transform.up * 1.5f, ForceMode.Impulse);
    }
}
