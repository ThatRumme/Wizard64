using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EventManager : MonoBehaviour {

    //Game
    public static event Action ResetLevel;
    public static event Action ApplySettings;
    public static event Action<int> RuneObtained;
    public static event Action<int> CrystalObtained;
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

    public static void OnRuneObtained(int id)
    {
        RuneObtained?.Invoke(id);
    }

    public static void OnCrystalObtained(int id)
    {
        CrystalObtained?.Invoke(id);
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
