using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

    [Header("Skeleton")]
    public Skeleton skeleton;

    [Header("Animation")]
    public int selectedAnimationIndex;
    public AnimationClip[] animations;

    private Animator animator;

    void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void LateUpdate () {

        if (WordsManager.writeWords == true)
        {
            skeleton.AutoAddFrameValuesForEachJoint(this.transform);
        }
    }

    public void AnimationStart()
    {
        WordsManager.StartWriteWords();
    } 

    public void AnimationStop()
    {
        WordsManager.StopWriteWords();
    }
}
