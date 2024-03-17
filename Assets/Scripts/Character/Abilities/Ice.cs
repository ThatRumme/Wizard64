using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Ability
{

    public AbilityHitBox HitBox;

    public float maxDuration = 3f;
    public float maxRange = 5f;

    public GameObject iceDecal;

    public Vector3[] raycasts;

    public ParticleSystem particleEffect;
    

    public override bool Activate()
    {
        base.Activate();

        //Layermask everything except these layers
        int layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;


        for(int i = 0; i < raycasts.Length; ++i)
        {

            Vector3 dir = raycasts[i];
            RaycastHit hit;
            Physics.Raycast(transform.position, Camera.main.transform.TransformDirection(dir), out hit, maxRange, layerMask);
            if (hit.collider)
            {
                Debug.Log(hit.collider.gameObject.name);
                GameObject decal = Instantiate(iceDecal, hit.point, Quaternion.Euler(90, 0, 0));
            }
        }

        for(int i = 0; i < HitBox.enemiesInTrigger.Count; ++i)
        {
            HitBox.enemiesInTrigger[i].Freeze();
        }

        particleEffect.Play();

        return true;
    }
    public override void Deactivate()
    {
        base.Deactivate();
    }

    public override void SwitchOn()
    {
        base.SwitchOn();
    }

    public override void SwitchOff()
    {
        base.SwitchOff();
    }

}
