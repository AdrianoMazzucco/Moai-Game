using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementState
{
    walking, charging, flying, sucking 
}

public class PhysicsBasedPlayerMovement : MonoBehaviour , IDestructable
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction movement;
    private InputAction chargeAttack;
    private InputAction jumpInput;
    private InputAction suckAbility;

    public  Rigidbody playerRB;
    private float chargeAmount = 0;
    private MovementState currentState = MovementState.walking;

    public MovementState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            if (currentState != value)
            {
                currentState = value;
                CheckAnimation();
            }
            else
            {
                currentState = value;
            }
        }
    }

    private float flightTime = 0;
    [SerializeField] private float flightDuration = 3;

    bool jumpCharging = false;
    private bool bisGrounded;
    float jumpChargeForce = 0;
    float jumpChargeTime = 0;
    
    private CapsuleCollider _capsuleCollider;
    
    [SerializeField] private float time4FullJumpCharge = 1.5f;
    [SerializeField] private float tiltBackAngle = 45;

    [SerializeField] private float GroundCheckRadiusMultiplier = 1.0f;
    [SerializeField] private float shockwavePushMultiplier = 10f;
    [SerializeField] private float verticalShockwaveValue = 10f;
    private float groundedDistance = 0;
    /*
     * Should refactor all the stats
    */
    #region Player Stats
    [ReadOnly, SerializeField] private float movementForceMultiplier = 30;
    [SerializeField] private float multiplierMovementForceMultiplier = 1;
    [SerializeField] private float minimumMovementForceMultiplier = 30;
    [SerializeField] private float maximumMovementForceMultiplier = 60;
    
    [ReadOnly, SerializeField] private float SuckMovementForceMultiplier = 10;
    [SerializeField] private float SuckmultiplierMovementForceMultiplier = 1;
    [SerializeField] private float SuckminimumMovementForceMultiplier = 15;
    [SerializeField] private float SuckmaximumMovementForceMultiplier = 30;

    [ReadOnly, SerializeField] private float airMovementForceMultiplier = 15;
    [SerializeField] private float multiplierAirMovementForceMultiplier = 1;
    [SerializeField] private float minimumAirMovementForceMultiplier = 15;
    [SerializeField] private float maximumAirMovementForceMultiplier = 30;

    [ReadOnly, SerializeField] private float currentDrag = 0;
    [SerializeField] private float multiplierDrag = 1;
    [SerializeField] private float minimumDrag = 0;
    [SerializeField] private float maximumDrag = 5;

    [ReadOnly, SerializeField] private float chargeForcecMultiplier = 50;
    [SerializeField] private float multiplierChargeForcecMultiplier = 1;
    [SerializeField] private float minimumChargeForcecMultiplier = 50;
    [SerializeField] private float maximumChargeForcecMultiplier = 100;

    [ReadOnly, SerializeField] private float maxSpeed = 10;
    [SerializeField] private float multiplierMaxSpeed = 1;
    [SerializeField] private float minimumMaxSpeed = 10;
    [SerializeField] private float maximumMaxSpeed = 20;

    [ReadOnly, SerializeField] private float suckRadius = 10;
    [SerializeField] private float multiplierSuckRadius = 1;
    [SerializeField] private float minimumSuckRadius = 10;
    [SerializeField] private float maximumSuckRadius = 20;

    [ReadOnly, SerializeField] private float minimumJumpForce = 10;
    [SerializeField] private float multiplierMinimumJumpForce = 1;
    [SerializeField] private float minimumMinimumJumpForce = 10;
    [SerializeField] private float maximumMinimumJumpForce = 20;

    [ReadOnly, SerializeField] private float maximumJumpForce = 50;
    [SerializeField] private float multiplierMaximumJumpForce = 1;
    [SerializeField] private float minimumMaximumJumpForce = 50;
    [SerializeField] private float maximumMaximumJumpForce = 100;

    [ReadOnly, SerializeField] private float currentScale = 1;
    [SerializeField] private float multiplierScale = 0.25f;
    [SerializeField] private float minimumScale = 1;
    [SerializeField] private float maximumScale = 10;

    [ReadOnly, SerializeField] private float currentMass = 5;
    [SerializeField] private float multiplierMass = 1;
    [SerializeField] private float minimumMass = 5;
    [SerializeField] private float maximumMass = 20;


    #endregion
    
    [SerializeField] private GameObject currentCamera;
    [SerializeField] private Animator playerAnimator;

    [Header("VFX")] 
    [SerializeField] private ParticleSystem particleTrail1;
    [SerializeField] private ParticleSystem particleTrail2;

    [Header("Feel")] 
    [SerializeField] private MMF_Player SuckFeel;
    
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

    private void Start()
    {
        _capsuleCollider = this.GetComponent<CapsuleCollider>();
        GameManager.Instance.playerMovementScript = this;
        GameManager.Instance.playerGameObject = this.gameObject;
    }

    private void FixedUpdate()
    { 
        bisGrounded = CheckGrounded();
    }



    private void Update()
    {        
        switch(CurrentState)
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

        if (bisGrounded)
        {

            if (!particleTrail1.isPlaying)
            {
                particleTrail1.Clear();
                particleTrail1.Play();
            }

            if (!particleTrail2.isPlaying)
            {
                particleTrail2.Clear();
                particleTrail2.Play();
            }

        }
        else
        {
            particleTrail1.Stop();
            particleTrail2.Stop();
        }
        if(jumpCharging) 
        {
           
            jumpChargeTime += Time.deltaTime;
            if(jumpChargeTime >  time4FullJumpCharge) { jumpChargeTime = time4FullJumpCharge; }
            jumpChargeForce = minimumJumpForce + (maximumJumpForce - minimumJumpForce) * (jumpChargeTime / time4FullJumpCharge);
            //Debug.Log(jumpChargeForce);
        }
    }

    private void Movement()
    {
        Vector2 inputDirection = movement.ReadValue<Vector2>();
        playerAnimator.SetFloat("WalkBlend",playerRB.linearVelocity.magnitude);
        if (inputDirection != Vector2.zero)
        {
            playerRB.freezeRotation = true;

            Vector3 camForward = transform.position - currentCamera.transform.position;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = Vector3.Cross(new Vector3(0, 1, 0), camForward);

            Vector3 directionTotal = camForward * inputDirection.y + camRight * inputDirection.x;
            directionTotal.Normalize();
            if (bisGrounded)
            {
                directionTotal *= movementForceMultiplier;
            }
            else
            {
                directionTotal *= airMovementForceMultiplier;
            }
            if (jumpCharging) { directionTotal *= 0.5f; }

            playerRB.AddForce(directionTotal, ForceMode.Acceleration);
            
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
        //transform.forward = Vector3.Lerp(transform.forward, new Vector3(0, -1, 0), 0.05f);   // ENABLE THIS TO RETURN TO OLD CODE
        flightTime += Time.deltaTime;
        if (bisGrounded) {
            playerAnimator.SetTrigger("Vertical");
            // transform.up = Vector3.Lerp(transform.up, new Vector3(0, -1, 0), 0.05f);
        }
        
        

        if( flightTime >= flightDuration ) 
        {
            flightTime = 0;
            CurrentState = MovementState.walking;
        }

        if (playerRB.linearVelocity.magnitude < 2f)
        {
           //
        }
    }

    // private void GroundedFlight()
    // {
    //    
    //     flightTime += Time.deltaTime;
    //     transform.forward = Vector3.Lerp(transform.forward, new Vector3(-1, 0, -1), 0.05f);
    //
    //     if( flightTime >= flightDuration ) 
    //     {
    //         flightTime = 0;
    //         CurrentState = MovementState.walking;
    //        
    //     }
    // }

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
        if (CurrentState == MovementState.walking && !jumpCharging && bisGrounded)
        {
            CurrentState = MovementState.charging;
        }
    }

    private void ChargeAttack(InputAction.CallbackContext obj) 
    {
        if (CurrentState != MovementState.charging) return;
        
        CurrentState = MovementState.flying;
       
        chargeAmount = 0;
        // playerRB.AddForce(transform.forward * 100, ForceMode.Impulse);
        playerRB.AddForce(transform.forward * chargeForcecMultiplier, ForceMode.Impulse);
    }

    private void StartJump(InputAction.CallbackContext obj)
    {
        if (CurrentState == MovementState.walking && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            jumpChargeTime = 0;
            jumpCharging = true;
            if(bisGrounded) playerAnimator.SetTrigger("JumpCharge");
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (!jumpCharging) { return; }
        if (bisGrounded)
        {
            if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("JumpCharge")) playerAnimator.SetTrigger("Jump");
            jumpCharging = false;
            jumpChargeTime = 0;
            playerRB.AddForce(new Vector3(0, jumpChargeForce, 0), ForceMode.Impulse);
        }
    }

    private void StartSuck(InputAction.CallbackContext obj)
    {
        if (CurrentState == MovementState.walking && !jumpCharging)
        {
            CurrentState = MovementState.sucking;
            playerAnimator.SetBool("isSuck", true);
            SuckFeel?.PlayFeedbacks();
        }
    }

    private void EndSuck(InputAction.CallbackContext obj)
    {
       CurrentState = MovementState.walking;
       playerAnimator.SetBool("isSuck",false);
       SuckFeel?.PlayFeedbacks();
    }
    
    private void SuckUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, suckRadius);

        foreach(var hitCollider in hitColliders) 
        {
            if(hitCollider.gameObject.GetComponent < MineralStateMachine > () )
            {
                Vector3 direction = transform.position - hitCollider.transform.position;
                hitCollider.gameObject.GetComponent<Rigidbody>().AddForce(direction);
            }
        }
        Vector2 inputDirection = movement.ReadValue<Vector2>();
        playerAnimator.SetFloat("WalkBlend",playerRB.linearVelocity.magnitude);
        if (inputDirection != Vector2.zero)
        {
            playerRB.freezeRotation = true;

            Vector3 camForward = transform.position - currentCamera.transform.position;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = Vector3.Cross(new Vector3(0, 1, 0), camForward);

            Vector3 directionTotal = camForward * inputDirection.y + camRight * inputDirection.x;
            directionTotal.Normalize();
            if (bisGrounded)
            {
                directionTotal *= SuckMovementForceMultiplier;
            }
            else
            {
                directionTotal *= airMovementForceMultiplier;
            }
            if (jumpCharging) { directionTotal *= 0.5f; }

            playerRB.AddForce(directionTotal, ForceMode.Acceleration);
            
            if (playerRB.linearVelocity.magnitude > maxSpeed)
            {
                playerRB.linearVelocity = playerRB.linearVelocity.normalized * maxSpeed;
            }
            Vector3 turnAngle = new Vector3(directionTotal.x, directionTotal.z, directionTotal.y);
            Quaternion deltaRotation = Quaternion.Euler(turnAngle * Time.fixedDeltaTime);
            transform.forward = Vector3.Lerp(transform.forward, directionTotal, 0.005f);
        }
    }

    private void CheckAnimation()
    {
        switch(CurrentState)
        {
            case MovementState.walking:
                playerAnimator.SetTrigger("Walk");
                break;
            case MovementState.charging:
                playerAnimator.SetTrigger("Charge");
                break; 
            case MovementState.flying:
                playerAnimator.SetTrigger("Spin");
                playerAnimator.ResetTrigger("Charge");
                break;
            case MovementState.sucking:
                playerAnimator.SetTrigger("Walk");
                break;
        }
    }
    
    public void UpdateStats(int _mineralCount)
    {
        movementForceMultiplier = minimumMovementForceMultiplier + multiplierMovementForceMultiplier * _mineralCount;
        if (movementForceMultiplier > maximumMovementForceMultiplier) { movementForceMultiplier = maximumMovementForceMultiplier; }

        airMovementForceMultiplier = minimumAirMovementForceMultiplier + multiplierAirMovementForceMultiplier * _mineralCount;
        if (airMovementForceMultiplier > maximumAirMovementForceMultiplier) { airMovementForceMultiplier = maximumAirMovementForceMultiplier; }

        chargeForcecMultiplier = minimumChargeForcecMultiplier + multiplierChargeForcecMultiplier * _mineralCount;
        if (chargeForcecMultiplier > maximumChargeForcecMultiplier) { chargeForcecMultiplier = maximumChargeForcecMultiplier; }

        maxSpeed = minimumMaxSpeed + multiplierMaxSpeed * _mineralCount;
        if (maxSpeed > maximumMaxSpeed) { maxSpeed = maximumMaxSpeed; }

        minimumJumpForce = minimumMinimumJumpForce + multiplierMinimumJumpForce * _mineralCount;
        if (minimumJumpForce > maximumJumpForce) { minimumJumpForce = maximumJumpForce; }

        maximumJumpForce = minimumMaximumJumpForce + multiplierMaximumJumpForce * _mineralCount;
        if (maximumJumpForce > maximumMaximumJumpForce) { maximumJumpForce = maximumMaximumJumpForce; }

        suckRadius = minimumSuckRadius + multiplierSuckRadius * _mineralCount;
        if (suckRadius > maximumSuckRadius) { suckRadius = maximumSuckRadius; }

        currentScale = minimumScale + (multiplierScale * _mineralCount);
        if(currentScale > maximumScale) {  currentScale = maximumScale; }
        transform.localScale= new Vector3(currentScale, currentScale, currentScale);

        currentDrag = minimumDrag + multiplierDrag * _mineralCount;
        if (currentDrag > maximumDrag) { currentDrag = maximumDrag; }
        playerRB.linearDamping  = currentDrag;

        currentMass = minimumMass + multiplierMass * _mineralCount;
        if (currentMass > maximumMass) { currentMass = maximumMass; }
        playerRB.mass = currentMass;
            
        
    }

    private bool CheckGrounded(float _lengthMultiplier = 1)
    {
        //bool bground =  Physics.Raycast(transform.position, -Vector3.up, currentScale * 1.2f * _lengthMultiplier);
        float radius;
        Vector3 pos;

        radius = _capsuleCollider.radius * GroundCheckRadiusMultiplier;
        pos = transform.position + Vector3.up * (radius * 0.9f);

        // else
        // {
        //     radius = _capsuleCollider.radius * 0.9f;
        //     pos = transform.position + Vector3.up * (radius * 0.9f);
        // }



        bool bground = Physics.CheckSphere(pos, radius, 1 << 10);
        bisGrounded = bground;
        playerAnimator.SetBool("isGrounded", bground);
        



        return bground;
    }

    public void Destruct(Vector3 position)
    {
        playerRB.AddForce(((this.transform.position - position).normalized 
                 + new Vector3(0,verticalShockwaveValue,0))* shockwavePushMultiplier,ForceMode.Impulse );
    }
}
