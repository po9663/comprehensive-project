using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonClick : MonoBehaviour
{
    Main2BlockFactory main2BlockFactory;
    Main2BlockMovement main2BlockMovement;
    public GameObject inputField;
    public GameObject btn;
    public Text textList;
    string str = "";
    public bool isStart = false;
    public ButtonClick()
    {

    }
    public void OnClickList()
    {
        main2BlockFactory = new Main2BlockFactory();
        main2BlockMovement = new Main2BlockMovement();
        str = textList.text.ToString();
        Debug.Log(textList.text);
        isStart = true;
        inputField.SetActive(false);
        btn.SetActive(false);
    }
    public string CurrentList()
    {
        return str;
    }
    public bool ListStart()
    {
        return isStart;
    }
}
