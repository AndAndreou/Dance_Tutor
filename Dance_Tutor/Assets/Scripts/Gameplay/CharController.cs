using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

    [Header("Skeleton")]
    public Skeleton skeleton;

    [Header("Role")]
    public Role avatarRole;

    public enum Role
    {
        Tutor = 0,
        Student = 1,
        NPCDancer = 2,
        NPC = 3
    }

    private bool animationIsActive = false;

    private Animator animator;

    void Awake()
    {
        animator = this.GetComponent<Animator>();

        // Get animator and change the animation clip 
        AnimationControllerEditor();
    }

    // Use this for initialization
    void Start () {
    }

    private void Update()
    {
        //if (avatarRole == Role.Tutor)
        {
            if ((animationIsActive == false) && (GameManager.instance.showChoreography))
            {
                animator.SetTrigger("StartShowChoreography"); // start the animation
            }
        }
    }

    // Update is called once per frame
    void LateUpdate () {

        //if (WordsManager.writeWords == true)
        if (WordsManagerWithSync.writeWords == true)
        {
            skeleton.AutoAddFrameValuesForEachJoint(this.transform);
            //Debug.Log(gameObject.name + " ---> " + skeleton.frameMotions.Count);
        }
    }

    /// <summary>
    /// Modify animation controller. Put selected animation from prev scene and put the animation events to start and end of animation clip if the avatar is tutor
    /// </summary>
    private void AnimationControllerEditor()
    {
        if (((avatarRole == Role.Tutor) || (avatarRole == Role.NPCDancer)) && (GameManager.instance.useDataFromUser == true))
        {
            AnimationClip selectedAnimationClip = DataEditor.GetAnimationClip(); // Get selected animation

            Debug.Log("------> Tutor animation clip change to -> " + selectedAnimationClip.name);

            // Get current and create new animation contoller
            RuntimeAnimatorController currentAnimationController = animator.runtimeAnimatorController;
            AnimatorOverrideController newAnimatorController = new AnimatorOverrideController();
            newAnimatorController.runtimeAnimatorController = currentAnimationController;

            if (avatarRole == Role.Tutor)
            {
                // Create and add animation event for start of animation clip
                AnimationEvent animationEventStart = new AnimationEvent();
                animationEventStart.time = 0;
                animationEventStart.functionName = "AnimationStart";
                selectedAnimationClip.AddEvent(animationEventStart);

                /// Create and add animation event for end of animation clip
                AnimationEvent animationEventFinish = new AnimationEvent();
                animationEventFinish.time = selectedAnimationClip.length;
                animationEventFinish.functionName = "AnimationStop";
                selectedAnimationClip.AddEvent(animationEventFinish);
            }

            // Replace current animation with new one ( new = selected animation + start & stop animation event)
            newAnimatorController["Antonis3_1"] = selectedAnimationClip;
            // Replace current controller with new
            animator.runtimeAnimatorController = newAnimatorController;
        }
    }

    public void AnimationStart() //call from animation in first frame
    {
        if (avatarRole == Role.Tutor)
        {
            //WordsManager.StartWriteWords();
            WordsManagerWithSync.StartWriteWords();
        }
        animationIsActive = true;
    } 

    public void AnimationStop() //call from animation in last frame
    {
        if (avatarRole == Role.Tutor)
        {
            GameManager.instance.ChoreographyFinished();
            //WordsManager.StopWriteWords();
            WordsManagerWithSync.StopWriteWords();
        }
        animationIsActive = false;
    }
}
