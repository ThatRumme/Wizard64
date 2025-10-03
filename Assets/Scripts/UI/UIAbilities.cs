using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilities : MonoBehaviour
{

    public Color[] abilityColors;
    public Image abilityImage;

    public GameObject[] manaIcons;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AbilitySwitch += OnAbilitySwitched;
        EventManager.ManaUpdated += OnManaUpdated;
    }

    private void OnDestroy()
    {
        EventManager.AbilitySwitch -= OnAbilitySwitched;
        EventManager.ManaUpdated -= OnManaUpdated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAbilitySwitched(int ability)
    {
        abilityImage.color = abilityColors[ability];
    }

    void  OnManaUpdated(int mana)
    {
        for(int i = 0; i < manaIcons.Length; i++)
        {
            manaIcons[i].SetActive(i < mana);
        }
    }
}
