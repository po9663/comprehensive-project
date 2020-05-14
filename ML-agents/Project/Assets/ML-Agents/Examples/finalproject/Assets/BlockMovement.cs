/*




*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using MLAgents;

public class BlockMovement : Agent {

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
    public string dicList = "";
    public string boxsize = "";
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
            {"Block","1,1,1"},
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
            Debug.Log("Start here");
            mr.material = MATBLUE;
        }
        boxes = new ArrayList();
    }
    public override void AgentReset()
    {
        isDead = false;
        //InitializeAgent();
    }
    void BlockCheck()
    {
        int count = 0;
        currentBox = blockFactory.CurrentBox();
        if (dictionary.TryGetValue(blockFactory.CurrentBox(), out string description))
        {
            boxsize = description;
        }
        //x,y,z 좌표 순서
        bool Isy = false;
        bool Isz = false;
        string [] bsize = boxsize.Split(',');
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
                                        count++;
                                        //비어있음
                                    }
                                   
                                }
                                Isz = false;
                            }
                            Isy = false;
                        }
                        if (count!=0 && count == xsize * ysize * zsize)
                        {
                            //들어갈 수 있다.
                            string data = i + "," + j + "," + k;
                            vlist.Add(data);
                            Debug.Log(data + "= 최종적으로 들어갈 수 있다");
                        }
                        else if (count ==0)
                        {
                            Debug.Log("게임오버");
                            isDead = true;
                        }
                        else
                        {
                            //들어갈 수 없다.
                            Debug.Log("최종적으로 들어갈 수 없다");
                        }
                        count = 0;
                    }
                }
            }
        }
        
    }
    public override void CollectObservations()
    {
        currentBox=blockFactory.CurrentBox();
        BlockCheck();

        Vector3 relativePosition = activeBlock.transform.position;

        // 정규화된 값
        //AddVectorObs(Mathf.Clamp(relativePosition.x / 7f, -1f, 1f));
        //AddVectorObs(Mathf.Clamp(relativePosition.y / 7f, -1f, 1f));
        //AddVectorObs(Mathf.Clamp(relativePosition.z / 12f, -1f, 1f));
        AddVectorObs(relativePosition.x);
        AddVectorObs(relativePosition.y);
        AddVectorObs(relativePosition.z);
        
        for (int i=0; i<vlist.Count; i++)
        {
            string temp = vlist[i].ToString();
            string[] bp = temp.Split(',');
            int xp = Int32.Parse(bp[0]);
            int yp = Int32.Parse(bp[1]);
            int zp = Int32.Parse(bp[2]);
            Vector3 distanceToblock = new Vector3(xp, yp, zp)-activeBlock.transform.position;
            Debug.Log(distanceToblock);
            AddVectorObs(Mathf.Clamp(distanceToblock.x / 7f, -1f, 1f));
            AddVectorObs(Mathf.Clamp(distanceToblock.y / 7f, -1f, 1f));
            AddVectorObs(Mathf.Clamp(distanceToblock.z / 12f, -1f, 1f));


        }
        


        //Vector3 distanceToblock = activeBlock.transform.position;
    }
    public override void AgentAction(float[] vectorAction)
    {
        AddReward(-0.01f); //가만히 있는것 방지
        
        int movement = Mathf.FloorToInt(vectorAction[0]);
       
        int block = Mathf.FloorToInt(vectorAction[1]);
        
        if (movement == 1) {
            if (!isMoving && !isRotating)
            {
                //directionX = -1; // 왼쪽 방향키
                Vector3 futurePos = activeBlock.transform.position + new Vector3(-1, 0, 0);
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
        if (movement == 2) {
            if (!isMoving && !isRotating)
            {
                //directionX = 1; // 오른쪽 방향키 x축
                Vector3 futurePos = activeBlock.transform.position + new Vector3(1, 0, 0);
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
        if (movement == 3) {
            if (!isMoving && !isRotating)
            {
                //directionZ = -1; // 아래 방향키 z축
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, -1);
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
        if (movement == 4) {
            if (!isMoving && !isRotating)
            {
                //directionZ = 1; // 위 방향키 z축
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, 1);
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
        if (movement == 5) {
            if (!isMoving && !isRotating)
            {
                //directionY = -1; // 아래 방향키 y축
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, -1, 0);
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
        if (movement == 6) {
            if (!isMoving && !isRotating)
            {
                //directionY = 1; // 위 방향키 y축
                Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 1, 0);
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
        
        if(block == 1) {
            isBlocked = true;
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
            Invoke("FreezeBlock", 2);
            Invoke("SetPositionBlocked", 3);

            dataSend = cnt + "," + ((int)Mathf.Round(activeBlock.transform.position.x)) + "," +
                    ((int)Mathf.Round(activeBlock.transform.position.y)) + "," +
                    ((int)Mathf.Round(activeBlock.transform.position.z)) + "," +
                    blockFactory.CurrentBox() + "," + dicList;

            boxes.Add(dataSend);
            Debug.Log("이 박스의 정보는 " + dataSend);
        }



        if (isDead)
        {
            AddReward(-1.0f);
            Done();
        }
        if (isBlocked)
        {
            AddReward(1.0f);
            isBlocked = false;
        }

    }
    /*
    void blocked()
    {
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
        Invoke("FreezeBlock", 2);
        Invoke("SetPositionBlocked", 3);

        dataSend = cnt + "," + ((int)Mathf.Round(activeBlock.transform.position.x) + 1) + "," +
                ((int)Mathf.Round(activeBlock.transform.position.y) + 2) + "," +
                ((int)Mathf.Round(activeBlock.transform.position.z) - 1) + "," +
                blockFactory.CurrentBox() + "," + dicList;

        boxes.Add(dataSend);
        Debug.Log("이 박스의 정보는 " + dataSend);
        isBlocked = false;
    }*/
    /*void Start () {
        // BlockFactory 클래스 객체생성
        dictionary = new Dictionary<string, string>()
        {
            {"Block","1,1,1"},
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
			Debug.Log("Start here");
			mr.material = MATBLUE;
		}
        boxes = new ArrayList();
        
    }*/



            // Update is called once per frame
            /*
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



                    //fallSpeed = defaultFallSpeed;


                    else if (Input.GetKey(KeyCode.Space)) 
                    {
                        Debug.Log("space");


                        //fallSpeed = 20f;
                    }

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
                    Invoke("FreezeBlock", 2);
                    Invoke("SetPositionBlocked", 3);

                    dataSend = cnt + "," + ((int)Mathf.Round(activeBlock.transform.position.x) + 1) + "," +
                            ((int)Mathf.Round(activeBlock.transform.position.y) + 2) + "," +
                            ((int)Mathf.Round(activeBlock.transform.position.z) - 1) + "," +
                            blockFactory.CurrentBox() + "," + dicList;

                    boxes.Add(dataSend);
                    Debug.Log("이 박스의 정보는 " + dataSend);


                }

            }*/


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
                    if (((int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) < 0) || ((int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z)) < 0) || ((int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) < 0))
                    {
                        return true;
                    }
                    else if (blocked[
                            (int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)),
                            (int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)),
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
                    int i = (int)Mathf.Round(cube.transform.position.x);
                    int j = (int)Mathf.Round(cube.transform.position.y);
                    int k = (int)Mathf.Round(cube.transform.position.z);
                    blocked[i, j, k] = true;

                }
            }
            catch
            {
                SceneManager.LoadScene("Main");
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
