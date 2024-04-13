using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        if(!_isFrozen)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectPlayerRange)
            {
                SetTargetPosition(player.transform.position);
            }
        }
    }

    protected virtual void AttemptAttack()
    {
        if (!_isFrozen)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            bool allowAttackTimeWise = attackTimer >= attackDelay;
            if(!allowAttackTimeWise)
            {
                attackTimer += Time.deltaTime;
            }
            
            if (distanceToPlayer <= attackRange && allowAttackTimeWise)
            {
                Attack();
                attackTimer = 0;
            }
        } 
        
    }
}
