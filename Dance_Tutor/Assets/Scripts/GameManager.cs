using EazyTools.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public AudioClip song;
    public float songDelay;

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
        DataEditor.LoadGameData();
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
        SoundManager.PlayMusic(song, 0.5f);
    }

    void OnApplicationQuit()
    {
        DataEditor.SaveGameData();
    }
}
