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
    long lastAttackTime = 0;

    protected override void Start()
    {
        base.Start();
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
            if (distanceToPlayer <= attackRange && DateTime.Now.Ticks > lastAttackTime + attackDelay)
            {
                Attack();
                lastAttackTime = DateTime.Now.Ticks;
            }
        } 
        
    }
}
