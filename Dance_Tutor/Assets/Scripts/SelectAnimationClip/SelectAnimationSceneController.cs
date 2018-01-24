using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectAnimationSceneController : MonoBehaviour {

    public static Country[] countries;

    [Header("UI Compnents")]
    public GameObject uiLoadingPanel;
    public Text uiLoadingTxt;

    [HideInInspector]
    public static int selectedContryID;
    [HideInInspector]
    public static int selectedAnimationClipID;

    //events
    [HideInInspector]
    public UnityEvent ChangeSelectedCountryEvent;
    [HideInInspector]
    public UnityEvent ChangeSelectedAnimationClipEvent;

    private static AnimationClip[] selectedCountryAllAnimations;

    private static SelectAnimationSceneController _instance = null;
    public static SelectAnimationSceneController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (SelectAnimationSceneController)FindObjectOfType(typeof(GameManager));
                if (_instance == null)
                    _instance = (new GameObject("SelectAnimationSceneController")).AddComponent<SelectAnimationSceneController>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        LoadCountries();

        //create event for selected country
        if (ChangeSelectedCountryEvent == null)
        {
            ChangeSelectedCountryEvent = new UnityEvent();
        }

        //create event for selectet animation clip
        if (ChangeSelectedAnimationClipEvent == null)
        {
            ChangeSelectedAnimationClipEvent = new UnityEvent();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadLevel("Gameplay");
        }
    }

    private void LoadCountries()
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

    public static int GoToNextCountry()
    {
        selectedContryID++;
        if (selectedContryID > (countries.Length - 1))
        {
            selectedContryID = 0;
        }

        ChangeSelectedCounty();

        return selectedContryID;
    }

    public static int GoToPrevCountry()
    {
        selectedContryID--;
        if (selectedContryID < 0)
        {
            selectedContryID = countries.Length - 1;
        }

        ChangeSelectedCounty();

        return selectedContryID;
    }

    private static void ChangeSelectedCounty()
    {
        selectedCountryAllAnimations = countries[selectedContryID].GetAllAnimations();
        selectedAnimationClipID = 0;

        instance.ChangeSelectedCountryEvent.Invoke();
        ChangeSelectedAnimationClip();
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

    public static int GetAnimationsClipsLength()
    {
        return selectedCountryAllAnimations.Length;
    }

    public static int GoToNextAnimationClip()
    {
        selectedAnimationClipID++;
        if (selectedAnimationClipID > (selectedCountryAllAnimations.Length-1))
        {
            selectedAnimationClipID = 0;
        }

        ChangeSelectedAnimationClip();

        return selectedAnimationClipID;
    }

    public static int GoToPrevAnimationClip()
    {
        selectedAnimationClipID--;
        if (selectedAnimationClipID < 0)
        {
            selectedAnimationClipID = selectedCountryAllAnimations.Length - 1;
        }

        ChangeSelectedAnimationClip();

        return selectedAnimationClipID;
    }

    private static void ChangeSelectedAnimationClip()
    {
        instance.ChangeSelectedAnimationClipEvent.Invoke();
    }

    public void LoadLevel(string sceneName) //The name of the scene
    {

        StartCoroutine(LevelCoroutine(sceneName));
    }

    IEnumerator LevelCoroutine(string sceneName)
    {
        uiLoadingPanel.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            uiLoadingTxt.text = (int)(async.progress * 100) + "%";
            yield return null;

        }
    }
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
}