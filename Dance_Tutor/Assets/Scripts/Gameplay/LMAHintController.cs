using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LMAHintController : MonoBehaviour {

    public Text txtField;

    private void Awake()
    {

    }

    public void SetMsg(string msg)
    {
        txtField.text = msg;
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
