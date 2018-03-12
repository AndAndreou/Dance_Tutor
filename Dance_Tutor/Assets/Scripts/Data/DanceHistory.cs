using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DanceHistory  {

    public string danceName;
    public Experience chorographyExperience;
    public string dateTime;
    public List<float> motionWordResultData;
    public List<float> styleWordResultData;
    public List<Skeleton.StyleWord> LMARadarGraphResults;
    public List<ArrayOfSerializableVector3> motionDataToUIAvatarResults;
    public List<float> wordsTimers;

    /// <summary>
    /// Create a dance history data
    /// </summary>
    /// <param name="danceName"></param>
    /// <param name="chorographyExperience"></param>
    /// <param name="motionWordResultData"></param>
    /// <param name="styleWordResultData"></param>
    public DanceHistory(string danceName, Experience chorographyExperience, List<float> motionWordResultData, List<float> styleWordResultData, List<Skeleton.StyleWord> LMARadarGraphResults, List<Vector3[]> motionDataToUIAvatarResults,List<float> wordsTimers)
    {
        this.danceName = danceName;
        this.chorographyExperience = chorographyExperience;
        this.dateTime = DateTime.Now.ToString();
        this.motionWordResultData = motionWordResultData;
        this.styleWordResultData = styleWordResultData;
        this.LMARadarGraphResults = LMARadarGraphResults;
        //this.motionDataToUIAvatarResults = motionDataToUIAvatarResults;
        this.motionDataToUIAvatarResults = new List<ArrayOfSerializableVector3>();
        foreach (Vector3[] arrayOfVector3 in motionDataToUIAvatarResults)
        {
            SerializableVector3[] temp = new SerializableVector3[arrayOfVector3.Length];
            for(int i = 0; i < arrayOfVector3.Length; i++)
            {
                temp[i] = arrayOfVector3[i];
            }
            this.motionDataToUIAvatarResults.Add(temp);
        }

        this.wordsTimers = wordsTimers;
    }
}
/// <summary>
/// To modify vector3 as serializeble
/// </summary>
[System.Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}]", x, y, z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}

/// <summary>
/// To modify array of serializableVector3 as serializable
/// </summary>
[System.Serializable]
public struct ArrayOfSerializableVector3
{
    public SerializableVector3[] array;

    public ArrayOfSerializableVector3(SerializableVector3[] arrayOfSerializableVector3)
    {
        array = arrayOfSerializableVector3;
    }

    /// <summary>
    /// Automatic conversion from ArrayOfSerializableVector3 to SerializableVector3[]
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3[] (ArrayOfSerializableVector3 rValue)
    {
        SerializableVector3[] temp = new SerializableVector3[rValue.array.Length];
        for(int i = 0; i < temp.Length; i++)
        {
            temp[i] = rValue.array[i];
        }
        return temp;
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3[] to ArrayOfSerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator ArrayOfSerializableVector3(SerializableVector3[] rValue)
    {
        ArrayOfSerializableVector3 temp = new ArrayOfSerializableVector3(rValue);
        for (int i = 0; i < temp.array.Length; i++)
        {
            temp.array[i] = rValue[i];
        }
        return temp;
    }
}
