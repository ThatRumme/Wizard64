using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilities : MonoBehaviour
{

    public Color[] abilityColors;
    public Image abilityImage;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AbilitySwitch += OnAbilitySwitched;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAbilitySwitched(int ability)
    {
        abilityImage.color = abilityColors[ability];
    }
}
