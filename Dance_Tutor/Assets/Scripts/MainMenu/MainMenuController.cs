using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    [Header("UI Compnents")]
    public InputField uiNameCompnent;
    public InputField uiEmailCompnent;
    public InputField uiDateOfBirthCompnent;
    public Toggle uiSexMaleCompnent;
    public Toggle uiSexFamaleCompnent;
    public Toggle uiSexOtherCompnent;
    public Dropdown uiExpirienceCompnent;
    public InputField uiCountryCompnent;

    [Space(20)]
    public GameObject uiUsersPanel;

    [Header("Prefabs")]
    public GameObject userUIPrefab;

    // Set ui element in correct fields
    private string uiName
    {
        get
        {
            return uiNameCompnent.text;
        }
    }

    private string uiEmail
    {
        get
        {
            return uiEmailCompnent.text;
        }
    }

    private string uiDateOfBirth
    {
        get
        {
            return uiDateOfBirthCompnent.text;
        }
    }

    private Sex uiSex
    {
        get
        {
            if (uiSexMaleCompnent.isOn) { return Sex.Male; }
            else if (uiSexFamaleCompnent.isOn) { return Sex.Female; }
            else { return Sex.Other; }
        }
    }

    private Experience uiExpirience
    {
        get
        {
            return (Experience) uiExpirienceCompnent.value;
        }
    }

    private string uiCountry
    {
        get
        {
            return uiCountryCompnent.text;
        }
    }

    private void Awake()
    {
        FillExperienceDropdownList();
        DataEditor.LoadGameData();
    }

    // Use this for initialization
    void Start () {
        AddAllUserToMenu();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RegisterUser()
    {
        if((uiName!= "") && (uiEmail != "") && (uiDateOfBirth != "") && (uiCountry != ""))
        {
            User newUser = null;
            newUser = DataEditor.AddNewUser(uiName, uiEmail, uiDateOfBirth, uiSex, uiExpirience, uiCountry);
            if (newUser != null)
            {
                // User added
                AddAllUserToMenu();
            }
            else
            {
                Debug.Log("*** Your email already exists ***");
            }
        }
        else
        {
            Debug.Log("*** Some of field is empty ***");
        }
    }

    // Fill the dropDownLisr
    private void FillExperienceDropdownList()
    {
        // Clean prev options 
        uiExpirienceCompnent.ClearOptions();

        // Fill new options
        List<string> experienceDropdownList = new List<string>();
        foreach (Experience exp in Enum.GetValues(typeof(Experience)))
        {
            experienceDropdownList.Add(exp.ToString());
        }
        uiExpirienceCompnent.AddOptions(experienceDropdownList);
    }


    private void AddAllUserToMenu()
    {
        // Clean user panel
        CleanUIUsersPanel();

        // Add all user in users panel (ui)
        foreach (User u in DataEditor.gameData.Users)
        {
            GameObject go = Instantiate(userUIPrefab, uiUsersPanel.transform);
            UserUIComponentController goScript = go.GetComponent<UserUIComponentController>();
            if (goScript != null)
            {
                goScript.SetUIComponents(u.name, u.email);
            }
        }
    }

    private void CleanUIUsersPanel()
    {
        foreach (Transform child in uiUsersPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Before application quit save all data
    /// </summary>
    void OnApplicationQuit()
    {
        DataEditor.SaveGameData();
    }
}
