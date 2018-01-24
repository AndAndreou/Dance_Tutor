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

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        ResetFlagsUI();

        enableScroll = true;

        //add listener for events
        SelectAnimationSceneController.instance.ChangeSelectedCountryEvent.AddListener(ChangeSelectedCountry);
        SelectAnimationSceneController.instance.ChangeSelectedAnimationClipEvent.AddListener(PreviewSelectedAnimationClip);

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
            SelectAnimationSceneController.GoToNextAnimationClip();

            animator.SetTrigger("GoUp");

            enableScroll = false;
        }
    }

    public void GoDown()
    {
        if (enableScroll)
        {
            SelectAnimationSceneController.GoToPrevAnimationClip();

            animator.SetTrigger("GoDown");

            enableScroll = false;
        }
    }

    public void ResetFlagsUI()
    {
        // Set the middle flag
        int middleAnimationClipID = (uiAnimationClipName.Length / 2);
        AnimationClip selectedAnimationClip = SelectAnimationSceneController.GetAnimationClip();
        uiAnimationClipName[middleAnimationClipID].text = selectedAnimationClip.name;

        // Set other flags
        int prevAnimationClipID = middleAnimationClipID - 1;
        int prevSelectedAnimationClipID = SelectAnimationSceneController.selectedAnimationClipID - 1;
        int nextAnimationClipID = middleAnimationClipID + 1;
        int nextSelectedAnimationClipID = SelectAnimationSceneController.selectedAnimationClipID + 1;
        for (int i = 0; i < middleAnimationClipID; i++)
        {
            if (prevAnimationClipID < 0)
            {
                prevAnimationClipID = uiAnimationClipName.Length - 1;
            }
            if (prevSelectedAnimationClipID < 0)
            {
                prevSelectedAnimationClipID = (SelectAnimationSceneController.GetAnimationsClipsLength() - 1);
            }
            uiAnimationClipName[prevAnimationClipID].text = SelectAnimationSceneController.GetAnimationClip(prevSelectedAnimationClipID).name;
            prevAnimationClipID--;
            prevSelectedAnimationClipID--;

            if (nextAnimationClipID > uiAnimationClipName.Length - 1)
            {
                nextAnimationClipID = 0;
            }
            if (nextSelectedAnimationClipID > (SelectAnimationSceneController.GetAnimationsClipsLength() - 1))
            {
                nextSelectedAnimationClipID = 0;
            }
            uiAnimationClipName[nextAnimationClipID].text = SelectAnimationSceneController.GetAnimationClip(nextSelectedAnimationClipID).name;
            nextAnimationClipID++;
            nextSelectedAnimationClipID++;

            enableScroll = true;
        }
    }

    private void ChangeSelectedCountry()
    {
        ResetFlagsUI();
    }

    private void PreviewSelectedAnimationClip()
    {
        AnimationClip previewAnimation = SelectAnimationSceneController.GetAnimationClip();
        previewAnimation.legacy = true;
        avatarAnimation.AddClip(previewAnimation, previewAnimation.name);
        avatarAnimation.Play(previewAnimation.name);


        Debug.Log(previewAnimation.name);
    }
}
