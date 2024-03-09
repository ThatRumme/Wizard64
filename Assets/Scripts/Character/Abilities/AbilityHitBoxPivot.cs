using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHitBoxPivot : MonoBehaviour
{

    public MouseLook cameraPivot;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity * Quaternion.Euler(cameraPivot.transform.eulerAngles.x -10, cameraPivot.transform.eulerAngles.y, 0);
    }
}
