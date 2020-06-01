/*




*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    
	public Material MATRED;
	public Material MATBLUE;
	public Material MATGREEN;

    Rigidbody rigidbody;
    Dictionary<string, string> dictionary;
    public string dicList = "";
    public string boxsize = "";
    List<string> vlist;
    List<string> boxes;
    List<string> li;
    List<string> li1;
    List<string> li2;
    List<string> li3;
    bool isli1 = false;
    bool isli2 = false;
    bool isli3 = false;
    BlockFactory blockFactory;
	GameObject activeBlock;
	float moveSpeed = 10f;
    //float defaultFallSpeed = 1f;
    //float fallSpeed = 1f;
    bool isBeat = false;
    bool isChk = true;
    bool isMinus = false;
    bool ischchk = false;
    string so = "";
    bool isReCount = true;
    int cnt = 0;
	//float zNextCheckPoint = 1f;
    //bool isFalling = true;
    bool isMoving = false;
    bool isRotating = false;
    bool isDead = false;
    Vector3 blockStartPos, blockEndPos;
    Quaternion blockStartRot, blockEndRot;
    float tElapsed;
    public string currentBox = ""; // 현재박스의 이름
    public string dataSend = ""; //데이터 전송
    bool[,,] blocked = new bool[7, 7, 12];
    private float timeLeft = 0.5f;
    private float nextTime = 0.0f;
    int maxXXX = 0;
    int maxYYY = 0;
    int maxZZZ = 0;
    int positionZ = 9;
    int minus = 0;
    int maxX = 0;
    int maxY = 0;
    int maxZ = 9;
    int chX = 0;
    int chY = 0;
    int chY2L = 0;
    int chY3L = 0;
    int chZ = 0;
    int pullX = 0;
    int pullY = 0;
    int pullZ = 0;
    float movingZ;
    int reY = 0;
    int line1 = 0;
    int line2 = 0;
    int line3 = 0;
    int block = 0;
    int block2 = 0;
    int block3 = 0;
    int block4 = 0;
    int block5 = 0;
    int block6 = 0;
    int block7 = 0;
    // Use this for initialization
    public BlockMovement()
    {

    }
	void Start () {
        // BlockFactory 클래스 객체생성
        boxes = new List<string>();
        li = new List<string>();
        li1 = new List<string>();
        li2 = new List<string>();
        li3 = new List<string>();
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
			Debug.Log("Start here");
			mr.material = MATBLUE;
		}
        
        BlockCheck();
        
    }
    
    private void UpKey()
    {
        
        Vector3 futurePos = activeBlock.transform.position + new Vector3(chX, chY, 0);
        Quaternion futureRot = activeBlock.transform.rotation;
        if (!IsPositionBlocked(futurePos, futureRot))
        {
            SmoothMove(activeBlock.transform.position, futurePos);
        }
    }
    private void UpKey2L()
    {

        Vector3 futurePos = activeBlock.transform.position + new Vector3(chX, chY2L, 0);
        Quaternion futureRot = activeBlock.transform.rotation;
        if (!IsPositionBlocked(futurePos, futureRot))
        {
            SmoothMove(activeBlock.transform.position, futurePos);
        }
    }
    private void UpKey3L()
    {

        Vector3 futurePos = activeBlock.transform.position + new Vector3(chX, chY3L, 0);
        Quaternion futureRot = activeBlock.transform.rotation;
        if (!IsPositionBlocked(futurePos, futureRot))
        {
            SmoothMove(activeBlock.transform.position, futurePos);
        }
    }
    private void DownKey()
    {
        Vector3 futurePos = activeBlock.transform.position + new Vector3(0, -1, 0);
        Quaternion futureRot = activeBlock.transform.rotation;
        if (!IsPositionBlocked(futurePos, futureRot))
        {
            SmoothMove(activeBlock.transform.position, futurePos);
        }
    }
    private void ForwardKey()
    {
        Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, 1);
        Quaternion futureRot = activeBlock.transform.rotation;
        if (!IsPositionBlocked(futurePos, futureRot))
        {
            SmoothMove(activeBlock.transform.position, futurePos);
            Debug.Log("if앞");
        }else
        {
            Vector3 futurePosf = activeBlock.transform.position;
            Quaternion futureRotf = activeBlock.transform.rotation;
            if (!IsSetPositionBlocked(futurePosf, futureRotf))
            {

                StopBox();
                Debug.Log("else앞");
            }
            
        }
        /*
        Debug.Log("첫번째");
        string boxcur = pullX.ToString() + "+";
        ischchk = true;
        if (ischchk)
        {
            Debug.Log("3번째");
            for (int i = 0; i < li.Count; i++)
            {
                string[] str = li[i].Split(',');
                Debug.Log(str[0] + "[0]번째");
                Debug.Log(str[1] + "[1]번째");
                if (str[1].Equals(boxcur))
                {
                    so = str[0];
                    CheckReY();
                }
            }
            ischchk = false;
            Debug.Log("두번째");

            if (activeBlock.transform.position.x < 3)
            {
                Vector3 futurePos1 = activeBlock.transform.position + new Vector3(1, 0, 0);
                Quaternion futureRot1 = activeBlock.transform.rotation;
                if (!IsPositionBlocked(futurePos1, futureRot1))
                {
                    SmoothMove(activeBlock.transform.position, futurePos1);
                }
            }
            else
            {
                LeftKey();
            }
        }
        */
    }
    private void BackKey()
    {
        chZ = -1 * chZ;
        Debug.Log("ChZ = " + chZ);
        Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, chZ);
        Quaternion futureRot = activeBlock.transform.rotation;
        if (!IsPositionBlocked(futurePos, futureRot))
        {
            SmoothMove(activeBlock.transform.position, futurePos);
        }
    }
    private void LeftKey()
    {
        Vector3 futurePos = activeBlock.transform.position + new Vector3(-1, 0, 0);
        Quaternion futureRot = activeBlock.transform.rotation;
        if (!IsPositionBlocked(futurePos, futureRot))
        {
            SmoothMove(activeBlock.transform.position, futurePos);
        }
    }
    private void RightKey()
    {
        Debug.Log("chX:" + chX);
        Vector3 futurePos = activeBlock.transform.position + new Vector3(chX, 0, 0);
        Quaternion futureRot = activeBlock.transform.rotation;
        if (!IsPositionBlocked(futurePos, futureRot))
        {
            SmoothMove(activeBlock.transform.position, futurePos);
        }
    }
    // Update is called once per frame
    void Update()
    {
        rigidbody = activeBlock.transform.GetComponent<Rigidbody>();
        
        if (!isMoving && !isRotating)
        {
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LeftKey();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RightKey();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ForwardKey();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                BackKey();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                ForwardKey();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                BackKey();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                LeftKey();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RightKey();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                UpKey();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                DownKey();
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
        if (isBeat)
        {
            if (Time.time > nextTime)
            {
                nextTime = Time.time + timeLeft;
                ForwardKey();
            }
        }

        
    
        
        




        /*
        if (chX >= 1)
        {
            if(blockFactory.CurrentBox().Equals("Block4") || blockFactory.CurrentBox().Equals("Block5") || blockFactory.CurrentBox().Equals("Block7"))
            {
                
                if (activeBlock.transform.position.z >= (maxZ - 1))
                {
                    Vector3 futurePos = activeBlock.transform.position;
                    Quaternion futureRot = activeBlock.transform.rotation;
                    if (!IsSetPositionBlocked(futurePos, futureRot))
                    {
                        StopBox();
                        Debug.Log("aaaa" + pullX + "," + chX);
                    }
                }
                
            }
            else
            {
                if (activeBlock.transform.position.z >= maxZ)
                {
                    Vector3 futurePos = activeBlock.transform.position;
                    Quaternion futureRot = activeBlock.transform.rotation;
                    if (!IsSetPositionBlocked(futurePos, futureRot))
                    {
                        StopBox();
                        Debug.Log("bbbb" + pullX + "," + chX);
                    }
                }
            }
            
        }
        if(chX == 0)
        {
            if(pullX >= 1)
            {
                if(isMinus)
                {
                    maxZ--;
                    Debug.Log(maxZ + "maxZ 감소값");
                    isMinus = false;
                }
            }


            if (blockFactory.CurrentBox().Equals("Block4") || blockFactory.CurrentBox().Equals("Block5") || blockFactory.CurrentBox().Equals("Block7"))
            {
                if (activeBlock.transform.position.z >= (maxZ - 1))
                {
                    Vector3 futurePos = activeBlock.transform.position;
                    Quaternion futureRot = activeBlock.transform.rotation;
                    if (!IsSetPositionBlocked(futurePos, futureRot))
                    {
                        StopBox();
                        Debug.Log("cccc" + pullX + "," + chX);
                    }
                }
            }
            else
            {
                if (activeBlock.transform.position.z >= maxZ)
                {
                    Vector3 futurePos = activeBlock.transform.position;
                    Quaternion futureRot = activeBlock.transform.rotation;
                    if (!IsSetPositionBlocked(futurePos, futureRot))
                    {
                        StopBox();
                        Debug.Log("dddd" + pullX + "," + chX);
                    }
                }
            }
        }
        */
        
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            Vector3 futurePos = activeBlock.transform.position;
            Quaternion futureRot = activeBlock.transform.rotation;
            if (!IsSetPositionBlocked(futurePos, futureRot))
            {
                StopBox();
            }
                /*
                isBeat = false;
                Debug.Log("This-space-");
                cnt++;
                rigidbody = activeBlock.transform.GetComponent<Rigidbody>();
                rigidbody.constraints = RigidbodyConstraints.None;

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
                */
            }
        
    }


    

    private void CheckReY() 
    {
        if (isReCount)
        {
            if (so.Equals("Block3") || so.Equals("Block6") || so.Equals("Block7"))
            {
                UpKey();
                UpKey();
                Debug.Log("성공");
            }
            else
            {
                Debug.Log("성공2");
                UpKey();
            }
            

            isReCount = false;
            
        }
        
        Debug.Log(isReCount + "isReCount");
    }

    int line3L = 0;
    int line2L = 0;
    int line3LL = 0;
    int line2LL = 0;
    int line1L = 0;
    int line1LL = 0;
    
    
    private void CreateBox()
    {
        //Debug.Log("CreateBox chX 값 = " + chX);
        activeBlock = (GameObject)GameObject.Instantiate(blockFactory.GetNextBlock(), new Vector3(0, 0, 0), Quaternion.identity);

        



        if (blockFactory.CurrentBox().Equals("Block7"))     
        {
            li3.Add("Block7");
            chX = 3;
            RightKey();
            if (line3L == 1)
            {
                chY3L = 2;
            }
            
            UpKey3L();
            line3 += 2;
            isli3 = true;
        }

        if (blockFactory.CurrentBox().Equals("Block6"))
        {
            li3.Add("Block6");
            chX = 3;
            RightKey();
            if (line3L == 1)
            {
                chY3L = 2;
            }
            
            UpKey3L();
            line3++;
            isli3 = true;
        }

        if(line3 >= 8)
        {
            line3L++;
            line3 = 0;
            //y축으로 2 이동
        }

        //isSetblock에서는 닿은것이 7일때는 2칸뒤로 이동 6일때는 1칸이동


        

        if (blockFactory.CurrentBox().Equals("Block5"))
        {
            li2.Add("Block5");
            chX = 1;
            RightKey();
            if (line2L == 1)
            {
                chY2L = 1;
            }
            else if (line2L == 2)
            {
                chY2L = 2;
            }
            else if (line2L == 3)
            {
                chY2L = 3;
            }
            else if (line2L == 4)
            {
                chY2L = 4;
            }
            else if (line2L == 5)
            {
                chY2L = 5;
            }
            UpKey2L();
            line2 += 2;
            isli2 = true;
        }

        if (blockFactory.CurrentBox().Equals("Block2"))
        {
            li2.Add("Block2");
            chX = 1;
            RightKey();
            if (line2L == 1)
            {
                chY2L = 1;
            }
            else if (line2L == 2)
            {
                chY2L = 2;
            }
            else if (line2L == 3)
            {
                chY2L = 3;
            }
            else if (line2L == 4)
            {
                chY2L = 4;
            }
            else if (line2L == 5)
            {
                chY2L = 5;
            }
            UpKey2L();
            line2++;
            isli2 = true;
        }

        if (line2 >= 8)
        {
            line2L++;
            
            line2 = 0;
            //y축으로 1 이동
        }

        //isSetblock에서는 닿은것이 5일때는 2칸뒤로 이동 2일때는 1칸이동


        /*
         * 123 => 12놓고 3을 위로 놓음
         * 132 => 1놓고 3앞에놓고 뒤로 2넣음
         * 213 => 21놓고 3을 위로 놓음
         * 231 => 2놓고 3위로놓고 1놓음
         * 312 => 
         * 321 => 
         */

        if (blockFactory.CurrentBox().Equals("Block "))        
        {
            li1.Add("Block ");
            line1++;
            isli1 = true;
        }

        if (blockFactory.CurrentBox().Equals("Block3"))        
        {
            li1.Add("Block3");
            line1++;
            isli1 = true;
        }

        if (blockFactory.CurrentBox().Equals("Block4"))        
        {
            li1.Add("Block4");
            line1++;
            isli1 = true;
        }

        if(line1 >= 8)
        {

        }
            
        /*
        if (blockFactory.CurrentBox().Equals("Block2") || blockFactory.CurrentBox().Equals("Block5") || blockFactory.CurrentBox().Equals("Block6") || blockFactory.CurrentBox().Equals("Block7"))
        {
            if (chX == 4)
            {
                Debug.Log(chX + "reset chX");
                pullX++;
                Debug.Log(pullX + "pullx");
                isMinus = true;
                chX = 0;
            }
        }
        
        if(chX > 0)
        {
            RightKey();
        }
        */
        isBeat = true;
        isChk = true;
    }
    
    private void StopBox()
    {
        isReCount = true;
        isBeat = false;
        Debug.Log("This-space-");
        cnt++;
        rigidbody = activeBlock.transform.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.None;
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
        string more = blockFactory.CurrentBox() + "," + pullX + "+";
        Debug.Log(more + "more");
        li.Add(more);

        rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        activeBlock.transform.position = new Vector3(
                       (int)Mathf.Round(activeBlock.transform.position.x),
                       (int)Mathf.Round(activeBlock.transform.position.y),
                       (int)Mathf.Round(activeBlock.transform.position.z));

        if (dictionary.TryGetValue(blockFactory.CurrentBox(), out string description))
        {
             dicList = description;
        }
        /*
        if (blockFactory.CurrentBox().Equals("Block ") || blockFactory.CurrentBox().Equals("Block3") || blockFactory.CurrentBox().Equals("Block4"))
        {
            if(chX <= 5)
            {
                chX++;
                Debug.Log(chX + "chX첫번째 값");
            }
            
        }
        else
        {
            if (chX <= 5)
            {
                chX += 2;
                Debug.Log(chX + "chX두번째 값");
                
            }
            
        }

        if (chX >= 5)
        {
            Debug.Log(chX + "reset chX");
            pullX++;
            Debug.Log(pullX + "pullx");
            isMinus = true;
            chX = 0;
        }
        */

        FreezeBlock();
        SetPositionBlocked();
        
        
        
        
        isChk = false;


    }
    
    private void initGrid() {
		for (int i = 0; i < 7; i++) {
			for (int j = 0; j < 7; j++) {
				for (int k = 0; k < 12; k++) {
                    
					blocked[i,j,k] = false;

                    if ((i >= 5) || (j >= 5) || (k >= 10))
                    {
                        blocked[i, j, k] = true;
                    }

                }
			}
		}
    }
    void BlockCheck()
    {
        Debug.Log("BlockCheck");
        int count = 0;
        currentBox = blockFactory.CurrentBox();
        if (dictionary.TryGetValue(blockFactory.CurrentBox(), out string description))
        {
            boxsize = description;
        }
        //x,y,z 좌표 순서
        bool Isy = false;
        bool Isz = false;
        string[] bsize = boxsize.Split(',');
        int xsize = Int32.Parse(bsize[0]);
        int ysize = Int32.Parse(bsize[1]);
        int zsize = Int32.Parse(bsize[2]);
        Debug.Log(xsize + "," + ysize + "," + zsize);
        vlist = new List<string>();
        if (xsize == 0 && ysize != 0)
        {
            Isy = true;
        }
        if (ysize == 0 && zsize != 0)
        {
            Isz = true;
        }
        for (int i = 0; i < 5; i++)
        {
            //좌표수 만큼 반복 7x7x12
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 10; k++)
                {
                    if (blocked[i, j, k] == true)
                    {
                        //들어있음
                    }
                    else
                    {
                        //비어있음
                        for (int a = 0; a < xsize || Isy == true || Isz == true; a++)
                        {
                            //x
                            for (int b = 0; b < ysize || Isz == true; b++)
                            {
                                //y
                                for (int c = 0; c < zsize; c++)
                                {
                                    //z
                                    if (blocked[i + a, j + b, k + c] == true)
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
                        if (count != 0 && count == xsize * ysize * zsize)
                        {

                            //들어갈 수 있다.
                            string data = i + "," + j + "," + k;
                            vlist.Add(data);
                            //Debug.Log(data + "= 최종적으로 들어갈 수 있다");
                            
                        }
                        else if (count == 0)
                        {
                            Debug.Log("게임오버");
                            isDead = true;
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
        //Debug.Log("카운트1" + vlist.Count);
        /*
        minus = (250 - vlist.Count);
        for (int a = 0; a < minus; a++)
        {
            vlist.Add("0,0,0");
        }
        */
        //Debug.Log("카운트2" + vlist.Count);
        
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
                            (int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)),
                            (int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)),
                            (int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z))])
                    {
                        
                        return true;
                    }
                    
                    /*
                    if (((int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) < 0) || ((int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z)) < 0) || ((int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) < 0) || ((int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) < 0) || ((int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z)) < 0) || ((int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) < 0)
                        || ((int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) > 4) || ((int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z)) > 9) || ((int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) > 4))
                    {
                        Debug.Log("3번째");
                        
                        return true;

                        
                    }
                    */
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



    private bool IsSetPositionBlocked(Vector3 futurePos, Quaternion futureRot)
    {
        
        foreach (Transform cube in activeBlock.transform.GetComponentsInChildren<Transform>())
        {
            if (cube.childCount == 0)
            {
                //Debug.Log ("Cube Pos: " + cube.position);
                //Debug.Log ("Future Pos: " + futurePos);
                try
                {
                    if (blocked[
                            (int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)),
                            (int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)),
                            (int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z))])
                    {
                        Debug.Log("1번째" + pullX);

                        if(isli1)
                        {
                            isBeat = false;
                            if (li1.Count > 1)
                            {
                                if (li1[li1.Count - 2].Equals("Block "))
                                {
                                    Debug.Log("Block 임");
                                    chZ = 1;
                                    BackKey();
                                }
                                else if (li1[li1.Count - 2].Equals("Block3"))
                                {
                                    Debug.Log("Block3임");
                                    chZ = 1;
                                    BackKey();
                                }
                                else if (li1[li1.Count - 2].Equals("Block4"))
                                {
                                    Debug.Log("Block4임");
                                    chZ = 1;
                                    BackKey();
                                }
                            }
                            isli1 = false;
                        }
                        else if (isli2)
                        {
                            isBeat = false;
                            if (li2.Count > 1)
                            {
                                Debug.Log("Count = " + li2.Count);
                                if (li2[li2.Count - 2].Equals("Block5"))
                                {
                                    //2
                                    chZ = 2;
                                    BackKey();
                                    Debug.Log("2백");
                                }
                                else if (li2[li2.Count - 2].Equals("Block2"))
                                {
                                    //1
                                    chZ = 1;
                                    BackKey();
                                    Debug.Log("1백");
                                }
                            }
                            
                            Debug.Log("끝");
                            isli2 = false;
                            Debug.Log("끝222");
                            //return false;
                        }
                        else if (isli3)
                        {
                            isBeat = false;
                            if (li3.Count > 1)
                            {
                                if (li3[li3.Count - 2].Equals("Block7"))
                                {
                                    //2
                                    chZ = 2;
                                    BackKey();
                                }
                                else if (li3[li3.Count - 2].Equals("Block6"))
                                {
                                    //1
                                    chZ = 1;
                                    BackKey();
                                }
                            }
                            isli3 = false;
                        }

                        /*
                        if(pullX > 0)
                        {
                            isBeat = false;
                            int lipullX = pullX - 1;
                            
                            string boxcur = lipullX.ToString() + "+";
                            Debug.Log(boxcur + "는 boxcur");
                            ischchk = true;
                            if (ischchk)
                            {
                                for (int i = 0; i < li.Count; i++)
                                {
                                    string[] str = li[i].Split(',');
                                    Debug.Log(str[0] + "[0]번째");
                                    Debug.Log(str[1] + "[1]번째");
                                    if (str[1].Equals(boxcur))
                                    {
                                        so = str[0];
                                        CheckReY();
                                    }
                                }
                                ischchk = false;
                                
                                if (activeBlock.transform.position.x <= 2)
                                {
                                    Vector3 futurePos1 = activeBlock.transform.position + new Vector3(1, 0, 0);
                                    Quaternion futureRot1 = activeBlock.transform.rotation;
                                    if (!IsPositionBlocked(futurePos1, futureRot1))
                                    {
                                        SmoothMove(activeBlock.transform.position, futurePos1);
                                    }
                                }
                                else
                                {
                                    LeftKey();
                                }
                                
                            }
                            
                        }*/

                        return true;
                        
                    }
                    else if (!(((int)Mathf.Round((cube.position.y - activeBlock.transform.position.y))) > 1) && !blocked[
                            (int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)),
                            (int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) - 1,
                            (int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z))])
                    {
                        if (cube.gameObject.name.Equals("Cube1"))
                        {

                        }
                        else
                        {

                            Debug.Log("2번째");
                            ForwardKey();
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
                SceneManager.LoadScene("Main");
            }
        }

        dataSend = cnt + "," + ((int)Mathf.Round(activeBlock.transform.position.x)) + "," +
                    ((int)Mathf.Round(activeBlock.transform.position.y)) + "," +
                    ((int)Mathf.Round(activeBlock.transform.position.z)) + "," +
                    blockFactory.CurrentBox() + "," + dicList;
        boxes.Add(dataSend);
        Debug.Log("이 박스의 정보는 " + dataSend);
        

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
        /*
        activeBlock.transform.position = new Vector3(
                    (int)Mathf.Round(activeBlock.transform.position.x),
                    (int)Mathf.Round(activeBlock.transform.position.y),
                    (int)Mathf.Round(activeBlock.transform.position.z));
        Debug.Log("isFalling in -:" + (int)Mathf.Round(activeBlock.transform.position.x) +
            (int)Mathf.Round(activeBlock.transform.position.y) +
            (int)Mathf.Round(activeBlock.transform.position.z));
            */
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
