using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User {
    public string photoName;
    public string name;
    public string email;
    public string dateOfBirth;
    public Sex sex;
    public Experience expirience;
    public string country;
    private List<DanceHistory> history;

    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="email"></param>
    /// <param name="dateOfBirth"></param>
    /// <param name="sex"></param>
    /// <param name="expirience"></param>
    /// <param name="country"></param>
    public User(string photo, string name, string email, string dateOfBirth, Sex sex, Experience expirience, string country)
    {
        this.photoName = photo;
        this.name = name;
        this.email = email;
        this.dateOfBirth = dateOfBirth;
        this.sex = sex;
        this.expirience = expirience;
        this.country = country;
        this.history = new List<DanceHistory>();
    }

    /// <summary>
    /// Add new dance history to user
    /// </summary>
    /// <param name="danceName"></param>
    /// <param name="chorographyExperience"></param>
    /// <param name="motionWordResultData"></param>
    /// <param name="styleWordResultData"></param>
    /// <returns></returns>
    public DanceHistory AddDanceHistory(string danceName, Experience chorographyExperience, List<Vector3[]> motionWordResultData, List<float> styleWordResultData)
    {
        DanceHistory newDanceHistory = new DanceHistory(danceName, chorographyExperience, motionWordResultData, styleWordResultData);
        this.history.Add(newDanceHistory);
        return newDanceHistory;
    }

    public List<DanceHistory> GetAllDanceHistory()
    {
        return this.history;
    }
}

public enum Sex
{
    Male = 0,
    Female = 1,
    Other = 2
}

public enum Experience
{
    Beginner = 0,
    Intermediate = 1,
    Expert = 2
}