using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectionFlagController : MonoBehaviour {

    [Header("UIComponents")]
    public Image[] uiFlags;
    [Space(20)]
    public Image uiBackgroundImg;

    private Animator animator;

    private bool enableScroll;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        ResetFlagsUI();

        enableScroll = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoLeft()
    {
        if (enableScroll)
        {
            SelectAnimationSceneController.GoToNextCountry();

            animator.SetTrigger("GoLeft");

            enableScroll = false;
        }
    }

    public void GoRight()
    {
        if (enableScroll)
        {
            SelectAnimationSceneController.GoToPrevCountry();

            animator.SetTrigger("GoRight");

            enableScroll = false;
        }
    }

    public void ResetFlagsUI()
    {
        // Set the middle flag
        int middleFlagID = (uiFlags.Length / 2);
        Country selectedCountry = SelectAnimationSceneController.GetCountry();
        uiFlags[middleFlagID].sprite = selectedCountry.flag;

        // Set background
        uiBackgroundImg.sprite = selectedCountry.background;

        //Debug.Log(SelectAnimationSceneController.countries[SelectAnimationSceneController.selectedContryID].name);

        // Set other flags
        int prevFlagID = middleFlagID - 1;
        int prevSelectedCountryID = SelectAnimationSceneController.selectedContryID - 1;
        int nextFlagID = middleFlagID + 1;
        int nextSelectedCountryID = SelectAnimationSceneController.selectedContryID + 1;
        for (int i =0;i< middleFlagID; i++)
        {
            if (prevFlagID < 0)
            {
                prevFlagID = uiFlags.Length - 1;
            }
            if (prevSelectedCountryID < 0)
            {
                prevSelectedCountryID = SelectAnimationSceneController.countries.Length - 1;
            }
            uiFlags[prevFlagID].sprite = SelectAnimationSceneController.countries[prevSelectedCountryID].flag;
            prevFlagID--;
            prevSelectedCountryID--;

            if (nextFlagID > uiFlags.Length - 1)
            {
                nextFlagID = 0;
            }
            if (nextSelectedCountryID > SelectAnimationSceneController.countries.Length - 1)
            {
                nextSelectedCountryID = 0;
            }
            uiFlags[nextFlagID].sprite = SelectAnimationSceneController.GetCountry(nextSelectedCountryID).flag;
            nextFlagID++;
            nextSelectedCountryID++;

            enableScroll = true;
        }
    }

}
