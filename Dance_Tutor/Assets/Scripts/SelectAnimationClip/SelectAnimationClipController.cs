using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAnimationClipController : MonoBehaviour {

    [Header("UIComponents")]
    public Text[] uiAnimationClipName;

    [Header("Avatar")]
    public GameObject avatar;

    [Header("Animations")]
    public AnimationClip idleAnimation;

    private Animation avatarAnimation;

    private Animator animator;

    private bool enableScroll;

    private SelectAnimationSceneController selectAnimationSceneController;

    // Use this for initialization
    void Start()
    {
        selectAnimationSceneController = FindObjectOfType<SelectAnimationSceneController>();
        animator = GetComponent<Animator>();
        ResetAnimationsUI();

        enableScroll = true;

        //add listener for events
        selectAnimationSceneController.ChangeSelectedCountryEvent.AddListener(ChangeSelectedCountry);
        selectAnimationSceneController.ChangeSelectedAnimationClipEvent.AddListener(PreviewSelectedAnimationClip);

        // Get/Set Animation component to avatar
        avatarAnimation = avatar.GetComponent<Animation>();
        if(avatarAnimation == null)
        {
            avatarAnimation = avatar.AddComponent<Animation>();
        }
        idleAnimation.legacy = true;
        avatarAnimation.AddClip(idleAnimation, idleAnimation.name);
        avatarAnimation.Play(idleAnimation.name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoUp()
    {
        if (enableScroll)
        {
            selectAnimationSceneController.GoToNextAnimationClip();

            animator.SetTrigger("GoUp");

            enableScroll = false;
        }
    }

    public void GoDown()
    {
        if (enableScroll)
        {
            selectAnimationSceneController.GoToPrevAnimationClip();

            animator.SetTrigger("GoDown");

            enableScroll = false;
        }
    }

    public void ResetAnimationsUI()
    {
        // Set the middle animation
        int middleAnimationClipID = (uiAnimationClipName.Length / 2);
        AnimationClip selectedAnimationClip = DataEditor.GetAnimationClip();
        uiAnimationClipName[middleAnimationClipID].text = selectedAnimationClip.name;
        uiAnimationClipName[middleAnimationClipID].color = GetColorOfExpirience();

        // Set other animation
        int prevAnimationClipID = middleAnimationClipID - 1;
        int prevSelectedAnimationClipID = DataEditor.selectedAnimationClipID - 1;
        int nextAnimationClipID = middleAnimationClipID + 1;
        int nextSelectedAnimationClipID = DataEditor.selectedAnimationClipID + 1;
        for (int i = 0; i < middleAnimationClipID; i++)
        {
            if (prevAnimationClipID < 0)
            {
                prevAnimationClipID = uiAnimationClipName.Length - 1;
            }
            if (prevSelectedAnimationClipID < 0)
            {
                prevSelectedAnimationClipID = (DataEditor.GetAnimationsClipsLength() - 1);
            }
            uiAnimationClipName[prevAnimationClipID].text = DataEditor.GetAnimationClip(prevSelectedAnimationClipID).name;
            uiAnimationClipName[prevAnimationClipID].color = GetColorOfExpirience(prevSelectedAnimationClipID);
            prevAnimationClipID--;
            prevSelectedAnimationClipID--;

            if (nextAnimationClipID > uiAnimationClipName.Length - 1)
            {
                nextAnimationClipID = 0;
            }
            if (nextSelectedAnimationClipID > (DataEditor.GetAnimationsClipsLength() - 1))
            {
                nextSelectedAnimationClipID = 0;
            }
            uiAnimationClipName[nextAnimationClipID].text = DataEditor.GetAnimationClip(nextSelectedAnimationClipID).name;
            uiAnimationClipName[nextAnimationClipID].color = GetColorOfExpirience(nextSelectedAnimationClipID);
            nextAnimationClipID++;
            nextSelectedAnimationClipID++;

            enableScroll = true;
        }
    }

    private void ChangeSelectedCountry()
    {
        ResetAnimationsUI();
    }

    private void PreviewSelectedAnimationClip()
    {
        AnimationClip previewAnimation = DataEditor.GetAnimationClip();
        previewAnimation.legacy = true;
        avatarAnimation.AddClip(previewAnimation, previewAnimation.name);
        avatarAnimation.Play(previewAnimation.name);


        Debug.Log(previewAnimation.name);
    }

    private Color GetColorOfExpirience(int animationID = -1)
    {
        Experience animationExpirience = DataEditor.GetExperineceChorographyOfSelectedCountry(animationID);
        switch (animationExpirience)
        {
            case Experience.Beginner:
                return Color.green;
            case Experience.Intermediate:
                return Color.yellow;
            case Experience.Expert:
                return Color.red;
            default:
                return Color.black;
        }
    }
}
