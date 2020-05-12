using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton
{
    private static Singleton instance = null;
    public static Singleton Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }
    }

    private Singleton()
    {
        
    }
    public string strList = "";
    public int number = 0;
}
