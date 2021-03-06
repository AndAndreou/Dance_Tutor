﻿using UnityEngine;
using System.Collections;
using ChartAndGraph;

public class MyStreamingGraph : MonoBehaviour
{

    public GraphChart styleWordGraph;
    public GraphChart motiomWordGraph;

    private float startShowGraphTime = -1f;

    void Start()
    {
        if ((styleWordGraph == null) || (motiomWordGraph == null)) // the ChartGraph info is obtained via the inspector
            return;

        // Style word graph 
        styleWordGraph.DataSource.StartBatch(); // calling StartBatch allows changing the graph data without redrawing the graph for every change
        styleWordGraph.DataSource.ClearCategory("Value"); // clear the "Value" category. this category is defined using the GraphChart inspector

        styleWordGraph.DataSource.EndBatch(); // finally we call EndBatch , this will cause the GraphChart to redraw itself

        // Motion word graph
        motiomWordGraph.DataSource.StartBatch(); // calling StartBatch allows changing the graph data without redrawing the graph for every change
        motiomWordGraph.DataSource.ClearCategory("Value"); // clear the "Value" category. this category is defined using the GraphChart inspector

        motiomWordGraph.DataSource.EndBatch(); // finally we call EndBatch , this will cause the GraphChart to redraw itself

        // put thresholds
        //styleWordGraph.DataSource.AddPointToCategory("Threshold", 0, WordsManager.instance.styleWordThreshold);
        //motiomWordGraph.DataSource.AddPointToCategory("Threshold", 0, WordsManager.instance.motionWordThreshold);
        styleWordGraph.DataSource.AddPointToCategory("Threshold", 0, WordsManagerWithSync.instance.styleWordThreshold);
        motiomWordGraph.DataSource.AddPointToCategory("Threshold", 0, WordsManagerWithSync.instance.motionWordThreshold);
    }

    public void AddNewStyleWord(float newStyleWord)
    {
        // put threshold
        //styleWordGraph.DataSource.AddPointToCategory("Threshold", TakeTime(), WordsManager.instance.styleWordThreshold);
        styleWordGraph.DataSource.AddPointToCategory("Threshold", TakeTime(), WordsManagerWithSync.instance.styleWordThreshold);

        styleWordGraph.DataSource.AddPointToCategoryRealtime("Value", TakeTime(), newStyleWord, 1f);  //System.DateTime.Now
    }

    public void AddNewMotionWord(float newMotionWord)
    {
        // put threshold
        //motiomWordGraph.DataSource.AddPointToCategory("Threshold", TakeTime(), WordsManager.instance.motionWordThreshold);
        motiomWordGraph.DataSource.AddPointToCategory("Threshold", TakeTime(), WordsManagerWithSync.instance.motionWordThreshold);

        motiomWordGraph.DataSource.AddPointToCategoryRealtime("Value", TakeTime(), newMotionWord, 1f);
       
    }

    private double TakeTime()
    {
        if (startShowGraphTime < 0)
        {
            startShowGraphTime = Time.time;
        }

        return Time.time - startShowGraphTime;
    }
}
