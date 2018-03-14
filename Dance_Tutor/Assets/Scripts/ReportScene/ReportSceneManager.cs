using ChartAndGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportSceneManager : MonoBehaviour {

    public GraphChart lineChart;
    public GameObject figurePrefab;

    private List<Image> figureImage;
    public GameObject figurePanel;

    private void Awake()
    {
        figureImage = new List<Image>();

        if (FindObjectOfType<DataEditor>() == null)
        {
            DataEditor.LoadGameData();
        }
    }

    // Use this for initialization
    void Start () {
        List<float> styleWordResults = DataEditor.selectedUser.GetTheLastDanceHistory().styleWordResultData;
        List<float> motionWordResults = DataEditor.selectedUser.GetTheLastDanceHistory().motionWordResultData;
        List<float> timer = DataEditor.selectedUser.GetTheLastDanceHistory().wordsTimers;
        lineChart.DataSource.AddPointToCategory("MotionWord", 0, 0);
        lineChart.DataSource.AddPointToCategory("StyleWord", 0, 0);

        float figurePanelSize = figurePanel.GetComponent<RectTransform>().sizeDelta.x / timer.Count;

        for (int i = 0; i < timer.Count; i++)
        {
            lineChart.DataSource.AddPointToCategory("StyleWord", timer[i], styleWordResults[i]);
            lineChart.DataSource.AddPointToCategory("MotionWord", timer[i], motionWordResults[i]);

            GameObject go = Instantiate(figurePrefab, figurePanel.transform.position, Quaternion.identity, figurePanel.transform);
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector2(i * figurePanelSize,0) ;

            figureImage.Add(go.GetComponent<Image>());
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddFigureImg(int index, Sprite sprite)
    {
        figureImage[index].sprite = sprite;
        figureImage[index].color = Color.white;
    }
}
