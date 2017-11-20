using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataEditor : MonoBehaviour {

    public static GameData gameData;

    private static string gameDataProjectFilePath = "/StreamingAssets/data.json";

    private static DataEditor _instance = null;
    public static DataEditor instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (DataEditor)FindObjectOfType(typeof(DataEditor));
                if (_instance == null)
                    _instance = (new GameObject("GameManager")).AddComponent<DataEditor>();
            }
            return _instance;
        }
    }

    public static void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(dataAsJson);
        }
        else
        {
            gameData = new GameData();
        }
    }

    public static void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson(gameData);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
    }
}

public struct GameData
{
    public float maxNumInStyleWords;
}
