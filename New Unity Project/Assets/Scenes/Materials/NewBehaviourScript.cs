using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
    }
    

    public float moveSpeed = 10.0f;
    // Update is called once per frame
    void Update()
    {
        
        
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Debug.Log(moveHorizontal + "===" + moveVertical);
            Vector3 moveVector = new Vector3(moveHorizontal, moveVertical, 0.1f);
            transform.position += moveVector * moveSpeed * Time.deltaTime;

        
        
    }
}
