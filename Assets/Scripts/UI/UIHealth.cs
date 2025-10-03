using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHealth : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.PlayerHealthUpdated += UpdateHealth;
    }

    void OnDestroy()
    {
        EventManager.PlayerHealthUpdated -= UpdateHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UpdateHealth(int currentHealth)
    {
        healthText.SetText(currentHealth.ToString());
    }
}
