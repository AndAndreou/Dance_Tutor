using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsManager : MonoBehaviour {

    [Header("MotionWord")]
    public int motionWordStep = 5;
    public int motionWordWindowSize = 35;
    private int motionWordStepCounter;

    [Header("StyleWord")]
    public int stykeWordStep = 5;
    public int styleWordWindowSize = 35;
    private int styleWordStepCounter;

    private CharController[] allCharCotrollers;

    private static WordsManager _instance = null;
    public static WordsManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (WordsManager)FindObjectOfType(typeof(WordsManager));
                if (_instance == null)
                    _instance = (new GameObject("WordsManager")).AddComponent<WordsManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        motionWordStepCounter = motionWordWindowSize;
        styleWordStepCounter = styleWordWindowSize;
    }

    // Use this for initialization
    void Start ()
    {
        allCharCotrollers = FindObjectsOfType<CharController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        #region Check and Add new words
        //new words for this frame
        List<Skeleton.MotionWord> newMotionWords = null;
        List < Skeleton.StyleWord> newStyleWords = null;

        // Check and put the new motion word if is the time
        motionWordStepCounter--;
        if (motionWordStepCounter <= 0)
        {
            newMotionWords = new List<Skeleton.MotionWord>();
            foreach (CharController controller in allCharCotrollers)
            {
                newMotionWords.Add(controller.skeleton.AddMotionWord(motionWordWindowSize));
            }
            motionWordStepCounter = motionWordWindowSize;
        }

        // Check and put the new style word if is the time
        styleWordStepCounter--;
        if (styleWordStepCounter <= 0)
        {
            newStyleWords = new List<Skeleton.StyleWord>();
            foreach (CharController controller in allCharCotrollers)
            {
                newStyleWords.Add(controller.skeleton.AddStyleWord(styleWordWindowSize));
            }
            styleWordStepCounter = styleWordWindowSize;
        }
        #endregion

        #region Comparisons


        #endregion
    }
}
