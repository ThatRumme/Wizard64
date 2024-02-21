using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHitBox : MonoBehaviour
{

    public List<Enemy> enemiesInTrigger = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy != null && !enemiesInTrigger.Contains(enemy))
            {
                enemiesInTrigger.Add(enemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && enemiesInTrigger.Contains(enemy))
            {;
                enemiesInTrigger.Remove(enemy);
            }
        }
    }
}
