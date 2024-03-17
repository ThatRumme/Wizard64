using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : Ability
{

    //public vars
    public float maxDuration;
    public int damage;
    public float secondsPerHit = 0.2f;
    public AbilityHitBox HitBoxForward;
    public AbilityHitBox HitBoxDownward;
    public ParticleSystem particleEffectForward;
    public ParticleSystem particleEffectDownward;


    //private vars
    private bool isInUse = false;
    private bool usedMidAir = false;

    private int currentPressId = 0;
    private float attackHitTimer = 0;

    public override bool Activate()
    {
        if (!isInUse && AllowUse())
        {
            isInUse = true;

            base.Activate();


            usedMidAir = !playerMovement.IsGrounded();
            if (usedMidAir)
            {
                playerMovement.OnFlameThrowerActivate();
                particleEffectDownward.Play();
            }
            else
            {
                particleEffectForward.Play();
            }

            StartCoroutine(TurnOffTimer());

            return true;
        }
        return false;
    }
    public override void Deactivate()
    {
        if (isInUse) { 
            base.Deactivate();
            if (usedMidAir)
            {
                playerMovement.OnFlameThrowerDeactivate();
            }
            particleEffectDownward.Stop();
            particleEffectForward.Stop();
        }
    }

    public override void SwitchOn()
    {
        base.SwitchOn();
    }

    public override void SwitchOff()
    {
        base.SwitchOff();
    }

    private void Update()
    {
        if (isInUse)
        {
            attackHitTimer += Time.deltaTime;
            if(attackHitTimer > secondsPerHit)
            {
                attackHitTimer -= secondsPerHit;
                AttackEnemies();
            }
        }
    }

    protected override void ResetValues()
    {
        isInUse = false;
        base.ResetValues();
    }

    private void AttackEnemies()
    {

        AbilityHitBox hitBox = usedMidAir ? HitBoxDownward : HitBoxForward;

        for (int i = 0; i < hitBox.enemiesInTrigger.Count; ++i)
        {
            if (hitBox.enemiesInTrigger[i] != null)
            {
                hitBox.enemiesInTrigger[i].TakeDamage(damage);
            }
            
        }
        hitBox.CheckForEnemiesDeleted();

        
    }

    IEnumerator TurnOffTimer()
    {
        currentPressId++;
        int pressId = currentPressId;
        yield return new WaitForSeconds(maxDuration);
        if(currentPressId == pressId)
        {
            Deactivate();
        }
    }
}
