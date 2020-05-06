
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{

    public GameObject inputField;
    public GameObject btn;
    public GameObject Canvas;
    public Text textList;
    
    public void OnClick()
    {
        
        Singleton.Instance.strList = textList.text.ToString();
        //inputField.SetActive(false);
        //btn.SetActive(false);
        Invoke("StartGame",0.5f);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("main2");
    }
    
}
