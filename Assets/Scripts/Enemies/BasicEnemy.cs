using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BasicEnemy : Enemy
{

   
    public float detectPlayerRange = 10;
    public float attackRange = 1;
    public float attackDelay = 1;
    float attackTimer = 0;

    protected override void Start()
    {
        base.Start();
        attackTimer = attackDelay;
    }

    protected override void Update()
    {
       base.Update();
       Move();
       AttemptAttack();
    }

    protected virtual void Move()
    {
        if (!_isFrozen)
        {
            if (IsWithinChaseRangeOfPlayer())
            {
                
                isChasingPlayer = true;
                SetTargetPosition(player.transform.position);
            }
            else
            {
                isChasingPlayer = false;
            }
        }
    }

    protected virtual void AttemptAttack()
    {
        if (!_isFrozen)
        {
            bool allowAttackTimeWise = attackTimer >= attackDelay;
            if(!allowAttackTimeWise)
            {
                attackTimer += Time.deltaTime;
            }
            
            if (IsWithinAttackRangeOfPlayer() && allowAttackTimeWise)
            {
                Attack();
                attackTimer = 0;
            }
        } 
        
    }


    protected bool IsWithinChaseRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= detectPlayerRange;
    }
    protected bool IsWithinAttackRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer <= attackRange;
    }
}
