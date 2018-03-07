using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFeedBackResultManager : MonoBehaviour {

    public UIAvatarController uiAvatarControllerScript;
    public MyStreamingGraph myStreaminGraphScript;
    public MyRadarLMAResults myRandarLMAResults;

    public void AddNewStyleWordForStreamingGraph(float totalDistanceStyleWord)
    {
        myStreaminGraphScript.AddNewStyleWord(totalDistanceStyleWord);
    }

    public void AddNewMotionWordForStreamingGraph(float motionResult)
    {
        myStreaminGraphScript.AddNewMotionWord(motionResult);
    }

    public void SendMotionDataToUIAvatar(Vector3[] totalDistanceMotion)
    {
        uiAvatarControllerScript.lastMotionWord = totalDistanceMotion;
    }

    public void SendNewStyleWordToLMARadarGraph(Skeleton.StyleWord newStyleWord)
    {
        myRandarLMAResults.GetNewData(newStyleWord);
    }
}
