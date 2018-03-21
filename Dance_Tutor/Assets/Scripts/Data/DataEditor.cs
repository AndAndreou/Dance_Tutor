using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataEditor : MonoBehaviour {
    #region Thresholds
        // Thresholds Amateur
        private static float styleWordThresholdAmateur = 5f;
        private static float motionWordThresholdAmateur = 65f;
        private static float uiMotionFeedBackThresholdAmateur = 5f;
        private static float lmaComponentsUpperThresholdAmateur = 5f;
        private static float lmaComponentsLowerThresholdAmateur = 1f;

        // Thresholds Intermediate
        private static float styleWordThresholdIntermediate = 4f;
        private static float motionWordThresholdIntermediate = 55f;
        private static float uiMotionFeedBackThresholdIntermediate = 4f;
        private static float lmaComponentsUpperThresholdIntermediate = 4f;
        private static float lmaComponentsLowerThresholdIntermediate = 2f;

        // Thresholds Expert
        private static float styleWordThresholdExpert = 3f;
        private static float motionWordThresholdExpert = 40f;
        private static float uiMotionFeedBackThresholdExpert = 3f;
        private static float lmaComponentsUpperThresholdExpert = 3.5f;
        private static float lmaComponentsLowerThresholdExpert = 2.5f;
    #endregion

    public static float styleWordThreshold
    { get
        {   if (selectedUser == null) return 0;

            switch (selectedUser.expirience)
            {
                case Experience.Beginner:
                    return styleWordThresholdAmateur;
                case Experience.Intermediate:
                    return styleWordThresholdIntermediate;
                case Experience.Expert:
                    return styleWordThresholdExpert;
                default:
                    return styleWordThresholdAmateur; ;
            }
        }
        private set { }
    }
    public static float motionWordThreshold
    {
        get
        {
            if (selectedUser == null) return 0;
            switch (selectedUser.expirience)
            {
                case Experience.Beginner:
                    return motionWordThresholdAmateur;
                case Experience.Intermediate:
                    return motionWordThresholdIntermediate;
                case Experience.Expert:
                    return motionWordThresholdExpert;
                default:
                    return motionWordThresholdAmateur; ;
            }
        }
        private set { }
    }
    public static float uiMotionFeedBackThreshold
    {
        get
        {
            if (selectedUser == null) return 0;
            switch (selectedUser.expirience)
            {
                case Experience.Beginner:
                    return uiMotionFeedBackThresholdAmateur;
                case Experience.Intermediate:
                    return uiMotionFeedBackThresholdIntermediate;
                case Experience.Expert:
                    return uiMotionFeedBackThresholdExpert;
                default:
                    return uiMotionFeedBackThresholdAmateur; ;
            }
        }
        private set { }
    }
    public static float lmaComponentsUpperThreshold
    {
        get
        {
            if (selectedUser == null) return 0;
            switch (selectedUser.expirience)
            {
                case Experience.Beginner:
                    return lmaComponentsUpperThresholdAmateur;
                case Experience.Intermediate:
                    return lmaComponentsUpperThresholdIntermediate;
                case Experience.Expert:
                    return lmaComponentsUpperThresholdExpert;
                default:
                    return lmaComponentsUpperThresholdAmateur; ;
            }
        }
        private set { }
    }
    public static float lmaComponentsLowerThreshold
    {
        get
        {
            if (selectedUser == null) return 0;
            switch (selectedUser.expirience)
            {
                case Experience.Beginner:
                    return lmaComponentsLowerThresholdAmateur;
                case Experience.Intermediate:
                    return lmaComponentsLowerThresholdIntermediate;
                case Experience.Expert:
                    return lmaComponentsLowerThresholdExpert;
                default:
                    return lmaComponentsLowerThresholdAmateur; ;
            }
        }
        private set { }
    }

    public static GameData gameData;

    public static Country[] countries;

    [HideInInspector]
    public static int selectedContryID;
    [HideInInspector]
    public static int selectedAnimationClipID;
    [HideInInspector]
    public static AnimationClip[] selectedCountryAllAnimations;
    [HideInInspector]
    public static User selectedUser;

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
            if (gameData.Users != null)
            {
                selectedUser = gameData.Users[0];
            }
        }
        else
        {
            gameData = new GameData();
            gameData.maxStyleWords = new Skeleton.StyleWord();
            gameData.Users = new List<User>();
        }

        // Load countries and animations
        LoadCountries();
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
        //foreach(float s in WordsManager.styleWordResults)
        foreach (float s in WordsManagerWithSync.styleWordResults)
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


        //// Write motionWord
        //results += "\r\n\r\n---MotionWord---\r\n";
        //List<Vector3[]> motionWord = WordsManager.motionWordResults;
        //Skeleton skeleton = WordsManager.allCharCotrollers[0].skeleton;
        //for (int i = 0; i < motionWord.Count; i++) // Motion words
        //{
        //    for (int j = 0; j < motionWord[i].Length; j++) // Joint 
        //    {
        //        results += skeleton.joints[j].GetJointName() + "" + motionWord[i][j] + ", ";

        //        counter++;
        //        if (counter >= itemPerLine)
        //        {
        //            results += "\r\n";
        //            counter = 0;
        //        }
        //    }
        //}

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
    public static User AddNewUser(string photoName, string name, string email, string dateOfBirth, Sex sex, Experience expirience, string country)
    {
        User newUser = null;

        if (FindUser(email) == null) 
        {
            newUser = new User(photoName, name, email, dateOfBirth, sex, expirience, country);
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

    public static void SetSelectedUser(string email)
    {
        selectedUser = FindUser(email);
    }

    public static User GetSelectedUser()
    {
        return selectedUser;
    }

    /// <summary>
    /// Load all counties and animation clips
    /// </summary>
    private static void LoadCountries()
    {
        string[] countriesFoldersPaths = Directory.GetDirectories(Application.dataPath + "/Resources/Animations/Countries"); // Get the paths of all folder in this path
        string[] countriesFoldersNames = new string[countriesFoldersPaths.Length];
        for (int i = 0; i < countriesFoldersPaths.Length; i++)
        {
            var d = new DirectoryInfo(countriesFoldersPaths[i]);
            countriesFoldersNames[i] = d.Name; // get only the name of folders
        }

        countries = new Country[countriesFoldersNames.Length];

        for (int i = 0; i < countriesFoldersNames.Length; i++)
        {
            countries[i].name = countriesFoldersNames[i];
            countries[i].flag = Resources.LoadAll<Sprite>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Flag")[0];
            countries[i].background = Resources.LoadAll<Sprite>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\BackgroundImage")[0];
            countries[i].beginnerAnimations = Resources.LoadAll<AnimationClip>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Clips\\Beginner");
            countries[i].intermediateAnimations = Resources.LoadAll<AnimationClip>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Clips\\Intermediate");
            countries[i].expertAnimations = Resources.LoadAll<AnimationClip>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Clips\\Expert");
        }

        selectedContryID = countries.Length / 2;
        selectedCountryAllAnimations = countries[selectedContryID].GetAllAnimations();
        selectedAnimationClipID = 0;
    }


    /// <summary>
    /// Get the country with some id. If dont give id then get the selected country
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public static Country GetCountry(int ID = -1)
    {
        if (ID < 0)
        {
            ID = selectedContryID;
        }

        return countries[ID];
    }

    /// <summary>
    /// Get the animation with some id. If dont give id then get the selected animation
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public static AnimationClip GetAnimationClip(int ID = -1)
    {
        if (ID < 0)
        {
            ID = selectedAnimationClipID;
        }

        return selectedCountryAllAnimations[ID];
    }

    /// <summary>
    /// Get the experience of animation with ID from selected country
    /// </summary>
    /// <param name="animationId"></param>
    /// <returns></returns>
    public static Experience GetExperineceChorographyOfSelectedCountry(int animationId = -1)
    {
        if (animationId < 0)
        {
            animationId = selectedAnimationClipID;
        }

        int beginnerAnimationLength = countries[selectedContryID].beginnerAnimations.Length;
        if (animationId < beginnerAnimationLength)
        {
            return Experience.Beginner;
        }
        else if(animationId < (beginnerAnimationLength + countries[selectedContryID].intermediateAnimations.Length))
        {
            return Experience.Intermediate;
        }
        else
        {
            return Experience.Expert;
        }
    }

    /// <summary>
    /// Get the last dance of history of selected user
    /// </summary>
    /// <returns></returns>
    public static AnimationClip GetLastAnimationClipInHistoryOfSelectedPlayer()
    {
        AnimationClip[] allAnimationClip = Resources.LoadAll<AnimationClip>("Animations");
        foreach (AnimationClip clip in allAnimationClip)
        {
            if(clip.name == selectedUser.history[selectedUser.history.Count - 1].danceName)
            {
                return clip;
            }
        }
        return null;
    }

    public static int GetAnimationsClipsLength()
    {
        return selectedCountryAllAnimations.Length;
    }

    /// <summary>
    /// Save dance(selected clip name, selected user expirience, motion and style words results from WordsManager) as history in selected user
    /// </summary>
    public static void SaveWords()
    {
        //selectedUser.AddDanceHistory(GetAnimationClip().name, selectedUser.expirience, WordsManager.motionWordResults, WordsManager.styleWordResults);
        string clipName;
        if (GameManager.instance.useDataFromUser)
        {
            clipName = GetAnimationClip().name;
        }
        else
        {
            clipName = "Evaki3_3";
        }
        selectedUser.AddDanceHistory(clipName, GetExperineceChorographyOfSelectedCountry(), WordsManagerWithSync.motionWordResults, WordsManagerWithSync.styleWordResults,WordsManagerWithSync.LMARadarGraphResults,WordsManagerWithSync.motionDataToUIAvatarResults, WordsManagerWithSync.wordsTimers);
        UpdateSelectedUser();
    }

    /// <summary>
    /// Update selected user to list of users
    /// </summary>
    public static void UpdateSelectedUser()
    {
        for(int i=0; i < gameData.Users.Count; i++) 
        {
            if (gameData.Users[i].email == selectedUser.email)
            {
                gameData.Users[i] = selectedUser;
                break;
            }
        }
    }

    /// <summary>
    /// Update selected user to list of users
    /// </summary>
    public static void UpdateSelectedUser(string photoName, string name, string dateOfBirth, Sex sex, Experience expirience, string country)
    {
        selectedUser.photoName = photoName;
        selectedUser.name = name;
        selectedUser.dateOfBirth = dateOfBirth;
        selectedUser.sex = sex;
        selectedUser.expirience = expirience;
        selectedUser.country = country;

        for (int i = 0; i < gameData.Users.Count; i++)
        {
            if (gameData.Users[i].email == selectedUser.email)
            {
                gameData.Users[i] = selectedUser;
                break;
            }
        }
    }
}

public struct GameData
{
    public Skeleton.StyleWord maxStyleWords;
    public List<User> Users;
}

public struct Country
{
    public string name;
    public Sprite flag;
    public Sprite background;
    public AnimationClip[] beginnerAnimations;
    public AnimationClip[] intermediateAnimations;
    public AnimationClip[] expertAnimations;

    public AnimationClip[] GetAllAnimations()
    {
        AnimationClip[] allAnimations = new AnimationClip[beginnerAnimations.Length + intermediateAnimations.Length + expertAnimations.Length];
        Array.Copy(beginnerAnimations, allAnimations, beginnerAnimations.Length);
        Array.Copy(intermediateAnimations, 0, allAnimations, beginnerAnimations.Length, intermediateAnimations.Length);
        Array.Copy(expertAnimations, 0, allAnimations, beginnerAnimations.Length + intermediateAnimations.Length, expertAnimations.Length);

        return allAnimations;
    }

    public Experience GetAnimationExpirience(AnimationClip animationClip)
    {
        foreach(AnimationClip clip in beginnerAnimations)
        {
            if (clip == animationClip)
            {
                return Experience.Beginner;
            }
        }

        foreach (AnimationClip clip in intermediateAnimations)
        {
            if (clip == animationClip)
            {
                return Experience.Intermediate;
            }
        }

        // Else
        return Experience.Expert;
    }
}