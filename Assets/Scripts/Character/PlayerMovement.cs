using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    #region Fields
    [Header("General")]
    [HideInInspector] public Vector3 vel; //Velocity
    [HideInInspector] public Vector2 vel2D;
    public bool isGrounded; //grounded
    public Player player; //Player

    [Header("Ground")]
    public float maxWalkSpeed; //Max velocity while walking
    public float groundAcceleration; //acceleration speed while on ground
    private bool _landed; //if landed has happened

    [Header("Air")]
    public float airAcceleration; //acceleration speed while in air
    public float gravity; //gravity velocity
    public float maxGravity; //Maximum gravity velocity

    private bool _inAir; //if player is in air
    private float _airTimer; //time while in air
    private bool _removedJump; //If one jump has been removed from jumpsleft

    [Header("Jump")]
    public float initialJumpForce; //jump velocity
    public float continuedJumpForce; //jump velocity when holding space for higher jump
    public float continuedJumpDuration; //time allowed to hold jump button for extra height
    private float _continuedJumpTimer;
    
    private bool _wishJump; //if the player wishes to jump
    private bool _wishedJumpPerformed;
    public float jumpGap; //amount of time the player can still jump after running off an edge
    public int maxJumps; //Maximum amount of jumps until hitting the ground again
    private bool _inJump; //If the player is currently doing a jump

    public int jumpsLeft; //current jumps left

    [Header("GroundPound")]
    //public float highJumpForce; //jump velocity
    //public float gpFreezeInAirTime; //amount of time player will freeze in the air before getting launched downwards
    //public float gpForwardPushForce; //how much should groundpound launch the player forwards
    public Vector2 gpDistanceForce;
    public Vector2 gpHighJumpForce;
    public float gpSuperChargeForwardMultiplier = 1.5f;
    public float gpSuperChargeUpwardsMutliplier = 1.25f;
    public float gpDownwardsPushForce; //how much should groundpound launch the player downwards before superjump
    public float gpNoMovementJumpHeightMultiplier; //Extra force for the super jump if player isn't pressing any movement inputs
    public float gpSlowmoMultiplier;
    public float gpUpgradeTime = 0.5f;
    private bool _isGroundPounding;
    private bool _isSuperJumping;
    private bool _isChargingGroundPound = false;
    private bool _gpIsUpgraded = false;
   

    [Header("Misc")]
    public Transform cameraPivot; // Camera pivot

    //Private
    Vector2 _movement; //movement from inputs
    public Vector3 moveVec; //movement in vector3
    public CharacterController _controller; //character controller
    Vector3 oldPos; //position before moving character
    float movedDistance = 0;
    public float stepDistance = 1f;
    private float _speedMultiplier = 1f;

    PlayerInput inputs;


    #endregion

    #region Propeties

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public bool IsUsingGroundPound()
    {
        return _isGroundPounding || _isChargingGroundPound;
    }

    #endregion

    #region Unity Methods (Start, Awake, Update)

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        inputs.Main.Jump.performed -= JumpInput;
        inputs.Main.Jump.canceled -= JumpInput;
        inputs.Main.Fire1.performed -= Fire1Input;
        inputs.Main.Fire1.canceled -= Fire1Input;
    }


    // Start is called before the first frame update
    void Start()
    {
        //setup
        _controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        inputs = GameManager.Instance.inputs;

        inputs.Main.Jump.performed += JumpInput;
        inputs.Main.Jump.canceled += JumpInput;
        inputs.Main.Fire1.performed += Fire1Input;
        inputs.Main.Fire1.canceled += Fire1Input;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.gameActive)
        {
            HandleInputs();
        }
            
        //update grounded
        isGrounded = _controller.isGrounded;

        vel2D = new Vector2(vel.x, vel.z);

        Jump();

        //Ground and air movement
        if (isGrounded)
        {
            _removedJump = false;
            _inJump = false;
            Land();   
        }
        else
        {
            CheckWalkOffEdge();
            AirTimer();
            ApplyGravity();
        }

        Move();
        

        oldPos = transform.position;

        //Move the player
        _controller.Move(Time.deltaTime * _speedMultiplier * vel);
        

        CancelVelocity();
        
    }

    #endregion

    void AirTimer()
    {
        _airTimer += Time.deltaTime;
        _landed = false;
        
        if(_airTimer >= jumpGap && !_removedJump && !_inJump)
        {
            jumpsLeft--;
            _removedJump = true;
        }

        if (_wishJump && _wishedJumpPerformed)
        {
            _continuedJumpTimer += Time.deltaTime;
        }
    }

    void RotateCharacterTowardsMoveDirection(Vector3 targetDir)
    {
        if (targetDir.magnitude < 0.9) return;

        Quaternion rotTarget = Quaternion.LookRotation(targetDir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, (500f*Time.deltaTime));
    }


    void HandleInputs()
    {
        float forward = inputs.Main.Forward.ReadValue<float>();
        float backwards = inputs.Main.Backwards.ReadValue<float>();
        float right = inputs.Main.Right.ReadValue<float>();
        float left = inputs.Main.Left.ReadValue<float>();

        _movement = new Vector2(right - left, forward - backwards);

        Vector3 forwardMovement = cameraPivot.forward * _movement.y;
        Vector3 sidewaysMovement = cameraPivot.right * _movement.x;
        Vector3 cameraRelativeMovement = forwardMovement + sidewaysMovement;
        Vector3 cameraRelativeMovementLocal = transform.InverseTransformDirection(cameraRelativeMovement);

        moveVec = new Vector3(cameraRelativeMovementLocal.x, 0, cameraRelativeMovementLocal.z).normalized;

        RotateCharacterTowardsMoveDirection(new Vector3(cameraRelativeMovement.x, 0, cameraRelativeMovement.z).normalized);
    }


    void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _wishJump = true;
            _wishedJumpPerformed = false;
        }
        if (context.canceled)
        {
            _wishJump = false;
        }
    }

    void Fire1Input(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ChargeGroundPound();
        }
        else if (context.canceled)
        {
            PerformGroundPoundMovement();
        }
    }


    /// <summary>
    /// Ground move 
    /// </summary>
    void Move()
    {
        Vector3 fixedMovement = transform.TransformVector(moveVec).normalized;

        float maxSpeed = maxWalkSpeed;

        // Move vel vector towards target vel
        Vector3 velXZ = new Vector3(vel.x, 0, vel.z);
        Vector3 newVelXZ = Vector3.MoveTowards(velXZ, maxSpeed * fixedMovement, groundAcceleration * Time.deltaTime);
        vel = new Vector3(newVelXZ.x, vel.y, newVelXZ.z);

        movedDistance += newVelXZ.magnitude * Time.deltaTime;
        if(movedDistance > stepDistance)
        {
            movedDistance -= stepDistance;
        }

    }

    void SuperJump()
    {
        //Add Velocity
        float jumpVel = _movement.magnitude > 0.8f ? gpDistanceForce.y : gpHighJumpForce.y;
        if (_gpIsUpgraded)
        {
            jumpVel *= gpSuperChargeUpwardsMutliplier;
        }
        vel.y = jumpVel;
        _airTimer = 1;

        _isSuperJumping = true;

        AdjustValuesWhenJumpPerformed();
    }

    void PerformGroundPoundMovement()
    {

        _speedMultiplier = 1;
        if (isGrounded)
        {
            _isChargingGroundPound = false;
                return;
        }
            
        if (!_isChargingGroundPound) return;

        _isChargingGroundPound = false;

        _isGroundPounding = true;



        float forwardsSpeed = _movement.magnitude > 0.8f ? gpDistanceForce.x : gpHighJumpForce.x;
        if (_gpIsUpgraded)
        {
            forwardsSpeed *= gpSuperChargeForwardMultiplier;
        }

        Vector3 force = transform.forward * forwardsSpeed + new Vector3(0, -gpDownwardsPushForce, 0);
        ApplyForce(force);
    }

    void ChargeGroundPound()
    {
        if (isGrounded || _isChargingGroundPound || _isGroundPounding || _isSuperJumping) return;

        _gpIsUpgraded = false;
        _speedMultiplier = gpSlowmoMultiplier;
        _isChargingGroundPound = true;
        StartCoroutine(UpgradeGroundPoundToSuper());
    }

    /// <summary>
    /// Jump 
    /// </summary>
    void Jump()
    {
        if (!_wishJump) return;

        if(isGrounded || _airTimer < jumpGap || jumpsLeft > 0 && !_wishedJumpPerformed)
        {
            if (vel.y <= initialJumpForce)
            {
                vel.y = initialJumpForce;
                _airTimer = 1;
            }

            _wishedJumpPerformed = true;

            AdjustValuesWhenJumpPerformed();

            if (_isGroundPounding)
            {
                _isGroundPounding = false;
            }
        }
        else if(_wishedJumpPerformed && _continuedJumpTimer < continuedJumpDuration && vel.y <= initialJumpForce*1.5f) //Only if player has less velocity than the highest you can reach from a normal jump
        {
            vel.y = vel.y + (continuedJumpForce*200*Time.deltaTime);
        }
        else
        {
            _wishJump = false;
        }
    }

    void AdjustValuesWhenJumpPerformed()
    {
        isGrounded = false;
        _inAir = true;
        jumpsLeft--;
        _inJump = true;
        _continuedJumpTimer = 0;
    }

    /// <summary>
    /// Check if the player is walking off an edge and adjust Y velocity depending on that
    /// </summary>
    private void CheckWalkOffEdge()
    {
        //return if we are on the way up as this is only useful for downgoing velocity
        if (vel.y > 0 || _inAir)
        {
            return;
        }

        _inAir = true;

        //Raycast downwards to see if ground is under the player
        float dist = 0.25f;
        var layerMask = 1 << LayerMask.NameToLayer("Default");
        bool rayHit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit _,
            dist + _controller.height * 0.6f, layerMask, QueryTriggerInteraction.Ignore);

        //If there is ground, keep some downgoing velocity, else make it 0.
        vel.y = rayHit ? -4 : -0;
    }

    private void Land()
    {

        if (_landed)
            return;

        _inAir = false;
        _airTimer = 0;
        jumpsLeft = maxJumps;
        _wishedJumpPerformed = false;


        if (vel.y < -4 && _airTimer > 0.5f)
        {
            //Landing sound
        }

        vel.y = -2;
 
        _landed = true;

        if (_isGroundPounding)
        {
            SuperJump();
        }

        if(!_isGroundPounding && _isSuperJumping)
        {
            _isSuperJumping = false;
        }

        _isGroundPounding = false;
        
    }

    void ApplyGravity()
    {
        if (-vel.y < maxGravity)
        {
            vel += gravity * Time.deltaTime * Vector3.down;
        }
    }

    void CancelVelocity()
    {

        if(Mathf.Abs(transform.position.x - oldPos.x) < Mathf.Abs(vel.x * Time.deltaTime * 0.2f * _speedMultiplier))
        {
            vel.x = 0;
        }

        if (Mathf.Abs(transform.position.y - oldPos.y) < Mathf.Abs(vel.y * Time.deltaTime * 0.2f * _speedMultiplier))
        {
            if(vel.y > 0)
            {
                vel.y = 0;
            }
        }

        if (Mathf.Abs(transform.position.z - oldPos.z) < Mathf.Abs(vel.z * Time.deltaTime * 0.2f * _speedMultiplier))
        {
            vel.z = 0;
        }
    }

    public void ApplyForce(Vector3 force, float scale = 1)
    {
        vel += (force * scale);
    }

    public void SetMaxJumps(int jumps)
    {
        maxJumps = jumps;
        jumpsLeft = jumps;
    }

    public void LimitHorizonalSpeed()
    {
        if(vel2D.magnitude > maxWalkSpeed)
        {
            Vector2 limitedVel = vel2D.normalized * maxWalkSpeed;
            vel.x = limitedVel.x;
            vel.z = limitedVel.y;
        }
    }

    IEnumerator UpgradeGroundPoundToSuper()
    {
        yield return new WaitForSeconds(gpUpgradeTime);
        if (_isChargingGroundPound)
        {
            _gpIsUpgraded = true;
        }
       
    }


    #region Abilities

    public void OnFlameThrowerActivate()
    {

    }

    public void OnFlameThrowerDeactivate()
    {

    }

    #endregion

}
