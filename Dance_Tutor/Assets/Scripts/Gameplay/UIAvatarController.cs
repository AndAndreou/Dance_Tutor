using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAvatarController : MonoBehaviour {

    [Header("UIComponent")]
    public Image headUIComponent;
    public Image[] chestUIComponent = new Image[3];
    public Image spineUIComponent;
    public Image rightThighUIComponent;
    public Image rightFootUIComponent;
    public Image leftThighUIComponent;
    public Image leftFootUIComponent;
    public Image rightShoulderUIComponent;
    public Image rightHandUIComponent;
    public Image leftShoulderUIComponent;
    public Image leftHandUIComponetn;

    [Header("Colors")]
    public Color correctColor;
    public Color wrongColor;

    private float motionWordThreshold;

    [HideInInspector]
    public Vector3[] lastMotionWord;
    private Vector3[] prevLastMotionWord;

    private Color headColor
    {
        get
        {
           return headUIComponent.color;
        }
        set
        {
            headUIComponent.color = value;
        }
    }

    private Color chestColor
    {
        get
        {
            return chestUIComponent[0].color;
        }
        set
        {
            chestUIComponent[0].color = value;
            chestUIComponent[1].color = value;
            chestUIComponent[2].color = value;
        }
    }

    private Color spineColor
    {
        get
        {
            return spineUIComponent.color;
        }
        set
        {
            spineUIComponent.color = value;
        }
    }

    private Color rightThighColor
    {
        get
        {
            return rightThighUIComponent.color;
        }
        set
        {
            rightThighUIComponent.color = value;
        }
    }

    private Color rightFootColor
    {
        get
        {
            return rightFootUIComponent.color;
        }
        set
        {
            rightFootUIComponent.color = value;
        }
    }

    private Color leftThighColor
    {
        get
        {
            return leftThighUIComponent.color;
        }
        set
        {
            leftThighUIComponent.color = value;
        }
    }

    private Color leftFootColor
    {
        get
        {
            return leftFootUIComponent.color;
        }
        set
        {
            leftFootUIComponent.color = value;
        }
    }

    private Color rightShoulderColor
    {
        get
        {
            return rightShoulderUIComponent.color;
        }
        set
        {
            rightShoulderUIComponent.color = value;
        }
    }

    private Color rightHandColor
    {
        get
        {
            return rightHandUIComponent.color;
        }
        set
        {
            rightHandUIComponent.color = value;
        }
    }

    private Color leftShoulderColor
    {
        get
        {
            return leftShoulderUIComponent.color;
        }
        set
        {
            leftShoulderUIComponent.color = value;
        }
    }

    private Color leftHandColor
    {
        get
        {
            return leftHandUIComponetn.color;
        }
        set
        {
            leftHandUIComponetn.color = value;
        }
    }

    private float headColorValue;
    private float chestColorValue;
    private float spineColorValue;
    private float rightThighColorValue;
    private float rightFootColorValue;
    private float leftThighColorValue;
    private float leftFootColorValue;
    private float rightShoulderColorValue;
    private float rightHandColorValue;
    private float leftShoulderColorValue;
    private float leftHandColorValue;


    // Use this for initialization
    void Start () {

        prevLastMotionWord = lastMotionWord;
        motionWordThreshold = DataEditor.uiMotionFeedBackThreshold;

    }
	
	// Update is called once per frame
	void Update ()
    {

        if (prevLastMotionWord!= lastMotionWord)
        {
            // We have new data
            CalculateWrongs();
        }
        prevLastMotionWord = lastMotionWord;

    }

    private void CalculateWrongs()
    {
        // Get motion word and check whos joint is out of threshold and put wrong color in uiAvatar
        // What is the threshold? 
        float jointAvg = 0;
        int jointIndex = 0;

        // headColorValue
        jointIndex = 16;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f)/WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            headColorValue = 1;
        }
        else
        {
            headColorValue = 0;
        }

        // chestColorValue
        jointIndex = 12;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f) / WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            chestColorValue = 1;
        }
        else
        {
            chestColorValue = 0;
        }

        // spineColorValue
        jointIndex = 1;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f) / WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            spineColorValue = 1;
        }
        else
        {
            spineColorValue = 0;
        }

        // rightThighColorValue
        jointIndex = 7;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f) / WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            rightThighColorValue = 1;
        }
        else
        {
            rightThighColorValue = 0;
        }

        // rightFootColorValue
        jointIndex = 8;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f) / WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            rightFootColorValue = 1;
        }
        else
        {
            rightFootColorValue = 0;
        }

        // leftThighColorValue
        jointIndex = 2;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f) / WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            leftThighColorValue = 1;
        }
        else
        {
            leftThighColorValue = 0;
        }

        // leftFootColorValue
        jointIndex = 3;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f) / WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            leftFootColorValue = 1;
        }
        else
        {
            leftFootColorValue = 0;
        }

        // rightShoulderColorValue
        jointIndex = 33;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f) / WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            rightShoulderColorValue = 1;
        }
        else
        {
            rightShoulderColorValue = 0;
        }

        // rightHandColorValue
        jointIndex = 34;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f) / WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            rightHandColorValue = 1;
        }
        else
        {
            rightHandColorValue = 0;
        }

        // leftShoulderColorValue
        jointIndex = 19;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f) / WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            leftShoulderColorValue = 1;
        }
        else
        {
            leftShoulderColorValue = 0;
        }

        // leftHandColorValue
        jointIndex = 20;
        jointAvg = ((lastMotionWord[jointIndex].x + lastMotionWord[jointIndex].y + lastMotionWord[jointIndex].z) / 3f) / WordsManager.instance.motionWordWindowSize;
        if (jointAvg > motionWordThreshold)
        {
            leftHandColorValue = 1;
        }
        else
        {
            leftHandColorValue = 0;
        }

        ReDrawUIAvatar();
    }

    private void ReDrawUIAvatar()
    {
        headColor = CalculateColor(headColorValue);
        chestColor = CalculateColor(chestColorValue);
        spineColor = CalculateColor(spineColorValue);
        rightThighColor = CalculateColor(rightThighColorValue);
        rightFootColor = CalculateColor(rightFootColorValue);
        leftThighColor = CalculateColor(leftThighColorValue);
        leftFootColor = CalculateColor(leftFootColorValue);
        rightShoulderColor = CalculateColor(rightShoulderColorValue);
        rightHandColor = CalculateColor(rightHandColorValue);
        leftShoulderColor = CalculateColor(leftShoulderColorValue);
        leftHandColor = CalculateColor(leftHandColorValue);
    }

    private Color CalculateColor( float v)
    {
        return Color.Lerp(correctColor, wrongColor, v);
    }
}
