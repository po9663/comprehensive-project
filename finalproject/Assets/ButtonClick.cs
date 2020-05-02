
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
    string str = "";
    public List<string> list;
    Main2BlockMovement main2BlockMoveMent;
    public ButtonClick()
    {
        
    }
    void Awake()
    {
        //DontDestroyOnLoad(Canvas);
    }
    void Start()
    {
        list = new List<string>();
    }
    public void OnClick()
    {
        
        str = textList.text.ToString();
        list.Add(str);
        foreach(string s in list)
        {
            Debug.Log("ArrayList = "+s);
        }
        //inputField.SetActive(false);
        //btn.SetActive(false);
        Invoke("StartGame",0.5f);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("main2",LoadSceneMode.Additive);
    }
    public List<string> CurrentList()
    {
        foreach (string s in list)
        {
            Debug.Log("ArrayList2222 = " + s);
        }
        return list;
    }
}
