using System;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {
    public UnityEvent[] activation;
    public UnityEvent[] deActivation;
    public bool once = false; //Only trigger once and only on enter
    bool activated;

    private void Start()
    {
        DeActivate();
    }
    public virtual void Activate()
    {
        if (activated && once)
            return;

        activated = true;

        foreach (UnityEvent e in activation)
        {
            e.Invoke();
        }
    }

    public virtual void DeActivate()
    {
        if (once && activated)
            return;

        foreach (UnityEvent e in deActivation)
        {
            e.Invoke();
        }
    }
}

