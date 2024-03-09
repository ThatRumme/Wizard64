using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EventManager : MonoBehaviour {

    //Game
    public static event Action ResetLevel;
    public static event Action Goal;
    public static event Action ApplySettings;
    public static event Action CollectableObtained;
    public static event Action<int> AbilitySwitch;

    public static void OnResetLevel()
    {
        Debug.Log("[EventManager] Reset Level!");
        ResetLevel?.Invoke();
    }

    public static void OnSettingsApplied()
    {
        Debug.Log("[EventManager] Apply Settings");
        ApplySettings?.Invoke();
    }

    public static void OnCollectableObtained()
    {
        Debug.Log("[EventManager] Collectable Obtained");
        CollectableObtained?.Invoke();
    }

    public static void OnAbilitySwitched(int ability)
    {
        Debug.Log("[EventManager] Ability Switch");
        AbilitySwitch?.Invoke(ability);
    }
}
