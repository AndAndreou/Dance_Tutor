using ChartAndGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReportSceneManager : MonoBehaviour {

    [Header("General Components")]
    public Text danceName;
    public Text danceDateTime;
    public Text chorographyExperience;

    [Header("LMA Randar Chart")]
    public MyRadarLMAResults myRandarLMAResults;

    [Header("T-Pose Avatar Feedback")]
    public UIAvatarControllerReport uiAvatarControllerScript;

    [Header("Motion n Style Words")]
    public GraphChart lineChart;

    [Header("Figures")]
    public GameObject figurePrefab;
    public GameObject figurePanel;
    public int spaceBetweenFigures;

    [Header("UI Component")]
    public GameObject uiLoadingPanel;
    public Text uiLoadingTxt;

    private int spaceBetweenFiguresCounter;

    private List<Image> figureImage;

    private DanceHistory danceHistory;

    // Data
    List<float> styleWordResults;
    List<float> motionWordResults;
    List<float> timer;

    List<Skeleton.StyleWord> lmaRadarGraphResults;
    List<ArrayOfSerializableVector3> uiAvatarResults;

    private void Awake()
    {
        figureImage = new List<Image>();

        if (FindObjectOfType<DataEditor>() == null)
        {
            DataEditor.LoadGameData();
        }

        danceHistory = DataEditor.selectedUser.GetTheLastDanceHistory();

        danceName.text = danceHistory.danceName;
        danceDateTime.text = danceHistory.dateTime;
        chorographyExperience.text = danceHistory.chorographyExperience.ToString();

        // Data Init
        styleWordResults = danceHistory.styleWordResultData;
        motionWordResults = danceHistory.motionWordResultData;
        timer = danceHistory.wordsTimers;

        lmaRadarGraphResults = DataEditor.selectedUser.GetTheLastDanceHistory().LMARadarGraphResults;
        uiAvatarResults = DataEditor.selectedUser.GetTheLastDanceHistory().motionDataToUIAvatarResults;

        spaceBetweenFiguresCounter = spaceBetweenFigures;
    }

    // Use this for initialization
    void Start () {
        SetMotionAndStyleChart();
        SetLMAComponentRadar();
        SetUIAvatarFeedback();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SetMotionAndStyleChart()
    {
        lineChart.DataSource.AddPointToCategory("MotionWord", 0, 0);
        lineChart.DataSource.AddPointToCategory("StyleWord", 0, 0);

        float figurePanelSize = figurePanel.GetComponent<RectTransform>().sizeDelta.x / timer.Count;

        for (int i = 0; i < timer.Count; i++)
        {
            lineChart.DataSource.AddPointToCategory("StyleWord", timer[i], styleWordResults[i]);
            lineChart.DataSource.AddPointToCategory("MotionWord", timer[i], motionWordResults[i]);

            GameObject go = Instantiate(figurePrefab, figurePanel.transform.position, Quaternion.identity, figurePanel.transform);
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector2(i * figurePanelSize, 0);

            figureImage.Add(go.GetComponent<Image>());
        }
    }

    private void SetLMAComponentRadar()
    {
        Skeleton.StyleWord sumOfStyleResult = new Skeleton.StyleWord();
        sumOfStyleResult = sumOfStyleResult.GetSumOfWords(lmaRadarGraphResults.ToArray());
        Skeleton.StyleWord avgOfStyleResult = new Skeleton.StyleWord();
        avgOfStyleResult = avgOfStyleResult.DivideWordWithNumber(sumOfStyleResult, lmaRadarGraphResults.Count);
        myRandarLMAResults.GetNewData(avgOfStyleResult);
    }

    private void SetUIAvatarFeedback()
    {
        Vector3[] sumOfUIResults = new Vector3[uiAvatarResults[0].array.Length];
        float maxValue = 0;

        foreach (ArrayOfSerializableVector3 arrayOfSerializableVector3 in uiAvatarResults)
        {
            SerializableVector3[] temp = arrayOfSerializableVector3.array;
            for(int i = 0; i < temp.Length; i++) 
            {
                sumOfUIResults[i] += temp[i];
                maxValue = Mathf.Max(maxValue, sumOfUIResults[i].x, sumOfUIResults[i].y, sumOfUIResults[i].z);
            }
        }

        // Normilize
        for(int x=0;x< sumOfUIResults.Length; x++)
        {
            sumOfUIResults[x] /= maxValue;
        }

        uiAvatarControllerScript.lastMotionWord = sumOfUIResults;
    }

    public void AddFigureImg(int index, Sprite sprite)
    {
        figureImage[index].sprite = sprite;
        if ((spaceBetweenFiguresCounter == 0)||(index== figureImage.Count-1))
        {
            figureImage[index].color = Color.white;
            spaceBetweenFiguresCounter = spaceBetweenFigures;
        }
        spaceBetweenFiguresCounter--;
    }

    public void LoadLevel(string sceneName) //The name of the scene
    {
        DataEditor.SaveGameData();
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
