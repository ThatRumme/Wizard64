using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public string levelId = "DefaultLevelId_01";
    public string displayName = "TestLevel";

    public Crystal[] crystals;
    public Rune[] runes;

    // Start is called before the first frame update
    void Start()
    {
        OnLoad();
        EventManager.RuneObtained += OnPickupRune;
        EventManager.CrystalObtained += OnPickupCrystal;
    }

    private void OnDestroy()
    {
        EventManager.RuneObtained -= OnPickupRune;
        EventManager.CrystalObtained -= OnPickupCrystal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLoad()
    {
        LevelData levelData =  SaveGame.Instance.GetLevelData(levelId);
        foreach(int i in levelData.collectedRunes)
        {
            if (i >= runes.Length) continue;

            runes[i].DisableObject();
        }

        for(int i = 0; i < runes.Length; i++)
        {
            runes[i].id = i;
        }

        foreach (int i in levelData.collectedCrystals)
        {
            if (i >= runes.Length) continue;

            crystals[i].DisableObject();
        }

        for (int i = 0; i < crystals.Length; i++)
        {
            crystals[i].id = i;
        }
    }

    public void OnPickupRune(int id)
    {
        SaveGame.Instance.OptainedRune(levelId, id);
    }

    public void OnPickupCrystal(int id)
    {
        SaveGame.Instance.OptainedCrystal(levelId, id);
    }
}
