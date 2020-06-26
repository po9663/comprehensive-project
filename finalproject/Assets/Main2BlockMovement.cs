/*




*/

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.ComponentModel;

using System.Linq;
using System.Text;

using System.IO.Ports;
public class Main2BlockMovement : MonoBehaviour
{
    
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
    GameObject[] gameObjects;
    
    public int[] num;
    public int boxNum = 0;
    int cnt = 0;
    
    public string s = "";
    public string dicList = "";

    public string boxsize = "";
    GameObject[] gobjs;

    List<string> vlist;
    List<string> boxes;
    Dictionary<string, string> dictionary;
    Main2BlockFactory main2BlockFactory;
    GameObject activeBlock;
    float moveSpeed = 10f;
    //float defaultFallSpeed = 1f;
    //float fallSpeed = 1f;
    //float zNextCheckPoint = 1f;
    //bool isFalling = true;
    bool isMoving = false;
    bool isRotating = false;
    bool isDead = false;
    Vector3 blockStartPos, blockEndPos;
    Quaternion blockStartRot, blockEndRot;
    float tElapsed;
    bool[,,] blocked = new bool[11, 11, 12];
    bool isCreate = false;

    public string currentBox = ""; // 현재박스의 이름
    public string dataSend = ""; //데이터 전송
    // Use this for initialization
    private SerialPort sp;
    public string message;
    int recieve = 0;
    void Start()
    {
        /*
        sp = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);
        sp.Open();
        */
        boxes = new List<string>();
        num = new int[8];
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
        s = Singleton.Instance.strList;
        Debug.Log("main2 " + s);
        gameObjects = new GameObject[7];
        gameObjects[0] = Block;
        gameObjects[1] = Block2;
        gameObjects[2] = Block3;
        gameObjects[3] = Block4;
        gameObjects[4] = Block5;
        gameObjects[5] = Block6;
        gameObjects[6] = Block7;
        gobjs = new GameObject[8];
        
        string[] lists = s.Split(',');
        for(int i = 0; i<lists.Length; i++)
        {
            num[i] = int.Parse(lists[i]);
        }
        for (int i = 0; i < lists.Length; i++)
        {
            gobjs[i] = gameObjects[num[i]];
        }
        main2BlockFactory = new Main2BlockFactory(gobjs[0], gobjs[1]/*, gobjs[2], gobjs[3], gobjs[4], gobjs[5], gobjs[6], gobjs[7]*//*, MATRED, MATBLUE, MATGREEN*/);
        initGrid();
        CreateBox(boxNum);
        /*
        foreach (MeshRenderer mr in activeBlock.GetComponentsInChildren<MeshRenderer>())
        {
            Debug.Log("Start here");
            mr.material = MATBLUE;
        }
        */
        //grid생성
        
        // get the active block
        //activeBlock = GameObject.FindGameObjectWithTag("Player");
        //상자 생성 위치는 0,0,1위치
        
    }
    bool isYCheck = false;
    bool isCr = true;
    private void SendArduino() // 좌표 아두이노로 전송
    {
        if (sp.IsOpen)
        {
            string rsStr = "";
            for (int i = 0; i < boxes.Count; i++)
            {
                rsStr += boxes[i] + "/";
            }
            sp.Write(rsStr);

        }
        else
        {
            sp.Close();
            Application.Quit();
        }
        

    }
    private void CarryBox()
    {
        for (int y = 0; y <= 9; y++)
        {
            for (int z = 10; z >= 0; z--)
            {
                for (int x = 4; x <= 4; x++)
                {
                    if (cnt != 0)
                    {
                        for (int a = 0; a < vlist.Count; a++)
                        {
                            string[] strs = vlist[a].Split(',');
                            if (Int32.Parse(strs[1]) >= 1)
                            {
                                isYCheck = true;

                            }
                        }
                    }
                    if (isYCheck)
                    {
                        for (int ay = 1; ay <= 3; ay++)
                        {
                            if (main2BlockFactory.CurrentBox().Equals("Block "))
                            {
                                if (!blocked[x, ay, z] && z != 0)
                                {
                                    if (blocked[x, ay - 1, z - 1] && blocked[x, ay - 1, z])
                                    {
                                        Debug.Log("ifisY에서 Block 에서의 좌표" + x + "," + ay + "," + z);
                                        Vector3 futurePos1 = activeBlock.transform.position + new Vector3(x, ay, z);
                                        Quaternion futureRot1 = activeBlock.transform.rotation;
                                        if (!IsPositionBlocked(futurePos1, futureRot1))
                                        {
                                            SmoothMove(activeBlock.transform.position, futurePos1);
                                            Debug.Log("원하는 곳으로 이동 되었습니다.");


                                        }
                                        Vector3 futurePosf11 = activeBlock.transform.position;
                                        Quaternion futureRotf11 = activeBlock.transform.rotation;
                                        if (!IsSetPositionBlocked(futurePosf11, futureRotf11))
                                        {
                                            Debug.Log("원하는곳 블락성공");
                                            Invoke("StopBox", 1f);
                                            //StopBox();


                                        }
                                        else
                                        {
                                            Invoke("StopBox", 1f);
                                        }
                                        goto EXIT;
                                    }
                                    else
                                    {
                                        goto chck;
                                    }




                                }
                            }
                            else if (main2BlockFactory.CurrentBox().Equals("Block2"))
                            {
                                if (!blocked[x, ay, z] && !blocked[x + 1, ay, z] && z != 0)
                                {
                                    if (blocked[x, ay - 1, z] && blocked[x + 1, ay - 1, z] && blocked[x, ay - 1, z - 1] && blocked[x + 1, ay - 1, z - 1])
                                    {
                                        Debug.Log("if에서 Block2에서의 좌표" + x + "," + y + "," + z);
                                        Vector3 futurePos2 = activeBlock.transform.position + new Vector3(x, ay, z);
                                        Quaternion futureRot2 = activeBlock.transform.rotation;
                                        if (!IsPositionBlocked(futurePos2, futureRot2))
                                        {
                                            SmoothMove(activeBlock.transform.position, futurePos2);
                                            Debug.Log("원하는 곳으로 이동 되었습니다.");

                                        }
                                        Vector3 futurePos22 = activeBlock.transform.position;
                                        Quaternion futureRot22 = activeBlock.transform.rotation;
                                        if (!IsSetPositionBlocked(futurePos22, futureRot22))
                                        {
                                            Debug.Log("원하는곳 블락성공");
                                            Invoke("StopBox", 1f);
                                            //StopBox();


                                        }
                                        else
                                        {
                                            Invoke("StopBox", 1f);
                                        }
                                        goto EXIT;
                                    }
                                    else
                                    {
                                        goto chck;
                                    }

                                }
                            }
                            else if (main2BlockFactory.CurrentBox().Equals("Block3"))
                            {
                                if (!blocked[x, ay, z] && !blocked[x, ay + 1, z] && z != 0)
                                {
                                    if (blocked[x, ay - 1, z] && blocked[x, ay - 1, z - 1])
                                    {
                                        Debug.Log("if에서 Block3에서의 좌표" + x + "," + y + "," + z);
                                        Vector3 futurePos3 = activeBlock.transform.position + new Vector3(x, ay, z);
                                        Quaternion futureRot3 = activeBlock.transform.rotation;
                                        if (!IsPositionBlocked(futurePos3, futureRot3))
                                        {
                                            SmoothMove(activeBlock.transform.position, futurePos3);
                                            Debug.Log("원하는 곳으로 이동 되었습니다.");

                                        }
                                        Vector3 futurePos33 = activeBlock.transform.position;
                                        Quaternion futureRot33 = activeBlock.transform.rotation;
                                        if (!IsSetPositionBlocked(futurePos33, futureRot33))
                                        {
                                            Debug.Log("원하는곳 블락성공");
                                            Invoke("StopBox", 1f);
                                            //StopBox();


                                        }
                                        else
                                        {
                                            Invoke("StopBox", 1f);
                                        }
                                        goto EXIT;
                                    }
                                    else
                                    {
                                        goto chck;
                                    }


                                }
                            }
                            else if (main2BlockFactory.CurrentBox().Equals("Block4"))
                            {
                                if (!blocked[x, ay, z] && !blocked[x, ay, z + 1] && z != 0)
                                {
                                    if (blocked[x, ay - 1, z] && blocked[x, ay - 1, z + 1] && blocked[x, ay - 1, z - 1])
                                    {
                                        Debug.Log("if에서 Block4에서의 좌표" + x + "," + ay + "," + z);
                                        Vector3 futurePos4 = activeBlock.transform.position + new Vector3(x, ay, z);
                                        Quaternion futureRot4 = activeBlock.transform.rotation;
                                        if (!IsPositionBlocked(futurePos4, futureRot4))
                                        {
                                            SmoothMove(activeBlock.transform.position, futurePos4);
                                            Debug.Log("원하는 곳으로 이동 되었습니다.");

                                        }
                                        Vector3 futurePos44 = activeBlock.transform.position;
                                        Quaternion futureRot44 = activeBlock.transform.rotation;
                                        if (!IsSetPositionBlocked(futurePos44, futureRot44))
                                        {
                                            Debug.Log("원하는곳 블락성공");
                                            Invoke("StopBox", 1f);
                                            //StopBox();


                                        }
                                        else
                                        {
                                            Invoke("StopBox", 1f);
                                        }
                                        goto EXIT;
                                    }
                                    else
                                    {
                                        goto chck;
                                    }

                                }
                            }
                            else if (main2BlockFactory.CurrentBox().Equals("Block5"))
                            {
                                if (!blocked[x, ay, z] && !blocked[x + 1, ay, z] && !blocked[x, ay, z + 1] && !blocked[x + 1, ay, z + 1] && z != 0)
                                {
                                    if (blocked[x, ay - 1, z] && blocked[x + 1, ay - 1, z] && blocked[x, ay - 1, z + 1] && blocked[x + 1, ay - 1, z + 1] && blocked[x, ay - 1, z - 1] && blocked[x + 1, ay - 1, z - 1])
                                    {
                                        Debug.Log("if에서 Block5에서의 좌표" + x + "," + y + "," + z);
                                        Vector3 futurePos5 = activeBlock.transform.position + new Vector3(x, ay, z);
                                        Quaternion futureRot5 = activeBlock.transform.rotation;
                                        if (!IsPositionBlocked(futurePos5, futureRot5))
                                        {
                                            SmoothMove(activeBlock.transform.position, futurePos5);
                                            Debug.Log("원하는 곳으로 이동 되었습니다.");
                                        }
                                        Vector3 futurePos55 = activeBlock.transform.position;
                                        Quaternion futureRot55 = activeBlock.transform.rotation;
                                        if (!IsSetPositionBlocked(futurePos55, futureRot55))
                                        {
                                            Debug.Log("원하는곳 블락성공");
                                            Invoke("StopBox", 1f);
                                            //StopBox();
                                        }
                                        else
                                        {
                                            Invoke("StopBox", 1f);
                                        }
                                        goto EXIT;
                                    }
                                    else
                                    {
                                        goto chck;
                                    }

                                }

                            }
                            else if (main2BlockFactory.CurrentBox().Equals("Block6"))
                            {
                                if (!blocked[x, ay, z] && !blocked[x + 1, ay, z] && !blocked[x, ay + 1, z] && !blocked[x + 1, ay + 1, z] && z != 0)
                                {
                                    if (blocked[x, ay - 1, z] && blocked[x + 1, ay - 1, z] && blocked[x, ay - 1, z - 1] && blocked[x + 1, ay - 1, z - 1])
                                    {
                                        Debug.Log("if에서 Block6에서의 좌표" + x + "," + ay + "," + z);
                                        Vector3 futurePos6 = activeBlock.transform.position + new Vector3(x, ay, z);
                                        Quaternion futureRot6 = activeBlock.transform.rotation;
                                        if (!IsPositionBlocked(futurePos6, futureRot6))
                                        {
                                            SmoothMove(activeBlock.transform.position, futurePos6);
                                            Debug.Log("원하는 곳으로 이동 되었습니다.");

                                        }
                                        Vector3 futurePos66 = activeBlock.transform.position;
                                        Quaternion futureRot66 = activeBlock.transform.rotation;
                                        if (!IsSetPositionBlocked(futurePos66, futureRot66))
                                        {
                                            Debug.Log("원하는곳 블락성공");
                                            Invoke("StopBox", 1f);
                                            //StopBox();


                                        }
                                        else
                                        {
                                            Invoke("StopBox", 1f);
                                        }
                                        goto EXIT;
                                    }
                                    else
                                    {
                                        goto chck;
                                    }

                                }
                            }
                            else if (main2BlockFactory.CurrentBox().Equals("Block7"))
                            {
                                if (!blocked[x, ay, z] && !blocked[x + 1, ay, z] && !blocked[x, ay + 1, z] && !blocked[x, ay, z + 1] && !blocked[x + 1, ay + 1, z] && !blocked[x + 1, ay, z + 1] && !blocked[x, ay + 1, z + 1] && !blocked[x + 1, ay + 1, z + 1] && z != 0)
                                {
                                    if (blocked[x, ay - 1, z] && blocked[x + 1, ay - 1, z] && blocked[x, ay - 1, z + 1] && blocked[x + 1, ay - 1, z + 1] && blocked[x, ay - 1, z - 1] && blocked[x + 1, ay - 1, z - 1])
                                    {
                                        Debug.Log("if에서 Block7에서의 좌표" + x + "," + ay + "," + z);
                                        Vector3 futurePos7 = activeBlock.transform.position + new Vector3(x, ay, z);
                                        Quaternion futureRot7 = activeBlock.transform.rotation;
                                        if (!IsPositionBlocked(futurePos7, futureRot7))
                                        {
                                            SmoothMove(activeBlock.transform.position, futurePos7);
                                            Debug.Log("원하는 곳으로 이동 되었습니다.");

                                        }
                                        Vector3 futurePos77 = activeBlock.transform.position;
                                        Quaternion futureRot77 = activeBlock.transform.rotation;
                                        if (!IsSetPositionBlocked(futurePos77, futureRot77))
                                        {
                                            Debug.Log("원하는곳 블락성공");
                                            Invoke("StopBox", 1f);
                                            //StopBox();


                                        }
                                        else
                                        {
                                            Invoke("StopBox", 1f);
                                        }
                                        goto EXIT;
                                    }
                                    else
                                    {
                                        goto chck;
                                    }

                                }
                            }

                            isYCheck = false;
                        }
                        isYCheck = false;
                    }

                chck: isCr = true;

                    if (isCr)
                    {
                        if (main2BlockFactory.CurrentBox().Equals("Block "))
                        {
                            if (!blocked[x, y, z])
                            {
                                Debug.Log("ifCr에서 Block 에서의 좌표" + x + "," + y + "," + z);
                                Vector3 futurePos111 = activeBlock.transform.position + new Vector3(x, y, z);
                                Quaternion futureRot111 = activeBlock.transform.rotation;
                                if (!IsPositionBlocked(futurePos111, futureRot111))
                                {
                                    SmoothMove(activeBlock.transform.position, futurePos111);
                                    Debug.Log("원하는 곳으로 이동 되었습니다.");


                                }
                                Vector3 futurePos1111 = activeBlock.transform.position;
                                Quaternion futureRot1111 = activeBlock.transform.rotation;
                                if (!IsSetPositionBlocked(futurePos1111, futureRot1111))
                                {
                                    Debug.Log("원하는곳 블락성공");
                                    Invoke("StopBox", 1f);
                                    //StopBox();


                                }
                                else
                                {
                                    Invoke("StopBox", 1f);
                                }
                                goto EXIT;

                            }
                        }
                        else if (main2BlockFactory.CurrentBox().Equals("Block2"))
                        {
                            if (!blocked[x, y, z] && !blocked[x + 1, y, z])
                            {
                                Debug.Log("if에서 Block2에서의 좌표" + x + "," + y + "," + z);
                                Vector3 futurePos222 = activeBlock.transform.position + new Vector3(x, y, z);
                                Quaternion futureRot222 = activeBlock.transform.rotation;
                                if (!IsPositionBlocked(futurePos222, futureRot222))
                                {
                                    SmoothMove(activeBlock.transform.position, futurePos222);
                                    Debug.Log("원하는 곳으로 이동 되었습니다.");

                                }
                                Vector3 futurePos2222 = activeBlock.transform.position;
                                Quaternion futureRot2222 = activeBlock.transform.rotation;
                                if (!IsSetPositionBlocked(futurePos2222, futureRot2222))
                                {
                                    Debug.Log("원하는곳 블락성공");
                                    Invoke("StopBox", 1f);
                                    //StopBox();


                                }
                                else
                                {
                                    Invoke("StopBox", 1f);
                                }
                                goto EXIT;
                            }
                        }
                        else if (main2BlockFactory.CurrentBox().Equals("Block3"))
                        {
                            if (!blocked[x, y, z] && !blocked[x, y + 1, z])
                            {
                                Debug.Log("if에서 Block3에서의 좌표" + x + "," + y + "," + z);
                                Vector3 futurePos333 = activeBlock.transform.position + new Vector3(x, y, z);
                                Quaternion futureRot333 = activeBlock.transform.rotation;
                                if (!IsPositionBlocked(futurePos333, futureRot333))
                                {
                                    SmoothMove(activeBlock.transform.position, futurePos333);
                                    Debug.Log("원하는 곳으로 이동 되었습니다.");

                                }
                                Vector3 futurePos3333 = activeBlock.transform.position;
                                Quaternion futureRot3333 = activeBlock.transform.rotation;
                                if (!IsSetPositionBlocked(futurePos3333, futureRot3333))
                                {
                                    Debug.Log("원하는곳 블락성공");
                                    Invoke("StopBox", 1f);
                                    //StopBox();


                                }
                                else
                                {
                                    Invoke("StopBox", 1f);
                                }
                                goto EXIT;


                            }
                        }
                        else if (main2BlockFactory.CurrentBox().Equals("Block4"))
                        {
                            if (!blocked[x, y, z] && !blocked[x, y, z + 1])
                            {
                                Debug.Log("if에서 Block4에서의 좌표" + x + "," + y + "," + z);
                                Vector3 futurePos444 = activeBlock.transform.position + new Vector3(x, y, z);
                                Quaternion futureRot444 = activeBlock.transform.rotation;
                                if (!IsPositionBlocked(futurePos444, futureRot444))
                                {
                                    SmoothMove(activeBlock.transform.position, futurePos444);
                                    Debug.Log("원하는 곳으로 이동 되었습니다.");

                                }
                                Vector3 futurePos4444 = activeBlock.transform.position;
                                Quaternion futureRot4444 = activeBlock.transform.rotation;
                                if (!IsSetPositionBlocked(futurePos4444, futureRot4444))
                                {
                                    Debug.Log("원하는곳 블락성공");
                                    Invoke("StopBox", 1f);
                                    //StopBox();


                                }
                                else
                                {
                                    Invoke("StopBox", 1f);
                                }
                                goto EXIT;
                            }
                        }
                        else if (main2BlockFactory.CurrentBox().Equals("Block5"))
                        {
                            if (!blocked[x, y, z] && !blocked[x + 1, y, z] && !blocked[x, y, z + 1] && !blocked[x + 1, y, z + 1])
                            {
                                Debug.Log("if에서 Block5에서의 좌표" + x + "," + y + "," + z);
                                Vector3 futurePos555 = activeBlock.transform.position + new Vector3(x, y, z);
                                Quaternion futureRot555 = activeBlock.transform.rotation;
                                if (!IsPositionBlocked(futurePos555, futureRot555))
                                {
                                    SmoothMove(activeBlock.transform.position, futurePos555);
                                    Debug.Log("원하는 곳으로 이동 되었습니다.");
                                }
                                Vector3 futurePos5555 = activeBlock.transform.position;
                                Quaternion futureRot5555 = activeBlock.transform.rotation;
                                if (!IsSetPositionBlocked(futurePos5555, futureRot5555))
                                {
                                    Debug.Log("원하는곳 블락성공");
                                    Invoke("StopBox", 1f);
                                    //StopBox();


                                }
                                else
                                {
                                    Invoke("StopBox", 1f);
                                }
                                goto EXIT;
                            }

                        }
                        else if (main2BlockFactory.CurrentBox().Equals("Block6"))
                        {
                            if (!blocked[x, y, z] && !blocked[x + 1, y, z] && !blocked[x, y + 1, z] && !blocked[x + 1, y + 1, z])
                            {
                                Debug.Log("if에서 Block6에서의 좌표" + x + "," + y + "," + z);
                                Vector3 futurePos666 = activeBlock.transform.position + new Vector3(x, y, z);
                                Quaternion futureRot666 = activeBlock.transform.rotation;
                                if (!IsPositionBlocked(futurePos666, futureRot666))
                                {
                                    SmoothMove(activeBlock.transform.position, futurePos666);
                                    Debug.Log("원하는 곳으로 이동 되었습니다.");

                                }
                                Vector3 futurePos6666 = activeBlock.transform.position;
                                Quaternion futureRot6666 = activeBlock.transform.rotation;
                                if (!IsSetPositionBlocked(futurePos6666, futureRot6666))
                                {
                                    Debug.Log("원하는곳 블락성공");
                                    Invoke("StopBox", 1f);
                                    //StopBox();


                                }
                                else
                                {
                                    Invoke("StopBox", 1f);
                                }
                                goto EXIT;
                            }
                        }
                        else if (main2BlockFactory.CurrentBox().Equals("Block7"))
                        {
                            if (!blocked[x, y, z] && !blocked[x + 1, y, z] && !blocked[x, y + 1, z] && !blocked[x, y, z + 1] && !blocked[x + 1, y + 1, z] && !blocked[x + 1, y, z + 1] && !blocked[x, y + 1, z + 1] && !blocked[x + 1, y + 1, z + 1] &&
                                !blocked[x, y, z - 1])
                            {
                                Debug.Log("if에서 Block7에서의 좌표" + x + "," + y + "," + z);
                                Vector3 futurePos777 = activeBlock.transform.position + new Vector3(x, y, z);
                                Quaternion futureRot777 = activeBlock.transform.rotation;
                                if (!IsPositionBlocked(futurePos777, futureRot777))
                                {
                                    SmoothMove(activeBlock.transform.position, futurePos777);
                                    Debug.Log("원하는 곳으로 이동 되었습니다.");

                                }
                                Vector3 futurePos7777 = activeBlock.transform.position;
                                Quaternion futureRot7777 = activeBlock.transform.rotation;
                                if (!IsSetPositionBlocked(futurePos7777, futureRot7777))
                                {
                                    Debug.Log("원하는곳 블락성공");
                                    Invoke("StopBox", 1f);
                                    //StopBox();


                                }
                                else
                                {
                                    Invoke("StopBox", 1f);
                                }
                                goto EXIT;
                            }
                        }


                        isCr = false;
                    }




                }
            }
        }
    EXIT:;

    }



    private void UpKey()
    {
        Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 1, 0);
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
        }
        else
        {
            Debug.Log("else앞");
            Vector3 futurePosf = activeBlock.transform.position;
            Quaternion futureRotf = activeBlock.transform.rotation;
            if (!IsSetPositionBlocked(futurePosf, futureRotf))
            {
                Debug.Log("else앞성공");
                StopBox();
            }

        }

    }
    private void BackKey()
    {
        Vector3 futurePos = activeBlock.transform.position + new Vector3(0, 0, -1);
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

        Vector3 futurePos = activeBlock.transform.position + new Vector3(1, 0, 0);
        Quaternion futureRot = activeBlock.transform.rotation;
        if (!IsPositionBlocked(futurePos, futureRot))
        {
            SmoothMove(activeBlock.transform.position, futurePos);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        if (isCreate)
        {
            Invoke("CarryBox", 1f);
            isCreate = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 futurePos = activeBlock.transform.position;
            Quaternion futureRot = activeBlock.transform.rotation;
            if (!IsSetPositionBlocked(futurePos, futureRot))
            {
                StopBox();
            }

        }

    }
    private void CreateBox(int num)
    {
        activeBlock = (GameObject)GameObject.Instantiate(main2BlockFactory.GetNextBlock(num), new Vector3(0, 0, 0), Quaternion.identity);
        Debug.Log(num + "번째가 생성되었습니다.");
        BlockCheck();


        
        isCreate = true;
    }
    private void StopBox()
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

        
        if (dictionary.ContainsKey(main2BlockFactory.CurrentBox()))
        {

            dicList = dictionary[main2BlockFactory.CurrentBox()];
            
        }





        FreezeBlock();
        SetPositionBlocked();






    }

    private void initGrid()
    {
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                for (int k = 0; k < 12; k++)
                {

                    blocked[i, j, k] = false;

                    if ((i >= 9) || (j >= 9) || (k >= 10))
                    {
                        blocked[i, j, k] = true;
                    }

                }
            }
        }
    }

    void BlockCheck()
    {
        //vlist.Dispose();
        int count = 0;


        //currentBox = blockFactory.CurrentBox();
        if (dictionary.ContainsKey(main2BlockFactory.CurrentBox()))
        {

            boxsize = dictionary[main2BlockFactory.CurrentBox()];

        }
        //Debug.Log("size=" + boxsize);
        //x,y,z 좌표 순서
        bool Isy = false;
        bool Isz = false;
        string[] bsize = boxsize.Split(',');
        Debug.Log("bs0="+bsize[0]);
        Debug.Log("bs1=" + bsize[1]);
        Debug.Log("bs2=" + bsize[2]);
        int xsize = Int32.Parse(bsize[0]);
        int ysize = Int32.Parse(bsize[1]);
        int zsize = Int32.Parse(bsize[2]);
        vlist = new List<string>();
        if (xsize == 0 && ysize != 0)
        {
            Isy = true;
        }
        if (ysize == 0 && zsize != 0)
        {
            Isz = true;
        }
        for (int i = 0; i < 9; i++)
        {
            //좌표수 만큼 반복 7x7x12
            for (int j = 0; j < 9; j++)
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
                        if (count != 0 && count == xsize * ysize * zsize)
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




    private bool IsPositionBlocked(Vector3 futurePos, Quaternion futureRot)
    {
        foreach (Transform cube in activeBlock.transform.GetComponentsInChildren<Transform>())
        {
            if (cube.childCount == 0)
            {
                //Debug.Log ("Cube Pos: " + cube.position);
                //Debug.Log ("Future Pos: " + futurePos);
                try
                {
                    if (((int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) < 0) || ((int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z)) < 0) || ((int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) < 0) || ((int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) < 0) || ((int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z)) < 0) || ((int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) < 0)
                        || ((int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)) > 8) || ((int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z)) > 9) || ((int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)) > 8))
                    {
                        Debug.Log("3번째");
                        return true;
                    }
                    if (((int)(Mathf.Round(cube.transform.position.z))) > 9.5)
                    {
                        activeBlock.transform.position = new Vector3(activeBlock.transform.position.x, activeBlock.transform.position.y, 8);
                    }

                    /*
                    
                    if (blocked[
                            (int)Mathf.Round(futurePos.x + (cube.position.x - activeBlock.transform.position.x)),
                            (int)Mathf.Round(futurePos.y + (cube.position.y - activeBlock.transform.position.y)),
                            (int)Mathf.Round(futurePos.z + (cube.position.z - activeBlock.transform.position.z))])
                    {
                        
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
                        Debug.Log("1번째");


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
                    Debug.Log("SetPosition에서 블락됨");
                }
            }
            catch
            {
                SceneManager.LoadScene("Main");
            }
        }

        dataSend = ((int)Mathf.Round(activeBlock.transform.position.x)) + "," +
                   ((int)Mathf.Round(activeBlock.transform.position.y)) + "," +
                   ((int)Mathf.Round(activeBlock.transform.position.z)) + "," + dicList ;
        boxes.Add(dataSend);
        Debug.Log("이 박스의 정보는 " + dataSend);
        vlist.Clear();

        if(boxNum == 1) // 8개의 상자를 다 쌓았을 때
        {
            Debug.Log("상자끝끝");
            
            /*
            string rsStr = cnt + "/";
            for (int i = 0; i < boxes.Count; i++)
            {
                rsStr += boxes[i] + "/";
            }
            Debug.Log(rsStr);
            */
            
            //SendArduino();
        }
        else
        {
            boxNum++;
            CreateBox(boxNum);
        }
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
