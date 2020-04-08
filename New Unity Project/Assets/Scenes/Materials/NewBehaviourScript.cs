using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    
    
    public float moveSpeed = 10.0f;
    // Start is called before the first frame update

    void Awake()
    {
        Debug.Log("awake");
    }
    
    IEnumerator Start()
    {
        Debug.Log("start");
        yield return new WaitForSeconds(1);
        int rand = 0;
        GameObject[] boxes;

        boxes = GameObject.FindGameObjectsWithTag("1");
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].SetActive(false);
            Debug.Log(i + "번째" + boxes[i]);
        }
        rand = Random.Range(0, 5);
        boxes[rand].SetActive(true);
    }
    

    
    
    // Update is called once per frame
    void Update()
    {


        

        /*
        float moveX = Input.GetAxis("Horizontal");
        float moveY = 0.0f;
        float moveZ = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            moveY += 0.5f;
            Vector3 moveVector = new Vector3(moveX, moveY, moveZ);
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            moveY -= 1.0f;
            Vector3 moveVector = new Vector3(moveX, moveY, moveZ);
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }
        else
        {
            Vector3 moveVector = new Vector3(moveX, moveY, moveZ);
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }

        */
        moving();
    }
    
    
    void moving()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = 0.0f;
        float moveZ = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Q))
        {
            moveY += 0.5f;
            Vector3 moveVector = new Vector3(moveX, moveY, moveZ);
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            moveY -= 1.0f;
            Vector3 moveVector = new Vector3(moveX, moveY, moveZ);
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {

        }
        else
        {
            Vector3 moveVector = new Vector3(moveX, moveY, moveZ);
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }
    }

    
}
