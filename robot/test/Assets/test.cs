
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Text;

using System.IO.Ports;
using UnityEngine;

public class test : MonoBehaviour {

    private SerialPort sp;
    public string message;
    void Start () {
        sp = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);
        sp.Open();
      
        


    }
	
	// Update is called once    per frame
	void Update () {
        if(sp.IsOpen)
        {
            sp.Write("150");
           int cc= sp.ReadByte();
            Debug.Log(cc);

          
        }
        else
        {
            sp.Close();
            Application.Quit();
        }
    }
   
}
