using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PatrolingBasicEnemy : BasicEnemy
{

    public Transform[] pathPoints;
    private int currentPointIdx = 0;
    private PatrolingPattern patrolPattern;
    public bool goForward = true;
  
    protected override void Start()
    {
        base.Start();

        StartAtPoint(0);
    }

    protected override void Update()
    {
       base.Update();
    }

    protected override void OnArrivingAtTargetPosition()
    {
        base.OnArrivingAtTargetPosition();

        NextTargetPoint();
        
    }

    private void NextTargetPoint()
    {
        if (IsMoving()) return;

        switch (patrolPattern)
        { 
            case PatrolingPattern.ForthAndBack:
                {
                    if (goForward)
                    {
                        currentPointIdx++;
                        if (currentPointIdx == pathPoints.Length)
                        {
                            goForward = false;
                            currentPointIdx = pathPoints.Length - 2;
                        }
                    }
                    else
                    {
                        currentPointIdx--;
                        if (currentPointIdx == pathPoints.Length)
                        {
                            goForward = true;
                            currentPointIdx = 1;
                        }
                    }
                    
                    break;
                }
            case PatrolingPattern.Random:
                {
                    List<int> indexes= new List<int>();
                    for(int i = 0; i < pathPoints.Length; i++)
                    {
                        if(i != currentPointIdx)
                            indexes.Add(i);
                    }

                    currentPointIdx = UnityEngine.Random.Range(0, indexes.Count);
                    break;
                }
            default: //RoundAndRound
                {
                    currentPointIdx++;
                    if (currentPointIdx == pathPoints.Length)
                    {
                        currentPointIdx = 0;
                    }
                    break;
                }

                
        }
        if(pathPoints.Length > 0)
        {
            SetTargetPosition(pathPoints[currentPointIdx].position);
        }
        
    }

    private void StartAtPoint(int idx)
    {
        currentPointIdx = idx;
        if (pathPoints.Length > 0)
        {
            SetTargetPosition(pathPoints[0].position);
        }
    }

    enum PatrolingPattern
    {
        RoundAndRound,
        ForthAndBack,
        Random
    }

}
