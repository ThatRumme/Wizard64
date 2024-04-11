using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
            {
                enemiesInTrigger.Remove(enemy);
            }
        }
    }

    public void CheckForEnemiesDeleted()
    {

        List<Enemy> enemiesToDelete = new List<Enemy>();

        for (int i = 0; i < enemiesInTrigger.Count; ++i)
        {
            if (enemiesInTrigger[i] == null)
            {
                enemiesToDelete.Add(enemiesInTrigger[i]);
            }
        }

        if(enemiesToDelete.Count > 0)
        {
            for (int i = 0; i < enemiesToDelete.Count; ++i)
            {
                enemiesInTrigger.Remove(enemiesToDelete[i]);
            }
        }
    }
}
