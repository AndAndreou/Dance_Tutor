using ChartAndGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRadarLMAResults : MonoBehaviour {

    public RadarChart randarChart;

    public GameObject hintStartPosition;
    public GameObject hintPrefab;

    private float lmaComponentsUpperThreshold;
    private float lmaComponentsLowerThreshold;

    public float lmaHintTime;

    private float currentLMATime;

    private List<string> upperThresholdLMA;
    private List<string> lowerThresholdLMA;

    private void Awake()
    {
        upperThresholdLMA = new List<string>();
        lowerThresholdLMA = new List<string>();
    }

    private void Start()
    {

        lmaComponentsUpperThreshold = DataEditor.lmaComponentsUpperThreshold;
        lmaComponentsLowerThreshold = DataEditor.lmaComponentsLowerThreshold;

        currentLMATime = lmaHintTime;

        float maxValue = lmaComponentsUpperThreshold * 2;
        randarChart.DataSource.MaxValue = maxValue;

        // Init Upper Wrong Area
        randarChart.DataSource.SetValue("UpperWrongArea", "Body", maxValue);
        randarChart.DataSource.SetValue("UpperWrongArea", "Effort", maxValue);
        randarChart.DataSource.SetValue("UpperWrongArea", "Shape", maxValue);
        randarChart.DataSource.SetValue("UpperWrongArea", "Space", maxValue);

        // Init correct area
        randarChart.DataSource.SetValue("CorrectArea", "Body", lmaComponentsUpperThreshold);
        randarChart.DataSource.SetValue("CorrectArea", "Effort", lmaComponentsUpperThreshold);
        randarChart.DataSource.SetValue("CorrectArea", "Shape", lmaComponentsUpperThreshold);
        randarChart.DataSource.SetValue("CorrectArea", "Space", lmaComponentsUpperThreshold);

        // Init Lower Wrong Area
        randarChart.DataSource.SetValue("LowerWrongArea", "Body", lmaComponentsLowerThreshold);
        randarChart.DataSource.SetValue("LowerWrongArea", "Effort", lmaComponentsLowerThreshold);
        randarChart.DataSource.SetValue("LowerWrongArea", "Shape", lmaComponentsLowerThreshold);
        randarChart.DataSource.SetValue("LowerWrongArea", "Space", lmaComponentsLowerThreshold);
    }

    private void Update()
    {
        currentLMATime -= Time.deltaTime;
    }

    public void GetNewData(Skeleton.StyleWord styleWord)
    {
        upperThresholdLMA.Clear();
        lowerThresholdLMA.Clear();

        float lmaBody = styleWord.GetLMAComponent(Skeleton.StyleWord.LMACompnents.Body, Skeleton.StyleWord.LMAOperationType.Sum);
        randarChart.DataSource.SetValue("LMAValues", "Body", lmaBody);
        CheckThresholds("Body", lmaBody);

        float lmaEffort = styleWord.GetLMAComponent(Skeleton.StyleWord.LMACompnents.Effort, Skeleton.StyleWord.LMAOperationType.Sum);
        randarChart.DataSource.SetValue("LMAValues", "Effort", lmaEffort);
        CheckThresholds("Effort", lmaEffort);

        float lmaShape = styleWord.GetLMAComponent(Skeleton.StyleWord.LMACompnents.Shape, Skeleton.StyleWord.LMAOperationType.Sum);
        randarChart.DataSource.SetValue("LMAValues", "Shape", lmaShape);
        CheckThresholds("Shape", lmaShape);

        float lmaSpace = styleWord.GetLMAComponent(Skeleton.StyleWord.LMACompnents.Space, Skeleton.StyleWord.LMAOperationType.Sum);
        randarChart.DataSource.SetValue("LMAValues", "Space", lmaSpace);
        CheckThresholds("Space", lmaSpace);

        if (currentLMATime <= 0)
        {
            GenerateHint();
            currentLMATime = lmaHintTime;
        }
    }

    private void CheckThresholds(string lmaComponent,float value)
    {
        if (value < lmaComponentsLowerThreshold)
        {
            lowerThresholdLMA.Add(lmaComponent);
        }
        else if (value > lmaComponentsUpperThreshold)
        {
            upperThresholdLMA.Add(lmaComponent);
        }
    }

    private void GenerateHint()
    {
        string msg = "";
        if (lowerThresholdLMA.Count != 0)
        {
            msg += "More ";
            for(int i=0; i < lowerThresholdLMA.Count; i++) 
            {
                if (i > 0)
                {
                    msg += ", ";
                }
                msg += lowerThresholdLMA[i];
            }
        }

        if (upperThresholdLMA.Count != 0)
        {
            msg += "\nLess ";
            for (int i = 0; i < upperThresholdLMA.Count; i++)
            {
                if (i > 0)
                {
                    msg += ", ";
                }
                msg += upperThresholdLMA[i];
            }
        }

        if (!(hintPrefab == null) && (hintStartPosition != null))
        {
            GameObject hint = Instantiate(hintPrefab, hintStartPosition.transform.position, Quaternion.identity, hintStartPosition.transform);
            hint.GetComponent<LMAHintController>().SetMsg(msg);
        }
    }
}
