using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    [Header("UI Compnents")]
    public Image uiPhotoCompnent;
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

    [Space(20)]
    public GameObject uiUsersPhotosPanel;

    [Space(20)]
    public GameObject uiLoadingPanel;
    public Text uiLoadingTxt;

    [Header("Buttons")]
    public Button selectUserButton;
    public Button modifyUserButton;

    [Header("Prefabs")]
    public GameObject userUIPrefab;
    public GameObject userPhotoOptionPrefab;

    // Set ui element in correct fields
    private Sprite uiPhoto
    {
        get
        {
            return uiPhotoCompnent.sprite;
        }
        set
        {
            uiPhotoCompnent.sprite = value;
        }
    }

    private string uiName
    {
        get
        {
            return uiNameCompnent.text;
        }
        set
        {
            uiNameCompnent.text = value;
        }
    }

    private string uiEmail
    {
        get
        {
            return uiEmailCompnent.text;
        }
        set
        {
            uiEmailCompnent.text = value;
        }
    }

    private string uiDateOfBirth
    {
        get
        {
            return uiDateOfBirthCompnent.text;
        }
        set
        {
            uiDateOfBirthCompnent.text = value;
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
        set
        {
            uiSexMaleCompnent.isOn = false;
            uiSexFamaleCompnent.isOn = false;
            uiSexOtherCompnent.isOn = false;

            if (value == Sex.Male)
                uiSexMaleCompnent.isOn = true;
            else if (value == Sex.Female)
                uiSexFamaleCompnent.isOn = true;
            else
                uiSexOtherCompnent.isOn = true;
        }
    }

    private Experience uiExpirience
    {
        get
        {
            return (Experience) uiExpirienceCompnent.value;
        }
        set
        {
            uiExpirienceCompnent.value = (int)value;
        }
    }

    private string uiCountry
    {
        get
        {
            return uiCountryCompnent.text;
        }
        set
        {
            uiCountryCompnent.text = value;
        }
    }

    private void Awake()
    {
        //DontDestroyOnLoad(this);

        FillExperienceDropdownList();
        if (FindObjectOfType<DataEditor>() == null)
        {
            DataEditor.LoadGameData();
        }
    }

    // Use this for initialization
    void Start () {
        AddAllUserToMenu();
        FillPhotoPanel();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RegisterUserButton()
    {
        if((uiName!= "") && (uiEmail != "") && (uiDateOfBirth != "") && (uiCountry != ""))
        {
            User newUser = null;
            newUser = DataEditor.AddNewUser(uiPhoto.name,uiName, uiEmail, uiDateOfBirth, uiSex, uiExpirience, uiCountry);
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

    /// <summary>
    /// Fill the users panel with alrady registed users
    /// </summary>
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
                goScript.SetUIComponents(u.name, u.email, Resources.Load<Sprite>("UsersImages\\" + u.photoName));
                go.GetComponent<Button>().onClick.AddListener(() => UserButton(u.email));
            }
        }
    }

    /// <summary>
    /// Clean all the users panel
    /// </summary>
    private void CleanUIUsersPanel()
    {
        foreach (Transform child in uiUsersPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// User select his/her account
    /// </summary>
    /// <param name="userEmail"></param>
    public void UserButton(string userEmail)
    {
        DataEditor.SetSelectedUser(userEmail);
        FillFormWithSelectedUserInfo();

        selectUserButton.gameObject.SetActive(true);
        modifyUserButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Fill form with user infos
    /// </summary>
    private void FillFormWithSelectedUserInfo()
    {
        SelectUserPhotoButton(DataEditor.selectedUser.photoName);
        uiName = DataEditor.selectedUser.name;
        uiEmail = DataEditor.selectedUser.email;
        uiDateOfBirth = DataEditor.selectedUser.dateOfBirth;
        uiSex = DataEditor.selectedUser.sex;
        uiExpirience = DataEditor.selectedUser.expirience;
        uiCountry = DataEditor.selectedUser.country;
    }

    /// <summary>
    /// Modify information of selected user
    /// </summary>
    public void ModifyUserInfos()
    {
        DataEditor.UpdateSelectedUser(uiPhoto.name, uiName, uiDateOfBirth, uiSex, uiExpirience, uiCountry);
        AddAllUserToMenu();
    }

    /// <summary>
    /// Select user and go to the next scene
    /// </summary>
    public void SelectUserButton()
    {
        LoadLevel("SelectAnimationClip");
    }

    /// <summary>
    /// Put the photo button to change profile photo
    /// </summary>
    public void ChangeUserPhotoButton()
    {
        uiUsersPhotosPanel.SetActive(!uiUsersPhotosPanel.activeSelf);
    }

    /// <summary>
    /// Fill panel of user photo options with sprite from resources
    /// </summary>
    private void FillPhotoPanel()
    {
        Sprite[] userPhotosOptions = Resources.LoadAll<Sprite>("UsersImages");
        foreach(Sprite s in userPhotosOptions)
        {
            GameObject userPhotoOption = Instantiate(userPhotoOptionPrefab, uiUsersPhotosPanel.transform);
            userPhotoOption.GetComponent<Image>().sprite = s;
            userPhotoOption.GetComponent<Button>().onClick.AddListener(() => SelectUserPhotoButton(s.name));
        }
    }

    /// <summary>
    /// User select a photofrom options
    /// </summary>
    /// <param name="spriteName"></param>
    public void SelectUserPhotoButton(string spriteName)
    {
        uiPhoto = Resources.Load<Sprite>("UsersImages\\" + spriteName);
        uiUsersPhotosPanel.SetActive(false);
    }

    /// <summary>
    /// Before application quit save all data
    /// </summary>
    void OnApplicationQuit()
    {
        DataEditor.SaveGameData();
    }

    public void LoadLevel(string sceneName) //The name of the scene
    {
        DataEditor.SaveGameData();
        StartCoroutine(LevelCoroutine(sceneName));
    }

    IEnumerator LevelCoroutine(string sceneName)
    {
        uiLoadingPanel.SetActive(true);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            uiLoadingTxt.text = (int)(async.progress * 100) + "%";
            yield return null;

        }
    }
}
