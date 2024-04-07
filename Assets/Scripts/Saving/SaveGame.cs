using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Json;

[System.Serializable]
public class PlayerData
{
    public List<LevelData> levels;
}

[System.Serializable]
public class LevelData
{
    public string name = "DefaultLevelData";
    public List<int> collectedRunes = new List<int>();
    public List<int> collectedCrystals = new List<int>();

    public LevelData(string id)
    {
        this.name = id;
    }
}

public class SaveGame : Singleton<SaveGame>
{
    public PlayerData playerData;

    string saveFilePath;

    void Awake()
    {
       
        LoadData();
    }

    private string GetFilePath()
    {
        saveFilePath = Application.persistentDataPath + "/PlayerData.json";
        return saveFilePath;
    }

    public void SaveData()
    {
        string savePlayerData = JsonUtility.ToJson(playerData);
        Debug.Log(savePlayerData);

        File.WriteAllText(saveFilePath, savePlayerData);

    }

    public void LoadData()
    {
        if (saveFilePath == null)
        {
            GetFilePath();
        }

        if (File.Exists(saveFilePath))
        {
            string loadPlayerData = File.ReadAllText(saveFilePath);
            playerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
        }
        else
        {
            playerData = new PlayerData();
            playerData.levels = new List<LevelData>();
        }
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
    }

    LevelData GetLevelDataFromId(string id)
    {
        foreach(LevelData levelData in playerData.levels)
        {
            if(levelData.name == id)
            {
                return levelData;
            }
        }
        return null;
    }

    public void OptainedRune(string level, int id)
    {

        LevelData levelData = GetLevelDataFromId(level);


        if (levelData == null)
        {
            levelData = new LevelData(level);
            playerData.levels.Add(levelData);
        }

        if (!levelData.collectedRunes.Contains(id))
        {
            levelData.collectedRunes.Add(id);
            SaveData();
        }
    }

    public void OptainedCrystal(string level, int id)
    {

        LevelData levelData = GetLevelDataFromId(level);

        if (levelData == null)
        {
            levelData = new LevelData(level);
            playerData.levels.Add(levelData);
        }

        if (!levelData.collectedCrystals.Contains(id))
        {
            levelData.collectedCrystals.Add(id);
            SaveData();
        }
    }

    public LevelData GetLevelData(string level)
    {

        LevelData levelData = GetLevelDataFromId(level);

        if (levelData == null)
        {
            levelData = new LevelData(level);
            playerData.levels.Add(levelData);
        }

        return levelData;
    }
}
