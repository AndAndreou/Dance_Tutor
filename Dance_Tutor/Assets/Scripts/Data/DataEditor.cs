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

    /// <summary>
    /// Load all data from json file
    /// </summary>
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
            gameData.Users = new List<User>();
        }
    }

    /// <summary>
    /// Save all date in json data file
    /// </summary>
    public static void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(gameData);
        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
    }

    /// <summary>
    /// Save the last chorography result in separate text file
    /// </summary>
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

    /// <summary>
    /// Add new user in data if your email is unique
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="dateOfBirth"></param>
    /// <param name="sex"></param>
    /// <param name="expirience"></param>
    /// <param name="country"></param>
    /// <returns></returns>
    public static User AddNewUser(string name, string email, string dateOfBirth, Sex sex, Experience expirience, string country)
    {
        User newUser = null;

        if (FindUser(email) == null) 
        {
            newUser = new User(name, email, dateOfBirth, sex, expirience, country);
            gameData.Users.Add(newUser);
        }

        return newUser;
    }

    public static User FindUser(string email)
    {
        User user = null;
        foreach (User u in gameData.Users)
        {
            if (u.email == email)
            {
                user = u;
                break;
            }
        }

        return user;
    }
}

public struct GameData
{
    public Skeleton.StyleWord maxStyleWords;
    public List<User> Users;
}
