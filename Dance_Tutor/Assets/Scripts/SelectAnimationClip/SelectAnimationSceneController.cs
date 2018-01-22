using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SelectAnimationSceneController : MonoBehaviour {

    public static Country[] countries;
    public static int selectedContryID;

    private static SelectAnimationSceneController _instance = null;
    public static SelectAnimationSceneController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (SelectAnimationSceneController)FindObjectOfType(typeof(GameManager));
                if (_instance == null)
                    _instance = (new GameObject("SelectAnimationSceneController")).AddComponent<SelectAnimationSceneController>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        LoadCountries();
    }

    private void LoadCountries()
    {
        string[] countriesFoldersPaths = Directory.GetDirectories(Application.dataPath + "/Resources/Animations/Countries"); // Get the paths of all folder in this path
        string[] countriesFoldersNames = new string[countriesFoldersPaths.Length];
        for (int i = 0; i < countriesFoldersPaths.Length; i++)
        {
            var d = new DirectoryInfo(countriesFoldersPaths[i]);
            countriesFoldersNames[i] = d.Name; // get only the name of folders
        }

        countries = new Country[countriesFoldersNames.Length];

        for (int i = 0; i < countriesFoldersNames.Length; i++)
        {
            countries[i].name = countriesFoldersNames[i];
            countries[i].flag = Resources.LoadAll<Sprite>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Flag")[0];
            countries[i].background = Resources.LoadAll<Sprite>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\BackgroundImage")[0];
            countries[i].beginnerAnimations = Resources.LoadAll<Animation>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Clips\\Beginner");
            countries[i].intermediateAnimations = Resources.LoadAll<Animation>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Clips\\Intermediate");
            countries[i].expertAnimations = Resources.LoadAll<Animation>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Clips\\Expert");
        }

        selectedContryID = countries.Length / 2;
    }

    /// <summary>
    /// Get the cantry with some id. If dont give id then get the selected country
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public static Country GetCountry(int ID = -1)
    {
        if (ID < 0)
        {
            ID = selectedContryID;
        }

        return countries[ID];
    }

    public static int GoToNextCountry()
    {
        selectedContryID++;
        if (selectedContryID > (countries.Length - 1))
        {
            selectedContryID = 0;
        }
        return selectedContryID;
    }

    public static int GoToPrevCountry()
    {
        selectedContryID--;
        if (selectedContryID < 0)
        {
            selectedContryID = countries.Length - 1;
        }
        return selectedContryID;
    }

}
public struct Country
{
    public string name;
    public Sprite flag;
    public Sprite background;
    public Animation[] beginnerAnimations;
    public Animation[] intermediateAnimations;
    public Animation[] expertAnimations;
}