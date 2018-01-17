using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUIComponentController : MonoBehaviour {

    public Image uiImageComponent;
    public Text uiNameComponent;
    public string email; // Use it as ID

    private Sprite uiImage
    {
        get
        {
            return uiImageComponent.sprite;
        }
        set
        {
            uiImageComponent.sprite = value;
        }
    }

    private string uiName
    {
        get
        {
            return uiNameComponent.text;
        }
        set
        {
            uiNameComponent.text = value;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetUIComponents(string name, string email, Sprite photo)
    {
        uiName = name;
        this.email = email;
        uiImage = photo;
    }
}
