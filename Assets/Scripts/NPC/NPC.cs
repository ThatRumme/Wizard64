using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateToFacePlayer()
    {
        if (GameManager.Instance.player)
        {
            Vector3 direction = GameManager.Instance.player.transform.position - transform.position;
            direction.y = 0;

            TweenRotationToTarget(direction);
        }
    }

    void TweenRotationToTarget(Vector3 dir)
    {
        transform.DORotateQuaternion(Quaternion.LookRotation(dir), 0.25f).SetEase(Ease.Linear);
    }
}
