using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectionFlagController : MonoBehaviour {

    [Header("UIComponents")]
    public Image[] uiFlags;

    private Country[] countries;

    private int selectedContryID;

    private Animator animator;

    private bool enableScroll;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        LoadCountries();
        ResetUI();

        enableScroll = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoLeft()
    {
        if (enableScroll)
        {
            selectedContryID++;
            if (selectedContryID > (countries.Length - 1))
            {
                selectedContryID = 0;
            }
            animator.SetTrigger("GoLeft");

            enableScroll = false;
        }
    }

    public void GoRight()
    {
        if (enableScroll)
        {
            selectedContryID--;
            if (selectedContryID < 0)
            {
                selectedContryID = countries.Length - 1;
            }
            animator.SetTrigger("GoRight");

            enableScroll = false;
        }
    }

    public void ResetUI()
    {
        int middleFlagID = (uiFlags.Length / 2);
        uiFlags[middleFlagID].sprite = countries[selectedContryID].flag;

        Debug.Log(countries[selectedContryID].name);

        int prevFlagID = middleFlagID - 1;
        int prevSelectedCountryID = selectedContryID - 1;
        int nextFlagID = middleFlagID + 1;
        int nextSelectedCountryID = selectedContryID + 1;
        for (int i =0;i< middleFlagID; i++)
        {
            if (prevFlagID < 0)
            {
                prevFlagID = uiFlags.Length - 1;
            }
            if (prevSelectedCountryID < 0)
            {
                prevSelectedCountryID = countries.Length - 1;
            }
            uiFlags[prevFlagID].sprite = countries[prevSelectedCountryID].flag;
            prevFlagID--;
            prevSelectedCountryID--;

            if (nextFlagID > uiFlags.Length - 1)
            {
                nextFlagID = 0;
            }
            if (nextSelectedCountryID > countries.Length - 1)
            {
                nextSelectedCountryID = 0;
            }
            uiFlags[nextFlagID].sprite = countries[nextSelectedCountryID].flag;
            nextFlagID++;
            nextSelectedCountryID++;

            enableScroll = true;
        }
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
        
        for(int i = 0; i < countriesFoldersNames.Length; i++)
        {
            countries[i].name = countriesFoldersNames[i];
            countries[i].flag = Resources.LoadAll<Sprite>("Animations\\Countries\\" + countriesFoldersNames[i]+ "\\Flag")[0];
            countries[i].background = Resources.LoadAll<Sprite>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\BackgroundImage")[0];
            countries[i].beginnerAnimations = Resources.LoadAll<Animation>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Clips\\Beginner");
            countries[i].intermediateAnimations = Resources.LoadAll<Animation>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Clips\\Intermediate");
            countries[i].expertAnimations = Resources.LoadAll<Animation>("Animations\\Countries\\" + countriesFoldersNames[i] + "\\Clips\\Expert");
        }

        selectedContryID = countries.Length / 2;
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
