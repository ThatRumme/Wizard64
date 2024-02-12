using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : Ability
{

    //public vars
    public float maxDuration;


    //private vars
    private bool isInUse = false;
    private bool usedMidAir = false;

    private int currentPressId = 0;

    public override void Activate()
    {
        if (!isInUse && AllowUse())
        {
            isInUse = true;

            base.Activate();


            usedMidAir = !playerMovement.IsGrounded();
            if (usedMidAir)
            {
                playerMovement.OnFlameThrowerActivate();
            }

            StartCoroutine(TurnOffTimer());
            
        }
    }
    public override void Deactivate()
    {
        if (isInUse) { 
            base.Deactivate();
            if (usedMidAir)
            {
                playerMovement.OnFlameThrowerDeactivate();
            }
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
        
    }

    protected override void ResetValues()
    {
        isInUse = false;
        base.ResetValues();
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
