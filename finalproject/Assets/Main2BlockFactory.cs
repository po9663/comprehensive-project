using UnityEngine;
using System.Collections;
using System;

public class Main2BlockFactory
{

    //public GameObject BLOCKSMALL;
    //public GameObject BLOCKSTICK;
    //public GameObject BLOCKL;
    //public GameObject BLOCKSQUARE;
    GameObject[] blockTypes;
    Material[] materialTypes;
    public string box = "";
    public int cnt = 0;
    public ButtonClick buttonClick;
    public Main2BlockFactory()
    {

    }
    public Main2BlockFactory(GameObject a, GameObject b, GameObject c, GameObject d, GameObject e, GameObject f, GameObject g, GameObject h, GameObject i, GameObject j,
                        Material k, Material l, Material m)
    {
        blockTypes = new GameObject[10];
        blockTypes[0] = a;
        blockTypes[1] = b;
        blockTypes[2] = c;
        blockTypes[3] = d;
        blockTypes[4] = e;
        blockTypes[5] = f;
        blockTypes[6] = g;
        blockTypes[7] = h;
        blockTypes[8] = i;
        blockTypes[9] = j;

        materialTypes = new Material[4];
        materialTypes[0] = k;
        materialTypes[1] = l;
        materialTypes[3] = m;

        buttonClick = new ButtonClick();
    }
    


    public GameObject GetNextBlock()
    {
        GameObject[] block = ListPlay();

        try
        {
            foreach (MeshRenderer mr in block[cnt].GetComponentsInChildren<MeshRenderer>())
            {
                Debug.Log("BlockFactory here");
                mr.material = materialTypes[cnt];
            }
            
        }
        catch(Exception ex)
        {
            Debug.Log("Error!!"+ex.Message);
        }
        box = block[cnt].ToString().Substring(0, 6);
        Debug.Log("팩토리에서의" + box);
        return block[cnt];
        cnt++;
    }

    public GameObject[] ListPlay()
    {
        GameObject[] gobj;
        Debug.Log("ListPlay");
        string list = buttonClick.CurrentList();
        string[] lists = list.Split(',');
        gobj = new GameObject[lists.Length];
        for (int i = 0; i < lists.Length; i++)
        {
            Debug.Log(i + "번째" + lists[i]);

            gobj[i] = blockTypes[int.Parse(lists[i])]; Debug.Log("좀 돼라");
        }
        return gobj;
    }

    public string CurrentBox()
    {
        return box;
    }

}
