using UnityEngine;
using UnityEngine.Events;


public class ColliderTrigger : Trigger
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeActivate();
        }
    }
}

