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

    [Header("Controllers")]
    public MyStreamingGraph myStreamingGraphController;

    [HideInInspector]
    public static List<float> styleWordResults;
    [HideInInspector]
    public static List<Vector3[]> motionWordResults;

    private Skeleton.StyleWord maxStyleWords = new Skeleton.StyleWord(); //used for styleword normilized

    public static bool writeWords { get; private set; }
    public static bool prevWriteWordsVal { get; private set; }

    [HideInInspector]
    public static CharController[] allCharCotrollers { get; private set; }

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
        styleWordResults = new List<float>();
        motionWordResults = new List<Vector3[]>();

        allCharCotrollers = FindObjectsOfType<CharController>();

        maxStyleWords = DataEditor.gameData.maxStyleWords;

        writeWords = false;
        prevWriteWordsVal = false;

    }
	
	// Update is called once per frame
	void Update () {

        if (writeWords == false)
        {
            if (prevWriteWordsVal == true)
            {
                // End writing of frames
                DataEditor.SaveResultsData();
            }
        }

        prevWriteWordsVal = writeWords;
    }

    void LateUpdate()
    {
        if (writeWords == false)
        {
            return;
        }

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
            //print("!!!!!!!!!!!!!!!!!!");
            newStyleWords = new List<Skeleton.StyleWord>();
            foreach (CharController controller in allCharCotrollers)
            {
                // Write style word for each controller
                Skeleton.StyleWord controllerNewStyleWord = controller.skeleton.AddStyleWord(styleWordWindowSize);
                newStyleWords.Add(controllerNewStyleWord);
                //print(controllerNewStyleWord.centroidPelvisDistanceMax);

                // Get the maximum value of all style words
                //maxStyleWords = Mathf.Max(maxStyleWords, controllerNewStyleWord.GetMax(controllerNewStyleWord, maxStyleWords), DataEditor.gameData.maxStyleWords);
                maxStyleWords = controllerNewStyleWord.GetMax(controllerNewStyleWord, maxStyleWords);
                DataEditor.gameData.maxStyleWords = maxStyleWords;
            }
            styleWordStepCounter = styleWordWindowSize;
        }
        #endregion

        #region Comparisons

        if (newStyleWords != null)
        {

            for (int i = 0; i < newStyleWords.Count; i++)
            {
                // Normalized all style words dependent on maxStyleWords
                newStyleWords[i] = newStyleWords[i].GetNormilizedWord(newStyleWords[i], maxStyleWords);
            }

            Skeleton.StyleWord distanceStyleWord = new Skeleton.StyleWord();
            // Get total distance
            distanceStyleWord = distanceStyleWord.GetDistanceBetweenWords(newStyleWords.ToArray());
            //print(distanceStyleWord.centroidHeightMax);

            // Get Sum of all ellements of distances style word
            float totalDistanceStyleWord = distanceStyleWord.GetSumOfVars(distanceStyleWord);

            if (totalDistanceStyleWord < styleWordThreshold)
            {
                //print("Style Word: " + totalDistanceStyleWord + " OK");
            }
            else
            {
                //print("Style Word: " + totalDistanceStyleWord + " NOT OK");
            }

            styleWordResults.Add(totalDistanceStyleWord);
            myStreamingGraphController.AddNewStyleWord(totalDistanceStyleWord);
        }

        if(newMotionWords != null)
        {
            List<Vector3[]> distanceMotionWord = new List<Vector3[]>();
            // Get distance for all frames
            distanceMotionWord = newMotionWords[0].GetDistanceBetweenWordsInDegrees(newMotionWords.ToArray());
            // Get Sum of all frames of distances motion word
            Vector3[] totalDistanceMotion = newMotionWords[0].GetSumOfFrames(distanceMotionWord);

            float avgError = 0;
            for (int i = 0; i < totalDistanceMotion.Length; i++) // For each joint
            {
                avgError += ((totalDistanceMotion[i].x + totalDistanceMotion[i].y + totalDistanceMotion[i].z)/3f); //avg of 3 axis error
                if ((totalDistanceMotion[i].x <= motionWordThreshold) && (totalDistanceMotion[i].y <= motionWordThreshold) && (totalDistanceMotion[i].z <= motionWordThreshold))
                {
                    //print("Motion Word - joint " + allCharCotrollers[0].skeleton.joints[i].GetJointName() + ": " + totalDistanceMotion[i] + " OK");
                }
                else
                {
                    //print("Motion Word - joint " + allCharCotrollers[0].skeleton.joints[i].GetJointName() + ": " + totalDistanceMotion[i] + " NOT OK");
                }
            }

            motionWordResults.Add(totalDistanceMotion);
            myStreamingGraphController.AddNewMotionWord(avgError/ totalDistanceMotion.Length);
        }
        #endregion
    }

    public static bool StartWriteWords()
    {
        writeWords = true;
        return writeWords;
    }

    public static bool StopWriteWords()
    {
        writeWords = false;
        return writeWords;
    }
}
