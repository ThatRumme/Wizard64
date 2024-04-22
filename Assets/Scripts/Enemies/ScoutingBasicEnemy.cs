using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScoutingBasicEnemy : BasicEnemy
{

  
    protected override void Start()
    {
        base.Start();
        SetRotationAngle();
    }

    protected override void Update()
    {
       base.Update();
    }

    protected override void OnArrivingAtTargetPosition()
    {
        base.OnArrivingAtTargetPosition();

        SetRotationAngle();    }


    private void SetRotationAngle()
    {
        if (IsMoving() || isChasingPlayer) return;

        
        Vector3 randomDir = new Vector3(UnityEngine.Random.Range(-10f, 10f), 0, UnityEngine.Random.Range(-10f, 10f));
        Vector3 randomLookAtPos = transform.position + randomDir;

        //Debug.Log("Look at " + randomDir);

        SetRotationTowardsTarget(randomLookAtPos);
        StartCoroutine(WaitForRotationDelay());
    }

    IEnumerator WaitForRotationDelay()
    {
        yield return new WaitForSeconds(1.5f);
        SetRotationAngle();
        
    }


}
