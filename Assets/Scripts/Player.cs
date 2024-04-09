using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = 0.2f;


    public bool isBusy { get; private set; }
    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce = 10f;
    public float dashSpeed;
    public float dashDuration;
    
    public float dashDirection {get; private set;}
    [SerializeField] private float dashCooldown = 1f;
    private float dashTimer;
    #region States
    public PlayerStateMachine stateMachine{get; private set;}

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    #endregion

    #region Init
    protected override void Awake() 
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
    }

    protected override void Start() 
    {
        base.Start();
        //Initialize state machine to idle
        stateMachine.Initialize(idleState);
    }
    #endregion Init

    protected override void Update() 
    {
        base.Update();
        stateMachine.currentState.Update();
        checkDashInput();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }


    #region Movement
    private void checkDashInput()
    {
        dashTimer -= Time.deltaTime;
        if(IsWallDetected() ) { return; }

        if(Input.GetKeyDown(KeyCode.LeftShift) && dashTimer < 0)
        {
            dashTimer = dashCooldown;
            dashDirection = Input.GetAxisRaw("Horizontal");

            if(dashDirection == 0)
            {
                dashDirection = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }
    #endregion Movement

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
