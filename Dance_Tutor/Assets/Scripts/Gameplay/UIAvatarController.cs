using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAvatarController : MonoBehaviour {

    [Header("UIComponent")]
    public Image headUIComponent;
    public Image chestUIComponent;
    public Image spineUIComponent;
    public Image rightThighUIComponent;
    public Image rightFootUIComponent;
    public Image leftThighUIComponent;
    public Image leftFootUIComponent;
    public Image rightShoulderUIComponent;
    public Image rightHandUIComponent;
    public Image leftShoulderUIComponent;
    public Image leftHandUIComponetn;

    [Header("Materials")]
    public Material correctMaterial;
    public Material wrongMaterial;

    [HideInInspector]
    public Skeleton.StyleWord lastStyleWord;
    [HideInInspector]
    public List<Vector3[]> lastMotionWord;

    private Material headMaterial
    {
        get
        {
           return headUIComponent.material;
        }
        set
        {
            headUIComponent.material = value;
        }
    }

    private Material chestMaterial
    {
        get
        {
            return chestUIComponent.material;
        }
        set
        {
            chestUIComponent.material = value;
        }
    }

    private Material spineMaterial
    {
        get
        {
            return spineUIComponent.material;
        }
        set
        {
            spineUIComponent.material = value;
        }
    }

    private Material rightThighMaterial
    {
        get
        {
            return rightThighUIComponent.material;
        }
        set
        {
            rightThighUIComponent.material = value;
        }
    }

    private Material rightFootMaterial
    {
        get
        {
            return rightFootUIComponent.material;
        }
        set
        {
            rightFootUIComponent.material = value;
        }
    }

    private Material leftThighMaterial
    {
        get
        {
            return leftThighUIComponent.material;
        }
        set
        {
            leftThighUIComponent.material = value;
        }
    }

    private Material leftFootMaterial
    {
        get
        {
            return leftFootUIComponent.material;
        }
        set
        {
            leftFootUIComponent.material = value;
        }
    }

    private Material rightShoulderMaterial
    {
        get
        {
            return rightShoulderUIComponent.material;
        }
        set
        {
            rightShoulderUIComponent.material = value;
        }
    }

    private Material rightHandMaterial
    {
        get
        {
            return rightHandUIComponent.material;
        }
        set
        {
            rightHandUIComponent.material = value;
        }
    }

    private Material leftShoulderMaterial
    {
        get
        {
            return leftShoulderUIComponent.material;
        }
        set
        {
            leftShoulderUIComponent.material = value;
        }
    }

    private Material leftHandMaterial
    {
        get
        {
            return leftHandUIComponetn.material;
        }
        set
        {
            leftHandUIComponetn.material = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Get style and motion word and check whos joint is out of threshold and put wrong material in uiAvatar
        // What is the threshold? 

	}
}
