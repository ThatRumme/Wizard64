using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : Ability
{

    private bool isInUse = false;
    private bool usedMidAir = false;

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
            
        }
    }
    public override void Deactivate()
    {
        if(isInUse)
        {
            base.Deactivate();
            if (usedMidAir)
            {
                playerMovement.OnFlameThrowerActivate();
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
    }
}
