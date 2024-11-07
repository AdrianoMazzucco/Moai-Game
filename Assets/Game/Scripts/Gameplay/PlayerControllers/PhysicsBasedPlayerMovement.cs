using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsBasedPlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction movement;

    private Rigidbody playerRB;

    [SerializeField] private GameObject currentCamera;
    [SerializeField] private GameObject topOfHead;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();

        var playerActionMap = inputActions.FindActionMap("Player");

        movement = playerActionMap?.FindAction("Move");
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 inputDirection = movement.ReadValue<Vector2>();
        if (inputDirection != Vector2.zero)
        {
            Vector3 camForward = transform.position - currentCamera.transform.position;
            camForward.y = 0;
            camForward.Normalize();
            
            Vector3 camRight = Vector3.Cross(new Vector3(0,1,0), camForward);

            Vector3 directionTotal = camForward * inputDirection.y + camRight * inputDirection.x;
            directionTotal.Normalize();
            directionTotal *= 30;

            playerRB.AddForce(directionTotal);

            //topOfHead.transform.rotation = Quaternion.LookRotation(directionTotal);

            Vector3 turnAngle = new Vector3(directionTotal.x, directionTotal.z, directionTotal.y);
            Quaternion deltaRotation = Quaternion.Euler(turnAngle * Time.fixedDeltaTime);
            topOfHead.transform.forward = Vector3.Lerp(topOfHead.transform.forward, directionTotal, 0.001f);
        }
    }
}
