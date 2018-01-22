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
        Student = 1
    }

    private bool animationIsActive = false;

    private Animator animator;

    void Awake()
    {
        animator = this.GetComponent<Animator>();
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

        if (WordsManager.writeWords == true)
        {
            skeleton.AutoAddFrameValuesForEachJoint(this.transform);
        }
    }

    public void AnimationStart() //call from animation
    {
        if (avatarRole == Role.Tutor)
        {
            animationIsActive = true;
            WordsManager.StartWriteWords();
        }
    } 

    public void AnimationStop() //call from animation
    {
        if (avatarRole == Role.Tutor)
        {
            GameManager.instance.ChoreographyFinished();
            animationIsActive = false;
            WordsManager.StopWriteWords();
        }
    }
}
