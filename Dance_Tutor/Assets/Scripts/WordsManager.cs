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

    [Header("Comparisons")]
    public float styleWordThreshold;
    public float motionWordThreshold;

    private float maxNumInStyleWords = 0; //used for styleword normilized

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
        maxNumInStyleWords = DataEditor.gameData.maxNumInStyleWords;
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
                // Write style word for each controller
                Skeleton.StyleWord controllerNewStyleWord = controller.skeleton.AddStyleWord(styleWordWindowSize);
                newStyleWords.Add(controllerNewStyleWord);

                // Get the maximum value of all style words
                maxNumInStyleWords = Mathf.Max(maxNumInStyleWords, controllerNewStyleWord.GetMax(controllerNewStyleWord));
                DataEditor.gameData.maxNumInStyleWords = maxNumInStyleWords;
            }
            styleWordStepCounter = styleWordWindowSize;
        }
        #endregion

        #region Comparisons

        if (newStyleWords != null)
        {
            for (int i = 0; i < newStyleWords.Count; i++)
            {
                // Normalized all style words dependent on maxNumInStyleWords
                newStyleWords[i] = newStyleWords[i].GetNormilizedWord(newStyleWords[i], maxNumInStyleWords);
            }

            Skeleton.StyleWord distanceStyleWord = new Skeleton.StyleWord();
            // Get total distance
            distanceStyleWord = distanceStyleWord.GetDistanceBetweenWords(newStyleWords.ToArray());
            // Get Sum of all ellements of distances style word
            float totalDistance = distanceStyleWord.GetSumOfVars(distanceStyleWord);

            if (totalDistance < styleWordThreshold)
            {
                print("Style Word: " + totalDistance + " OK");
            }
            else
            {
                print("Style Word: " + totalDistance + " NOT OK");
            }

        }

        if(newMotionWords != null)
        {
            List<Vector3[]> distanceMotionWord = new List<Vector3[]>();
            // Get total distance
            distanceMotionWord = newMotionWords[0].GetDistanceBetweenWordsInDegrees(newMotionWords.ToArray());
            // Get Sum of all ellements of distances style word
            Vector3[] totalDistanceMotion = newMotionWords[0].GetSumOfFrames(distanceMotionWord);
            for (int i = 0; i < totalDistanceMotion.Length; i++)
            {
                if ((totalDistanceMotion[i].x <= motionWordThreshold) && (totalDistanceMotion[i].y <= motionWordThreshold) && (totalDistanceMotion[i].z <= motionWordThreshold))
                {
                    print("Motion Word - joint " + allCharCotrollers[0].skeleton.joints[i].GetJointName() + ": " + totalDistanceMotion[i] + " OK");
                }
                else
                {
                    print("Motion Word - joint " + allCharCotrollers[0].skeleton.joints[i].GetJointName() + ": " + totalDistanceMotion[i] + " NOT OK");
                }
            }
        }
        #endregion
    }
}
