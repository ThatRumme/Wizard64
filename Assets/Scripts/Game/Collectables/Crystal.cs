using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Collectable
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            transform.Rotate(new Vector3(0,50*Time.deltaTime,0));
        }
    }

    public override void PickUp()
    {
        if (!active) return;

        base.PickUp();
        EventManager.OnCrystalObtained(id);
    }
}
