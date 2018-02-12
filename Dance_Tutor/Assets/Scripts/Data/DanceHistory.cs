using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DanceHistory  {

    public string danceName;
    public Experience chorographyExperience;
    public DateTime dateTime;
    public List<float> motionWordResultData;
    public List<float> styleWordResultData;

    /// <summary>
    /// Create a dance history data
    /// </summary>
    /// <param name="danceName"></param>
    /// <param name="chorographyExperience"></param>
    /// <param name="motionWordResultData"></param>
    /// <param name="styleWordResultData"></param>
    public DanceHistory(string danceName, Experience chorographyExperience, List<float> motionWordResultData, List<float> styleWordResultData)
    {
        this.danceName = danceName;
        this.chorographyExperience = chorographyExperience;
        this.dateTime = DateTime.Now;
        this.motionWordResultData = motionWordResultData;
        this.styleWordResultData = styleWordResultData;
    }
}
