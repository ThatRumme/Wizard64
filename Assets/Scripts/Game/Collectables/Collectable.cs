using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    [HideInInspector]
    public int id;

    public GameObject obj;
    public bool active = true;

    public virtual void PickUp()
    {
        if (!active) return;
        DisableObject();
        
    }


    public void DisableObject()
    {
        obj.SetActive(false);
        active = false;
    }


}
