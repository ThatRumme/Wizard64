using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMeshOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if(mr != null)
        {
            mr.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
