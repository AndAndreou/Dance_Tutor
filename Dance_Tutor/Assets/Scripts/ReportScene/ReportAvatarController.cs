using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportAvatarController : MonoBehaviour {

    public float timeForNextAnimationFrame = 0f;
    public ScreenShotsController screenShotController;

    private GameObject avatar;
    private Animation avatarAnimation;
    private float remainingTime = 0f;
    private int frameIndex = 0;

    [HideInInspector]
    public bool screenShotsIsDone = false;

    private int totalWordsFrame = 0;

    AnimationClip previewAnimation;

    // Use this for initialization
    void Start () {
        avatar = this.gameObject;

        // Get/Set Animation component to avatar
        avatarAnimation = avatar.GetComponent<Animation>();
        if (avatarAnimation == null)
        {
            avatarAnimation = avatar.AddComponent<Animation>();
        }

        PreviewSelectedAnimationClip();

        totalWordsFrame = DataEditor.selectedUser.GetTheLastDanceHistory().wordsTimers.Count;
    }
	
	// Update is called once per frame
	void Update () {

        PauseAnimation();

        if ((remainingTime <= 0) && (frameIndex < totalWordsFrame))
        {
            SetAnimationInNextFrameByTime();
        }
        remainingTime -= Time.deltaTime;
	}

    private void PreviewSelectedAnimationClip()
    {
        previewAnimation = DataEditor.GetLastAnimationClipInHistoryOfSelectedPlayer();
        if(previewAnimation == null)
        {
            return;
        }
        previewAnimation.legacy = true;
        avatarAnimation.AddClip(previewAnimation, previewAnimation.name);
        avatarAnimation.Play(previewAnimation.name);
       
        Debug.Log(previewAnimation.name);
    }

    private void PauseAnimation()
    {
        if ((avatarAnimation.isPlaying) && (avatarAnimation[previewAnimation.name].speed!=0))
        {
            screenShotController.TakeShot();
            avatarAnimation[previewAnimation.name].speed = 0;
        }
    }

    private void SetAnimationInNextFrameByTime()
    {
        avatarAnimation[previewAnimation.name].time = DataEditor.selectedUser.GetTheLastDanceHistory().wordsTimers[frameIndex];
        avatarAnimation.Play(previewAnimation.name);
        avatarAnimation[previewAnimation.name].speed = 1;

        frameIndex++;
        if(frameIndex >= totalWordsFrame)
        {
            screenShotsIsDone = true;
        }
        remainingTime = timeForNextAnimationFrame;
    }
}
