﻿using EazyTools.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

    public AudioClip song;
    public float songDelay;

    [Header("Debug")]
    public bool isDebugMode;
    public bool useDataFromUser;

    public bool showChoreography { get; private set; }

    //events
    [HideInInspector]
    public UnityEvent TPoseDetectionEvent;

    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));
                if (_instance == null)
                    _instance = (new GameObject("GameManager")).AddComponent<GameManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        //init
        showChoreography = isDebugMode;

        if (FindObjectOfType<DataEditor>() == null)
        {
            DataEditor.LoadGameData();
        }

        //create event for T-Pose Detect
        if (TPoseDetectionEvent == null)
        {
            TPoseDetectionEvent = new UnityEvent();
        }

        //add listener for T-Pose Detect ations
        TPoseDetectionEvent.AddListener(TPoseDetetionAction);

        // Prints
        Debug.Log("********************************************");
        Debug.Log("Selected user -> " + DataEditor.GetSelectedUser());
        Debug.Log("Selected country -> " + DataEditor.GetCountry());
        Debug.Log("Selected animation clip -> " + DataEditor.GetAnimationClip());
        Debug.Log("********************************************");
    }

    // Use this for initialization
    void Start () {
        Invoke("PlaySong", songDelay);
	}
	
	// Update is called once per frame
	void Update () {

    }

    void PlaySong()
    {
        //SoundManager.PlayMusic(song, 0.5f);
    }

    private void TPoseDetetionAction()
    {
        if(showChoreography == false)
        {
            showChoreography = true;
        }
    }

    public void ChoreographyFinished()
    {
        showChoreography = false;
    }

    /// <summary>
    /// Before application quit save all data
    /// </summary>
    void OnApplicationQuit()
    {
        DataEditor.SaveGameData();
    }
}
