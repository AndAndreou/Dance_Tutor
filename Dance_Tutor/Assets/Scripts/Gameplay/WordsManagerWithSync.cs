using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsManagerWithSync : MonoBehaviour {

    [Header("Extra Params For Sync")]
    public int syncWindowSize = 10;
    public int syncWindowStep = 15;

    private static int frameCounter = 0;
    private static int frameNoNeeded;
    private static int nextStartFrameIdForSyncWindowTutor = 0;
    private static int nextStartFrameIdForSyncWindowStudent = 0;

    [Header("MotionWord")]
    public int motionWordStep = 5;
    public int motionWordWindowSize = 35;
    private int motionWordStepCounter;

    [Header("StyleWord")]
    public int styleWordStep = 5;
    public int styleWordWindowSize = 35;
    private int styleWordStepCounter;

    [HideInInspector]
    public float styleWordThreshold ;
    [HideInInspector]
    public float motionWordThreshold ;

    [HideInInspector]
    public static List<float> styleWordResults;
    [HideInInspector]
    public static List<float> motionWordResults;
    [HideInInspector]
    public static List<Skeleton.StyleWord> LMARadarGraphResults;
    [HideInInspector]
    public static List<Vector3[]> motionDataToUIAvatarResults;
    [HideInInspector]
    public static List<float> wordsTimers; // Save time of animation for each written word

    private Skeleton.StyleWord maxStyleWords = new Skeleton.StyleWord(); //used for styleword normilized

    public static bool writeWords { get; private set; }
    public static bool prevWriteWordsVal { get; private set; }

    [HideInInspector]
    public static CharController[] allCharCotrollers { get; private set; }

    private static bool savedWords = true;

    private UIFeedBackResultManager uiFeedBackResultManager;

    // params from inspector
    [HideInInspector]
    public bool syncAnimationEnable = true;
    [HideInInspector]
    public bool syncWindowsMoveParallel;
    [HideInInspector]
    public PositionOfSyncFrameEnum positionOfSyncFrame;

    public enum PositionOfSyncFrameEnum
    {
        start = 0,
        middle = 1,
        end = 2
    }

    private static WordsManagerWithSync _instance = null;
    public static WordsManagerWithSync instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (WordsManagerWithSync)FindObjectOfType(typeof(WordsManagerWithSync));
                if (_instance == null)
                    _instance = (new GameObject("WordsManagerWithSync")).AddComponent<WordsManagerWithSync>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        motionWordStepCounter = motionWordWindowSize;
        styleWordStepCounter = styleWordWindowSize;

        frameNoNeeded = syncWindowSize + motionWordWindowSize;

        int startFrameID = 0;
        switch (positionOfSyncFrame)
        {
            case PositionOfSyncFrameEnum.start:
                startFrameID = 0;
                break;
            case PositionOfSyncFrameEnum.middle:
                startFrameID = Mathf.FloorToInt(motionWordWindowSize / 2f);
                break;
            case PositionOfSyncFrameEnum.end:
                startFrameID = frameNoNeeded - syncWindowSize + 1;
                break;
            default:
                startFrameID = frameNoNeeded - syncWindowSize + 1;
                break;
        }
        nextStartFrameIdForSyncWindowTutor = nextStartFrameIdForSyncWindowStudent = startFrameID;
    }

    // Use this for initialization
    void Start ()
    {
        InitWords();

        styleWordThreshold = DataEditor.styleWordThreshold;
        motionWordThreshold = DataEditor.motionWordThreshold;

        allCharCotrollers = FindObjectsOfType<CharController>();

        // Get only tutor and student charController
        List<CharController> tempList = new List<CharController>();
        foreach(CharController c in allCharCotrollers)
        {
            if ((c.avatarRole == CharController.Role.Tutor) || ((c.avatarRole == CharController.Role.Student)))
            {
                tempList.Add(c);
            }
        }
        allCharCotrollers = tempList.ToArray();

        uiFeedBackResultManager = FindObjectOfType<UIFeedBackResultManager>();

        maxStyleWords = DataEditor.gameData.maxStyleWords;

    }

    private static void InitWords()
    {
        styleWordResults = new List<float>();
        motionWordResults = new List<float>();
        LMARadarGraphResults = new List<Skeleton.StyleWord>();
        motionDataToUIAvatarResults = new List<Vector3[]>();
        wordsTimers = new List<float>(); 

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

    private int[] FindBestMatchingFrameMotions()
    {
        int[] bestMatchingId = new int[2];
        float minDistance = 10000;

        for (int i = nextStartFrameIdForSyncWindowTutor; i < nextStartFrameIdForSyncWindowTutor + syncWindowSize; i++)
        {
            for(int x = nextStartFrameIdForSyncWindowStudent; x < nextStartFrameIdForSyncWindowStudent + syncWindowSize; x++)
            {
                Skeleton.MotionWord[] frameMotions = new Skeleton.MotionWord[2];
                frameMotions[0] = allCharCotrollers[0].skeleton.frameMotions[i];
                frameMotions[1] = allCharCotrollers[1].skeleton.frameMotions[x];

                List<Vector3[]> distance = frameMotions[0].GetDistanceBetweenWordsInDegrees(frameMotions);
                Vector3[] sumOfFrames = frameMotions[0].GetSumOfFrames(distance);
                float totalSum = frameMotions[0].GetTotalSum(sumOfFrames);

                if ((totalSum < minDistance) || ((i == nextStartFrameIdForSyncWindowTutor) && (x == nextStartFrameIdForSyncWindowStudent))) // Find smaller distance or we are in first comparison
                {
                    minDistance = totalSum;
                    bestMatchingId[0] = i;
                    bestMatchingId[1] = x;
                }
            }
        }

        // Find next parameters 
        if (syncWindowsMoveParallel)
        {
            nextStartFrameIdForSyncWindowTutor = nextStartFrameIdForSyncWindowStudent = Mathf.Max(bestMatchingId[0], bestMatchingId[1]) + syncWindowStep;
        }
        else
        {
            nextStartFrameIdForSyncWindowTutor = bestMatchingId[0] + syncWindowStep;
            nextStartFrameIdForSyncWindowStudent = bestMatchingId[1] + syncWindowStep;
        }

        //frameNoNeeded = Mathf.Max(nextStartFrameIdForSyncWindowTutor, nextStartFrameIdForSyncWindowStudent) + syncWindowSize + Mathf.CeilToInt(motionWordWindowSize / 2f);
        switch (positionOfSyncFrame)
        {
            case PositionOfSyncFrameEnum.start:
                frameNoNeeded = Mathf.Max(nextStartFrameIdForSyncWindowTutor, nextStartFrameIdForSyncWindowStudent) + syncWindowSize + Mathf.Max(motionWordWindowSize,styleWordWindowSize);
                break;
            case PositionOfSyncFrameEnum.middle:
                frameNoNeeded = Mathf.Max(nextStartFrameIdForSyncWindowTutor, nextStartFrameIdForSyncWindowStudent) + syncWindowSize + Mathf.CeilToInt(Mathf.Max(motionWordWindowSize, styleWordWindowSize) / 2f);
                break;
            case PositionOfSyncFrameEnum.end:
                frameNoNeeded = Mathf.Max(nextStartFrameIdForSyncWindowTutor, nextStartFrameIdForSyncWindowStudent) + syncWindowSize - 1;
                break;
            default:
                frameNoNeeded = Mathf.Max(nextStartFrameIdForSyncWindowTutor, nextStartFrameIdForSyncWindowStudent) + syncWindowSize - 1;
                break;
        }
        //Debug.Log(Mathf.Max(nextStartFrameIdForSyncWindowTutor, nextStartFrameIdForSyncWindowStudent) + " , " + syncWindowSize + " , " + Mathf.CeilToInt(motionWordWindowSize / 2f));

        return bestMatchingId;
    }

    void LateUpdate()
    {
        if (writeWords == false)
        {
            return;
        }

        #region Check and Add new words

        List<Skeleton.MotionWord> newMotionWords = null;
        List<Skeleton.StyleWord> newStyleWords = null;

        if (syncAnimationEnable)
        {
            
            #region Sync Animation Code
            if (frameNoNeeded <= frameCounter) // Then can check the words
            {
                //print("frameNeeded:" + frameNoNeeded);
                newMotionWords = new List<Skeleton.MotionWord>();
                newStyleWords = new List<Skeleton.StyleWord>();

                //Debug.Log("frameCounter: " + frameCounter + ", frameMotionNo: " + allCharCotrollers[0].skeleton.frameMotions.Count + ", frameNeeded: " + frameNoNeeded);
                int[] bestMatchingFrameMotions = FindBestMatchingFrameMotions();

                int[] startEndFrames = new int[2];
                // Add MotionWords
                //Debug.Log("bestMatchingFrameMotions[0]: " + bestMatchingFrameMotions[0] + ", bestMatchingFrameMotions[1]: " + bestMatchingFrameMotions[1] + ", motionWordWindowSize: " + motionWordWindowSize);
                startEndFrames = FindStartAndEndFrameForWord(bestMatchingFrameMotions[0], motionWordWindowSize);
                //print(startEndFrames[0] + " - " + startEndFrames[1]);
                newMotionWords.Add(allCharCotrollers[0].skeleton.AddMotionWord(startEndFrames[0], startEndFrames[1]));

                startEndFrames = FindStartAndEndFrameForWord(bestMatchingFrameMotions[1], motionWordWindowSize);
                //print(startEndFrames[0] + " - " + startEndFrames[1]);
                newMotionWords.Add(allCharCotrollers[1].skeleton.AddMotionWord(startEndFrames[0], startEndFrames[1]));

                // Add StyleWords
                startEndFrames = FindStartAndEndFrameForWord(bestMatchingFrameMotions[0], styleWordWindowSize);
                Skeleton.StyleWord controllerNewStyleWord = allCharCotrollers[0].skeleton.AddStyleWord(startEndFrames[0], startEndFrames[1]);
                newStyleWords.Add(controllerNewStyleWord);
                // Get the maximum value of all style words
                maxStyleWords = controllerNewStyleWord.GetMax(controllerNewStyleWord, maxStyleWords);

                startEndFrames = FindStartAndEndFrameForWord(bestMatchingFrameMotions[1], styleWordWindowSize);
                controllerNewStyleWord = allCharCotrollers[1].skeleton.AddStyleWord(startEndFrames[0], startEndFrames[1]);
                newStyleWords.Add(controllerNewStyleWord);
                // Get the maximum value of all style words
                maxStyleWords = controllerNewStyleWord.GetMax(controllerNewStyleWord, maxStyleWords);

                DataEditor.gameData.maxStyleWords = maxStyleWords;

                // Save the time in animation of words
                wordsTimers.Add(allCharCotrollers[0].skeleton.joints[0].frames[bestMatchingFrameMotions[0]].time);
            }
            #endregion
        }
        else
        {
            #region Async Animation Code
            // Check and put the new motion word if is the time

            motionWordStepCounter--;
            if (motionWordStepCounter <= 0)
            {
                newMotionWords = new List<Skeleton.MotionWord>();
                foreach (CharController controller in allCharCotrollers)
                {
                    newMotionWords.Add(controller.skeleton.AddMotionWord(motionWordWindowSize));

                }
                motionWordStepCounter = motionWordStep;
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
                styleWordStepCounter = styleWordStep;
            }
            #endregion
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

            LMARadarGraphResults.Add(distanceStyleWord);
            uiFeedBackResultManager.SendNewStyleWordToLMARadarGraph(distanceStyleWord);

            // Get Sum of all ellements of distances style word
            float totalDistanceStyleWord = distanceStyleWord.GetSumOfVars(distanceStyleWord);
            //print("..." + totalDistanceStyleWord + ", " + newStyleWords[0].hipVelocityMax + ", " + newStyleWords[1].hipVelocityMax);
            styleWordResults.Add(totalDistanceStyleWord);
            uiFeedBackResultManager.AddNewStyleWordForStreamingGraph(totalDistanceStyleWord);
        }

        if(newMotionWords != null)
        {
            List<Vector3[]> distanceMotionWord = new List<Vector3[]>();
            // Get distance for all frames
            distanceMotionWord = newMotionWords[0].GetDistanceBetweenWordsInDegrees(newMotionWords.ToArray());

            // Get Sum of all frames of distances motion word
            Vector3[] totalDistanceMotion = newMotionWords[0].GetSumOfFrames(distanceMotionWord);

            // Send data to ui avatar controller
            motionDataToUIAvatarResults.Add(totalDistanceMotion);
            uiFeedBackResultManager.SendMotionDataToUIAvatar(totalDistanceMotion);

            float avgError = 0;
            for (int i = 0; i < totalDistanceMotion.Length; i++) // For each joint
            {
                avgError += ((totalDistanceMotion[i].x + totalDistanceMotion[i].y + totalDistanceMotion[i].z)/3f); //avg of 3 axis error

            }

            float motionResult = avgError / totalDistanceMotion.Length; // avg of all frames
            //motionWordResults.Add(totalDistanceMotion);
            motionWordResults.Add(motionResult);
            uiFeedBackResultManager.AddNewMotionWordForStreamingGraph(motionResult);
        }
        #endregion

        frameCounter++;
    }

    /// <summary>
    /// Return the first frame id in position 0 and the last frame id in position 1
    /// </summary>
    /// <param name="selectedFrame"></param>
    /// <param name="wordSize"></param>
    /// <returns></returns>
    private int[] FindStartAndEndFrameForWord(int selectedFrame,int wordSize)
    {
        // Calculate the start frame of word
        int[] frames = new int[2];
        int startFrame = 0;
        int endFrame = 0;
        switch (positionOfSyncFrame)
        {
            case PositionOfSyncFrameEnum.start:
                startFrame = selectedFrame;
                break;
            case PositionOfSyncFrameEnum.middle:
                startFrame = selectedFrame - Mathf.FloorToInt(wordSize / 2f);
                break;
            case PositionOfSyncFrameEnum.end:
                startFrame = selectedFrame - wordSize + 1;
                break;
            default:
                startFrame = selectedFrame - wordSize + 1;
                break;
        }

        endFrame = startFrame + wordSize - 1;

        frames[0] = startFrame;
        frames[1] = endFrame;

        return frames;
    }

    public static bool StartWriteWords()
    {
        writeWords = true;
        savedWords = false;
        return writeWords;
    }

    public static bool StopWriteWords()
    {
        if (savedWords == false)
        {
            DataEditor.SaveWords();
            savedWords = true;
        }

        InitWords();

        return writeWords;
    }
}
