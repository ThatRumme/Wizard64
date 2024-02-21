using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHitBoxPivot : MonoBehaviour
{

    public MouseLook cameraPivot;
    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.identity * Quaternion.Euler(cameraPivot.transform.eulerAngles.x, 0, 0);
    }
}
