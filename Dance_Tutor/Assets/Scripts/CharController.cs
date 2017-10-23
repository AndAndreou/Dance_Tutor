using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {

    [Header("Skeleton")]
    public SkeletonAndreas skeleton;

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
	void Update () {

        // if current animation in animator is the selected animation then in each frame write the values for each join
        if((animator.GetCurrentAnimatorStateInfo(0).IsName(animations[selectedAnimationIndex].name)))
        {
            skeleton.AutoAddFrameValuesForEachJoint();
        }		
	}
}
