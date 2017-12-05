using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataEditor : MonoBehaviour {

    public static GameData gameData;

    private static string gameDataProjectFilePath = "/StreamingAssets/data.json";
    private static string resultsDataProjectFilePath = "/StreamingAssets/results.txt";

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
            gameData.maxStyleWords = new Skeleton.StyleWord();
        }
    }

    public static void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(gameData);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
    }

    public static void SaveResultsData()
    {
        string results ="";
        int itemPerLine = 10;
        int counter = 0;

        // Write style word
        results += "---StyleWord---\r\n";
        foreach(float s in WordsManager.styleWordResults)
        {
            results += s + ", ";

            counter++;
            if(counter>= itemPerLine)
            {
                results += "\r\n";
                counter = 0;
            }
        }

        counter = 0;

        // Write motionWord
        results += "\r\n\r\n---MotionWord---\r\n";
        List<Vector3[]> motionWord = WordsManager.motionWordResults;
        Skeleton skeleton = WordsManager.allCharCotrollers[0].skeleton;
        for (int i = 0; i < motionWord.Count; i++) // Motion words
        {
            for (int j = 0; j < motionWord[i].Length; j++) // Joint 
            {
                results += skeleton.joints[j].GetJointName() + "" + motionWord[i][j] + ", ";

                counter++;
                if (counter >= itemPerLine)
                {
                    results += "\r\n";
                    counter = 0;
                }
            }
        }

        string filePath = Application.dataPath + resultsDataProjectFilePath;
        File.WriteAllText(filePath, results);

        Debug.Log("+++++Save Results+++++");
    }
}

public struct GameData
{
    public Skeleton.StyleWord maxStyleWords;
}
