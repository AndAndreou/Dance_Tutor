using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectAnimationSceneController : MonoBehaviour {

    //public static Country[] countries;

    [Header("UI Compnents")]
    public GameObject uiLoadingPanel;
    public Text uiLoadingTxt;

    //events
    [HideInInspector]
    public UnityEvent ChangeSelectedCountryEvent;
    [HideInInspector]
    public UnityEvent ChangeSelectedAnimationClipEvent;

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
        DontDestroyOnLoad(this);

        if (FindObjectOfType<DataEditor>() == null)
        {
            DataEditor.LoadGameData();
        }

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

    public static int GoToNextCountry()
    {
        DataEditor.selectedContryID++;
        if (DataEditor.selectedContryID > (DataEditor.countries.Length - 1))
        {
            DataEditor.selectedContryID = 0;
        }

        ChangeSelectedCounty();

        return DataEditor.selectedContryID;
    }

    public static int GoToPrevCountry()
    {
        DataEditor.selectedContryID--;
        if (DataEditor.selectedContryID < 0)
        {
            DataEditor.selectedContryID = DataEditor.countries.Length - 1;
        }

        ChangeSelectedCounty();

        return DataEditor.selectedContryID;
    }

    private static void ChangeSelectedCounty()
    {
        DataEditor.selectedCountryAllAnimations = DataEditor.countries[DataEditor.selectedContryID].GetAllAnimations();
        DataEditor.selectedAnimationClipID = 0;

        instance.ChangeSelectedCountryEvent.Invoke();
        ChangeSelectedAnimationClip();
    }

    public static int GoToNextAnimationClip()
    {
        DataEditor.selectedAnimationClipID++;
        if (DataEditor.selectedAnimationClipID > (DataEditor.selectedCountryAllAnimations.Length-1))
        {
            DataEditor.selectedAnimationClipID = 0;
        }

        ChangeSelectedAnimationClip();

        return DataEditor.selectedAnimationClipID;
    }

    public static int GoToPrevAnimationClip()
    {
        DataEditor.selectedAnimationClipID--;
        if (DataEditor.selectedAnimationClipID < 0)
        {
            DataEditor.selectedAnimationClipID = DataEditor.selectedCountryAllAnimations.Length - 1;
        }

        ChangeSelectedAnimationClip();

        return DataEditor.selectedAnimationClipID;
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