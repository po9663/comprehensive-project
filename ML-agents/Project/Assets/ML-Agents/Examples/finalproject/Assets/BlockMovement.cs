/*




*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using MLAgents;

public class BlockMovement : Agent {
    int deadcount = 0;
    public GameObject Boundaries;
	public GameObject Block;
    public GameObject Block2;
    public GameObject Block3;
    public GameObject Block4;
    public GameObject Block5;
    public GameObject Block6;
    public GameObject Block7;
	public Material MATRED;
	public Material MATBLUE;
	public Material MATGREEN;
    bool isBlocked = false;
    private bool bonus = false;
    private bool isDead = false;

    Rigidbody rigidbody;
    Dictionary<string, string> dictionary;
    public string boxsize = "";
    public string dicList = "";
    ArrayList vlist;
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
    bool[,,] blocked = new bool[7, 7, 12]; //블록체크
    // Use this for initialization
    public BlockMovement()
    {

    }

    public override void InitializeAgent()
    {
        // BlockFactory 클래스 객체생성
        dictionary = new Dictionary<string, string>()
        {
            {"Block ","1,1,1"},
            {"Block2","2,1,1"},
            {"Block3","1,2,1"},
            {"Block4","1,1,2"},
            {"Block5","2,1,2"},
            {"Block6","2,2,1"},
            {"Block7","2,2,2"}
        };
        blockFactory = new BlockFactory(Block, Block2, Block3, Block4, Block5, Block6, Block7, MATRED, MATBLUE, MATGREEN);
        
        //grid생성
        initGrid();
        // get the active block
        //activeBlock = GameObject.FindGameObjectWithTag("Player");
        //상자 생성 위치는 0,0,1위치
        CreateBox();
        foreach (MeshRenderer mr in activeBlock.GetComponentsInChildren<MeshRenderer>())
        {
            //Debug.Log("Start here");
            mr.material = MATBLUE;
        }
        boxes = new ArrayList();
       
    }
   
    public override void AgentReset()
    {
        isDead = false;
        dicList = "";
        boxsize = "";
        Debug.Log("주금");
        //InitializeAgent();
    }

    void BlockCheck()
    {
        int count = 0;
        
        //currentBox = blockFactory.CurrentBox();
        if (dictionary.TryGetValue(blockFactory.CurrentBox(), out string description))
        {
            boxsize = description;
        }
        //Debug.Log("size=" + boxsize);
        //x,y,z 좌표 순서
        bool Isy = false;
        bool Isz = false;
        string [] bsize = boxsize.Split(',');
        //Debug.Log("bs0="+bsize[0]);
        //Debug.Log("bs1=" + bsize[1]);
        //Debug.Log("bs2=" + bsize[2]);
        int xsize = Int32.Parse(bsize[0]);
        int ysize = Int32.Parse(bsize[1]);
        int zsize = Int32.Parse(bsize[2]);
        vlist= new ArrayList();
        if (xsize == 0 && ysize !=0)
        {
            Isy = true;
        }
        if (ysize == 0 && zsize != 0)
        {
            Isz = true;
        }
        for (int i=0; i<5; i++)
        {
            //좌표수 만큼 반복 7x7x12
            for(int j=0; j<5; j++)
            {
                for(int k=0; k<10; k++)
                {
                    if (blocked[i,j,k] == true)
                    {
                        //들어있음

                    }
                    else
                    {
                        //비어있음
                        for(int a=0; a<xsize||Isy==true||Isz==true; a++)
                        {
                            //x
                            for(int b=0; b<ysize||Isz==true; b++)
                            {
                                //y
                                for(int c=0; c<zsize; c++)
                                {
                                    //z
                                    if (blocked[i + a,j + b,k + c]== true)
                                    {
                                        //들어있음
                                    }
                                    else
                                    {
                                        if (j != 0)
                                        {
                                            if (b == 1)
                                            {
                                                    count++;
                                            }
                                            else
                                            {
                                                if (blocked[i + a, j + b - 1, k + c] == true)
                                                {
                                                    count++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            count++;
                                            //비어있음
                                        }

                                    }
                                   
                                }
                                Isz = false;
                            }
                            Isy = false;
                        }
                        if (count!=0 && count == xsize * ysize * zsize)
                        {
                            if (j != 0)
                            {
                                if (blocked[i, j - 1, k] == true)
                                {
                                    //들어갈 수 있다.
                                    string data = i + "," + j + "," + k;
                                    vlist.Add(data);
                                    //Debug.Log(data + "= 최종적으로 들어갈 수 있다");
                                }
                            }
                            else
                            {
                                //들어갈 수 있다.
                                string data = i + "," + j + "," + k;
                                vlist.Add(data);
                                //Debug.Log(data + "= 최종적으로 들어갈 수 있다");
                            }
                        }
                        else
                        {
                            //들어갈 수 없다.
                            //Debug.Log("최종적으로 들어갈 수 없다");
                        }
                        count = 0;
                    }
                }
            }
        }
        int num = 250 - vlist.Count;
        if (num == 250)
        {
            Debug.Log("게임오버");
            isDead = true;
        }
        else
        {
            for (int a = 0; a < num; a++)
            {
                vlist.Add("0,0,0");
            }
            //Debug.Log("Vlist 크기 : " + vlist.Count);
        }
    }
    public override void CollectObservations()
    {
        //currentBox=blockFactory.CurrentBox();
        BlockCheck();

        Vector3 relativePosition = activeBlock.transform.position;

        // 정규화된 값
        AddVectorObs(Mathf.Clamp(relativePosition.x / 7f, -1f, 1f));
        AddVectorObs(Mathf.Clamp(relativePosition.y / 7f, -1f, 1f));
        AddVectorObs(Mathf.Clamp(relativePosition.z / 12f, -1f, 1f));
        //AddVectorObs(relativePosition.x);
        //AddVectorObs(relativePosition.y);
        //AddVectorObs(relativePosition.z);
        //AddVectorObs(relativePosition);

        for (int i=0; i<vlist.Count; i++)
        {
            string temp = vlist[i].ToString();
            string[] bp = temp.Split(',');
            int xp = Int32.Parse(bp[0]);
            int yp = Int32.Parse(bp[1]);
            int zp = Int32.Parse(bp[2]);
            Vector3 blockTransform = new Vector3(xp, yp, zp);
            Vector3 distanceToblock = blockTransform - relativePosition;
            //Debug.Log(distanceToblock);
            AddVectorObs(Mathf.Clamp(distanceToblock.x / 7f, -1f, 1f));
            AddVectorObs(Mathf.Clamp(distanceToblock.y / 7f, -1f, 1f));
            AddVectorObs(Mathf.Clamp(distanceToblock.z / 12f, -1f, 1f));
            //AddVectorObs(distanceToblock);

        }
        


        //Vector3 distanceToblock = activeBlock.transform.position;
    }
    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.D))
        {
            return new float[] { 2 };
        }
        if (Input.GetKey(KeyCode.W))
        {
            return new float[] { 4 };
        }
        if (Input.GetKey(KeyCode.A))
        {
            return new float[] { 1 };
        }
        if (Input.GetKey(KeyCode.S))
        {
            return new float[] { 3 };
        }
        if (Input.GetKey(KeyCode.E))
        {
            return new float[] { 5 };
        }
        if (Input.GetKey(KeyCode.Q))
        {
            return new float[] { 6 };
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            return new float[] { 7 };
        }
        
        return new float[] { 0 };
        
        
    }
    public override void AgentAction(float[] vectorAction)
    {
        //AddReward(-0.01f); //가만히 있는것 방지
        
        var movement = Mathf.FloorToInt(vectorAction[0]);
        
        if (movement == 1) {
            AddReward(0.01f);
            if (!isMoving && !isRotating)
            {
                //directionX = -1; // 왼쪽 방향키
                Vector3 futurePos = activeBlock.transform.position + new Vector3(-1f, 0, 0);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
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

        }
        else if (movement == 2) {
            AddReward(0.01f);
            if (!isMoving && !isRotating)
            {
                //directionX = 1; // 오른쪽 방향키 x축
                Vector3 futurePos = activeBlock.transform.position + new Vector3(1f, 0, 0);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
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
        }
        else if (movement == 3) {
            AddReward(0.01f);
            if (!isMoving && !isRotating)
            {
                //directionZ = -1; // 아래 방향키 z축
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, -1f);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
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
        }
        else if (movement == 4) {
            AddReward(0.01f);
            if (!isMoving && !isRotating)
            {
                //directionZ = 1; // 위 방향키 z축
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, 1f);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
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
        }
        else if (movement == 5) {
            AddReward(0.01f);
            if (!isMoving && !isRotating)
            {
                //directionY = -1; // 아래 방향키 y축
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, -1f, 0);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
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
        }
        else if (movement == 6) {
            AddReward(0.01f);
            if (!isMoving && !isRotating)
            {
                //directionY = 1; // 위 방향키 y축
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 1f, 0);
                Quaternion futureRot = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos, futureRot))
                {
                    SmoothMove(activeBlock.transform.position, futurePos);
                }
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
        }
        else if (movement == 7)
        {
            AddReward(-0.01f);
            Vector3 futurePos = activeBlock.transform.position;
            Quaternion futureRot = activeBlock.transform.rotation;
            if (!IsSetPositionBlocked(futurePos, futureRot))
            {
                AddReward(0.1f);
                Debug.Log("This-space-");
                cnt++;
                rigidbody = activeBlock.transform.GetComponent<Rigidbody>();
                rigidbody.constraints = RigidbodyConstraints.None;


                rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
                activeBlock.transform.position = new Vector3(
                           (int)Mathf.Round(activeBlock.transform.position.x),
                           (int)Mathf.Round(activeBlock.transform.position.y),
                           (int)Mathf.Round(activeBlock.transform.position.z));

                if (dictionary.TryGetValue(blockFactory.CurrentBox(), out string description))
                {
                    dicList = description;
                }
                //Invoke("FreezeBlock", 1);
                //Invoke("SetPositionBlocked", 1);

                dataSend = cnt + "," + ((int)Mathf.Round(activeBlock.transform.position.x)) + "," +
                        ((int)Mathf.Round(activeBlock.transform.position.y)) + "," +
                        ((int)Mathf.Round(activeBlock.transform.position.z)) + "," +
                        blockFactory.CurrentBox() + "," + dicList;

                boxes.Add(dataSend);
                Debug.Log("이 박스의 정보는 " + dataSend);
                FreezeBlock();
                SetPositionBlocked();
                deadcount++;
            }
        }
        
        

        if (isDead||deadcount==10)
        {
            AddReward(-1.0f);
            Debug.Log("죽음");
            AgentReset();
        }
        

    }
    


    private void CreateBox()
    {
        activeBlock = (GameObject)GameObject.Instantiate(blockFactory.GetNextBlock(), new Vector3(0, 0, 0), Quaternion.identity);
        activeBlock.transform.parent = Boundaries.transform;
        
    }

   

    
    private void initGrid() {
		for (int i = 0; i < 7; i++) {
			for (int j = 0; j < 7; j++) {
				for (int k = 0; k < 12; k++) {
                    
					blocked[i,j,k] = false;
                    
					if ((i >= 5) || (j >= 5) || (k >= 10))
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
                    if (((int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) < 0) || ((int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z)) < 0) || ((int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) < 0)||((int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) < 0) || ((int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z)) < 0) || ((int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) < 0)
                        || ((int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) > 4) || ((int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z)) > 9) || ((int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) > 4))
                    {
                        return true;
                    }
                    /*else if (blocked[
                            (int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)),
                            (int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)),
                            (int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z))])
                    {

                        return true;
                    }*/

                }
                catch
                {
                    //SceneManager.LoadScene("Main");
                }




            }
		}
		return false;

		/*if (blocked[
			(int)Mathf.Round (futurePos.x) + 3, (int)Mathf.Round (futurePos.y) + 3, (int)Mathf.Round (futurePos.z)])
			return true;
		return false;*/
	}
    private bool IsSetPositionBlocked(Vector3 futurePos, Quaternion futureRot)
    {
        
        foreach (Transform cube in activeBlock.transform.GetComponentsInChildren<Transform>())
        {
            if (cube.childCount == 0)
            {
                //Debug.Log(cube.gameObject.name);
                //Debug.Log ("Cube Pos: " + cube.position);
                //Debug.Log ("Future Pos: " + futurePos);
                try
                {
                    
                    if (blocked[
                            (int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)),
                            (int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)),
                            (int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z))])
                    {
                        
                        return true;
                    }
                    else if(!(((int)Mathf.Round((cube.position.y - activeBlock.transform.position.y)))>1) && !blocked[
                            (int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)),
                            (int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y))-1,
                            (int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z))])
                    {
                        if (cube.gameObject.name.Equals("Cube1"))
                        {
                            
                        }
                        else
                        {
                            return true;
                        }
                    }

                }
                catch
                {
                    //SceneManager.LoadScene("Main");
                }




            }
        }
        return false;
        
    }


    private void SetPositionBlocked() // uses activeBlock
	{
        

        foreach (Transform cube in activeBlock.transform.GetComponentsInChildren<Transform>())
		{
            try
            {
                if (cube.childCount == 0)
                {
                    int i = (int)Mathf.Round(cube.transform.position.x);
                    int j = (int)Mathf.Round(cube.transform.position.y);
                    int k = (int)Mathf.Round(cube.transform.position.z);
                    blocked[i, j, k] = true;

                }
            }
            catch
            {
                //SceneManager.LoadScene("Main");
            }
        }
        CreateBox();
    }
    private void FreezeBlock()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
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
