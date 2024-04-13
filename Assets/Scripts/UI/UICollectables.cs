using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICollectables : MonoBehaviour
{

    public static int maxRunes = 5;
    public static int maxCrystals = 100;

    public static int runeCount = 0;
    public static int crystalCount = 0;
   
    public TextMeshProUGUI runesText;
    public TextMeshProUGUI crystalText;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.CrystalObtained += OnCrystalObtained;
        EventManager.RuneObtained += OnRuneOptained;
        CountValuesFromSaveGame();
        UpdateValues();
    }


    private void OnDestroy()
    {
        EventManager.CrystalObtained -= OnCrystalObtained;
        EventManager.RuneObtained -= OnRuneOptained;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CountValuesFromSaveGame()
    {
        int totalRunes = 0;
        int totalCrystals = 0;

        PlayerData playerData = SaveGame.Instance.playerData;
        for(int i = 0; i < playerData.levels.Count; i++)
        {
            totalCrystals += playerData.levels[i].collectedCrystals.Count;
            totalRunes += playerData.levels[i].collectedRunes.Count;
        }

        runeCount = totalRunes;
        crystalCount = totalCrystals;
    }

    void OnCrystalObtained(int id)
    {
        crystalCount++;
        UpdateValues();
    }

    void OnRuneOptained(int id)
    {
        runeCount++;
        UpdateValues();
    }

    void UpdateValues()
    {
        runesText.SetText(runeCount.ToString() + "/" + maxRunes.ToString());
        crystalText.SetText(crystalCount.ToString() + "/" + maxCrystals.ToString());
    }
}
