/*




*/
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class BlockMovement : MonoBehaviour {

	public GameObject Block;
    public GameObject Block2;
    public GameObject Block3;
    public GameObject Block4;
    public GameObject Block5;
    public GameObject Block6;
    public GameObject Block7;
    public GameObject Block8;
    public GameObject Block9;
    public GameObject Block10;
    
	public Material MATRED;
	public Material MATBLUE;
	public Material MATGREEN;
    

    ArrayList boxes;
    BlockFactory blockFactory;
	GameObject activeBlock;
	float moveSpeed = 10f;
    //float defaultFallSpeed = 1f;
    //float fallSpeed = 1f;
    int cnt = 0;
	//float zNextCheckPoint = 1f;
    //bool isFalling = true;
    bool isMoving = false;
    bool isRotating = false;
    Vector3 blockStartPos, blockEndPos;
    Quaternion blockStartRot, blockEndRot;
    float tElapsed;
    public string currentBox = ""; // 현재박스의 이름
    public string dataSend = ""; //데이터 전송
	// Use this for initialization
    public BlockMovement()
    {

    }
	void Start () {
        // BlockFactory 클래스 객체생성
		blockFactory = new BlockFactory(Block, Block2, Block3, Block4, Block5, Block6, Block7, Block8, Block9, Block10
                                        , MATRED, MATBLUE, MATGREEN);
        //grid생성
        initGrid();
        // get the active block
        //activeBlock = GameObject.FindGameObjectWithTag("Player");
        //상자 생성 위치는 0,0,1위치
		activeBlock = (GameObject)GameObject.Instantiate(blockFactory.GetNextBlock(), new Vector3(0, 0, 1), Quaternion.identity);
		activeBlock.transform.position = new Vector3(-1, 2, 1);
		foreach (MeshRenderer mr in activeBlock.GetComponentsInChildren<MeshRenderer>())
		{
			Debug.Log("Start here");
			mr.material = MATBLUE;
		}
        boxes = new ArrayList();
	}
    
    

    // Update is called once per frame
    void Update()
    {
        if (!isMoving && !isRotating)
        {
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
				Vector3 futurePos = activeBlock.transform.position + new Vector3(-1, 0, 0);
				Quaternion futureRot = activeBlock.transform.rotation;
				if (!IsPositionBlocked(futurePos, futureRot)) {
					SmoothMove(activeBlock.transform.position, futurePos);
				}
                
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
				Vector3 futurePos = activeBlock.transform.position + new Vector3(1, 0, 0);
				Quaternion futureRot = activeBlock.transform.rotation;
				if (!IsPositionBlocked(futurePos, futureRot)) {
					SmoothMove(activeBlock.transform.position, futurePos);
				}
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
				Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, 1);
				Quaternion futureRot = activeBlock.transform.rotation;
				if (!IsPositionBlocked(futurePos, futureRot)) {
					SmoothMove(activeBlock.transform.position, futurePos);
				}
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
				Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, -1);
				Quaternion futureRot = activeBlock.transform.rotation;
				if (!IsPositionBlocked(futurePos, futureRot)) {
					SmoothMove(activeBlock.transform.position, futurePos);
				}
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, 1);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, -1);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3 futurePos = activeBlock.transform.position + new Vector3(-1, 0, 0);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Vector3 futurePos = activeBlock.transform.position + new Vector3(1, 0, 0);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 1, 0);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, -1, 0);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
            }


            /*
			else if (Input.GetKeyDown(KeyCode.Q))
			{
				Vector3 futurePos = activeBlock.transform.position;
				Quaternion futureRot = Quaternion.Euler(0, 0, 90) * activeBlock.transform.rotation;
				if (!IsPositionBlocked(futurePos, futureRot)) {
					SmoothRotate(activeBlock.transform.rotation, futureRot);
				}
			}
            else if (Input.GetKeyDown(KeyCode.W))
            {
				Vector3 futurePos = activeBlock.transform.position;
				Quaternion futureRot = Quaternion.Euler(90, 0, 0) * activeBlock.transform.rotation;
				if (!IsPositionBlocked(futurePos, futureRot)) {
					SmoothRotate(activeBlock.transform.rotation, futureRot);
				}
			}
			else if (Input.GetKeyDown(KeyCode.E))
			{
				Vector3 futurePos = activeBlock.transform.position;
				Quaternion futureRot = Quaternion.Euler(0, 0, -90) * activeBlock.transform.rotation;
				if (!IsPositionBlocked(futurePos, futureRot)) {
					SmoothRotate(activeBlock.transform.rotation, futureRot);
				}
			}
            else if (Input.GetKeyDown(KeyCode.A))
            {
				Vector3 futurePos = activeBlock.transform.position;
				Quaternion futureRot = Quaternion.Euler(0, 90, 0) * activeBlock.transform.rotation;
				if (!IsPositionBlocked(futurePos, futureRot)) {
					SmoothRotate(activeBlock.transform.rotation, futureRot);
				}
			}
            else if (Input.GetKeyDown(KeyCode.S))
            {
				Vector3 futurePos = activeBlock.transform.position;
				Quaternion futureRot = Quaternion.Euler(-90, 0, 0) * activeBlock.transform.rotation;
				if (!IsPositionBlocked(futurePos, futureRot)) {
					SmoothRotate(activeBlock.transform.rotation, futureRot);
				}
			}
            else if (Input.GetKeyDown(KeyCode.D))
            {
				Vector3 futurePos = activeBlock.transform.position;
				Quaternion futureRot = Quaternion.Euler(0, -90, 0) * activeBlock.transform.rotation;
				if (!IsPositionBlocked(futurePos, futureRot)) {
					SmoothRotate(activeBlock.transform.rotation, futureRot);
				}
			}
            */


            //fallSpeed = defaultFallSpeed;

            /*
			else if (Input.GetKey(KeyCode.Space)) 
			{
                Debug.Log("space");

                
                //fallSpeed = 20f;
            }
            */
			//Debug.Log (fallSpeed);
        }
        else
        {
            if (isMoving)
            {
                SmoothMove();
            }
            else if (isRotating)
            {
                SmoothRotate();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("This-space-");
            cnt++;
            /*
            Debug.Log("----------------------------------------------------------------------------");
            
            Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, 1);
            Quaternion futureRot = activeBlock.transform.rotation;
            Debug.Log("###futurePos++" + futurePos + "###futureRot++" + futureRot);
            if (IsPositionBlocked(futurePos, futureRot))
            {
                Debug.Log("This-isFalling-");
                activeBlock.transform.position = new Vector3(
                    (int)Mathf.Round(activeBlock.transform.position.x),
                    (int)Mathf.Round(activeBlock.transform.position.y),
                    (int)Mathf.Round(activeBlock.transform.position.z));
                Debug.Log("isFalling in -:" + (int)Mathf.Round(activeBlock.transform.position.x) +
                    (int)Mathf.Round(activeBlock.transform.position.y) +
                    (int)Mathf.Round(activeBlock.transform.position.z));
                SetPositionBlocked();

                activeBlock = (GameObject)GameObject.Instantiate(blockFactory.GetNextBlock(), new Vector3(0, -2, 1), Quaternion.identity);
                zNextCheckPoint = 1;
            }
            */
            
            activeBlock.transform.position = new Vector3(
                       (int)Mathf.Round(activeBlock.transform.position.x),
                       (int)Mathf.Round(activeBlock.transform.position.y),
                       (int)Mathf.Round(activeBlock.transform.position.z));
            

            SetPositionBlocked();
            dataSend = cnt + "," + ((int)Mathf.Round(activeBlock.transform.position.x) + 1) + "," +
                    ((int)Mathf.Round(activeBlock.transform.position.y) + 2) + "," +
                    ((int)Mathf.Round(activeBlock.transform.position.z) - 1) + "," +
                    blockFactory.CurrentBox();
            boxes.Add(dataSend);
            Debug.Log("이 박스의 정보는 " + dataSend);
            activeBlock = (GameObject)GameObject.Instantiate(blockFactory.GetNextBlock(), new Vector3(-1, 2, 1), Quaternion.identity);
            
        }

    }

    

    bool[,,] blocked = new bool[7,7,12];
	private void initGrid() {
		for (int i = 0; i < 7; i++) {
			for (int j = 0; j < 7; j++) {
				for (int k = 0; k < 12; k++) {
                    
					blocked[i,j,k] = false;
                    
					if (i == 0 || j == 0 || k == 0 || i == 6 || j == 6 || k == 11)
					{
						blocked[i,j,k] = true;
                    }
                    
				}
			}
		}
    }
    

	private bool IsPositionBlocked(Vector3 futurePos, Quaternion futureRot) {
		foreach (Transform cube in activeBlock.transform.GetComponentsInChildren<Transform>())
		{
			if (cube.childCount == 0)
			{
                
                //Debug.Log ("Cube Pos: " + cube.position);
                //Debug.Log ("Future Pos: " + futurePos);
                try
                {
                    if (blocked[
                            (int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) + 3,
                            (int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) + 3,
                            (int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z))])
                    {

                        return true;
                    }
                }
                catch
                {
                    SceneManager.LoadScene("Main");
                }
               
                
                        
                
			}
		}
		return false;

		/*if (blocked[
			(int)Mathf.Round (futurePos.x) + 3, (int)Mathf.Round (futurePos.y) + 3, (int)Mathf.Round (futurePos.z)])
			return true;
		return false;*/
	}
    
    private void SetPositionBlocked() // uses activeBlock
	{
        
        foreach (Transform cube in activeBlock.transform.GetComponentsInChildren<Transform>())
		{
            try
            {
                if (cube.childCount == 0)
                {
                    int i = (int)Mathf.Round(cube.transform.position.x) + 3;
                    int j = (int)Mathf.Round(cube.transform.position.y) + 3;
                    int k = (int)Mathf.Round(cube.transform.position.z);
                    blocked[i, j, k] = true;

                }
            }
            catch
            {
                SceneManager.LoadScene("Main");
            }
        }
        
    }
    

    private void SmoothMove(Vector3 start, Vector3 end)
    {
        isMoving = true;
        blockStartPos = start;
        blockEndPos = end;
        tElapsed = 0;
        SmoothMove();
    }

    private void SmoothMove()
    {
        tElapsed += Time.deltaTime * moveSpeed;
        activeBlock.transform.position = Vector3.Lerp(blockStartPos, blockEndPos, tElapsed);
        if (tElapsed >= 1f) isMoving = false;
    }

    private void SmoothRotate(Quaternion start, Quaternion end)
    {
        isRotating = true;
        blockStartRot = start;
        blockEndRot = end;
        tElapsed = 0;
        SmoothRotate();
    }

    private void SmoothRotate()
    {
        tElapsed += Time.deltaTime * moveSpeed;
        activeBlock.transform.rotation = Quaternion.Lerp(blockStartRot, blockEndRot, tElapsed);
        if (tElapsed >= 1f) isRotating = false;
    }
    
}
