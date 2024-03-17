using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EventManager : MonoBehaviour {

    //Game
    public static event Action ResetLevel;
    public static event Action ApplySettings;
    public static event Action CollectableObtained;
    public static event Action<int> AbilitySwitch;
    public static event Action<int> ManaUpdated;

    public static void OnResetLevel()
    {
        ResetLevel?.Invoke();
    }

    public static void OnSettingsApplied()
    {
        ApplySettings?.Invoke();
    }

    public static void OnCollectableObtained()
    {
        CollectableObtained?.Invoke();
    }

    public static void OnAbilitySwitched(int ability)
    {
        AbilitySwitch?.Invoke(ability);
    }

    public static void OnManaUpdated(int ability)
    {
        ManaUpdated?.Invoke(ability);
    }
}
