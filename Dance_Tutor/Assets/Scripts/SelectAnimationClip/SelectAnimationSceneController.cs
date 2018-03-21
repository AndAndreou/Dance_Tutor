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
    [Header("User")]
    public Image userphoto;
    public Text userName;

    //events
    [HideInInspector]
    public UnityEvent ChangeSelectedCountryEvent;
    [HideInInspector]
    public UnityEvent ChangeSelectedAnimationClipEvent;

    //private static SelectAnimationSceneController _instance = null;
    //public static SelectAnimationSceneController instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = (SelectAnimationSceneController)FindObjectOfType(typeof(GameManager));
    //            if (_instance == null)
    //                _instance = (new GameObject("SelectAnimationSceneController")).AddComponent<SelectAnimationSceneController>();
    //        }
    //        return _instance;
    //    }
    //}

    void Awake()
    {
        //DontDestroyOnLoad(this);

        DataEditor.LoadGameData();

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

        userName.text = DataEditor.selectedUser.name;
        userphoto.sprite = Resources.Load<Sprite>("UsersImages\\" + DataEditor.selectedUser.photoName);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadLevel("Gameplay");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadLevel("MainMenu");
        }
    }

    public int GoToNextCountry()
    {
        DataEditor.selectedContryID++;
        if (DataEditor.selectedContryID > (DataEditor.countries.Length - 1))
        {
            DataEditor.selectedContryID = 0;
        }

        ChangeSelectedCounty();

        return DataEditor.selectedContryID;
    }

    public int GoToPrevCountry()
    {
        DataEditor.selectedContryID--;
        if (DataEditor.selectedContryID < 0)
        {
            DataEditor.selectedContryID = DataEditor.countries.Length - 1;
        }

        ChangeSelectedCounty();

        return DataEditor.selectedContryID;
    }

    private void ChangeSelectedCounty()
    {
        DataEditor.selectedCountryAllAnimations = DataEditor.countries[DataEditor.selectedContryID].GetAllAnimations();
        DataEditor.selectedAnimationClipID = 0;

        ChangeSelectedCountryEvent.Invoke();
        ChangeSelectedAnimationClip();
    }

    public int GoToNextAnimationClip()
    {
        DataEditor.selectedAnimationClipID++;
        if (DataEditor.selectedAnimationClipID > (DataEditor.selectedCountryAllAnimations.Length-1))
        {
            DataEditor.selectedAnimationClipID = 0;
        }

        ChangeSelectedAnimationClip();

        return DataEditor.selectedAnimationClipID;
    }

    public int GoToPrevAnimationClip()
    {
        DataEditor.selectedAnimationClipID--;
        if (DataEditor.selectedAnimationClipID < 0)
        {
            DataEditor.selectedAnimationClipID = DataEditor.selectedCountryAllAnimations.Length - 1;
        }

        ChangeSelectedAnimationClip();

        return DataEditor.selectedAnimationClipID;
    }

    private void ChangeSelectedAnimationClip()
    {
        ChangeSelectedAnimationClipEvent.Invoke();
    }

    public void LoadLevel(string sceneName) //The name of the scene
    {
        DataEditor.SaveGameData();
        uiLoadingPanel.SetActive(true);
        StartCoroutine(LevelCoroutine(sceneName));
    }

    IEnumerator LevelCoroutine(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            uiLoadingTxt.text = (int)(async.progress * 100) + "%";
            yield return null;

        }
    }

    void OnApplicationQuit()
    {
        DataEditor.SaveGameData();
    }
}