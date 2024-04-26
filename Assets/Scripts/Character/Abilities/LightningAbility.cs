using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAbility : Ability
{

    public int maxRange = 100;
    public int damage = 10;


    public GameObject lightningPrefab;
    public override bool Activate()
    {
        base.Activate();

        //Layermask everything except these layers
        int layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;

        Transform camTransform = Camera.main.transform;

        Vector3 endPos = transform.position + (camTransform.TransformDirection(Vector3.forward) * maxRange);
        Transform endTransform = null;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        Physics.Raycast(transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, maxRange, layerMask);
        if (hit.collider)
        {
            bool hitInteractableObject = false;
            bool hitEnemy = false;


            if (hit.collider.CompareTag("Interactable_Electric"))
            {
                hitInteractableObject = true;
                hit.collider.gameObject.GetComponent<ElectricTrigger>().Activate();
            }

            if (hit.collider.CompareTag("Enemy"))
            {
                hitEnemy = true;
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(10);
            }

            if (hitInteractableObject || hitEnemy)
            {
                endPos = hit.collider.transform.position;
                endTransform = hit.collider.transform;
            }
            else
            {
                endPos = hit.point;
            }
        }

        LightningBeam lb = Instantiate(lightningPrefab).GetComponent<LightningBeam>();
        lb.Setup(staffCrystal, endTransform, endPos);

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
