using ChartAndGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportSceneManager : MonoBehaviour {

    public GraphChart lineChart;

    private void Awake()
    {
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
        for(int i = 0; i < timer.Count; i++)
        {
            lineChart.DataSource.AddPointToCategory("StyleWord", styleWordResults[i], timer[i]);
            lineChart.DataSource.AddPointToCategory("MotionWord", styleWordResults[i], timer[i]);
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
