using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lasTimeAttacked;
    private float comboWindow = 1f;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0; // need this to fix attack direction bug
        if (comboCounter > 2 || Time.time >= lasTimeAttacked + comboWindow) { comboCounter = 0; }
        player.anim.SetInteger("ComboCounter", comboCounter);
        //make attack go in direction of input
        float attackDir = player.facingDir;
        if(xInput != 0)
        {
            attackDir = xInput;
        }

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        stateTimer = .1f;
        
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .15f);
        comboCounter ++;
        lasTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer <0)
        {
            player.ZeroVelocity();
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
