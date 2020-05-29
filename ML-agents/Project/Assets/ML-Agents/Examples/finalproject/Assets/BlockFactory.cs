using UnityEngine;
using System.Collections;

public class BlockFactory {

	//public GameObject BLOCKSMALL;
	//public GameObject BLOCKSTICK;
	//public GameObject BLOCKL;
	//public GameObject BLOCKSQUARE;
	GameObject[] blockTypes;
	Material[] materialTypes;
    
    public string box = "";
    public BlockFactory()
    {

    }
	public BlockFactory(GameObject a, GameObject b, GameObject c, GameObject d, GameObject e, GameObject f, GameObject g, Material k, Material l, Material m) {
		blockTypes = new GameObject[7];
		blockTypes[0] = a;
		blockTypes[1] = b;
		blockTypes[2] = c;
		blockTypes[3] = d;
        blockTypes[4] = e;
        blockTypes[5] = f;
        blockTypes[6] = g;

        materialTypes = new Material[4];
		materialTypes[0] = k;
		materialTypes[1] = l;
		materialTypes[3] = m;
        

	}

	public GameObject GetNextBlock() {	
		int num = ((int)Mathf.Round (Random.value * 10000)) % 7;
        num = 2;
		GameObject block = blockTypes[num];
        num = ((int)Mathf.Round (Random.value * 10000)) % 3;
		foreach (MeshRenderer mr in block.GetComponentsInChildren<MeshRenderer>())
		{
			//Debug.Log("BlockFactory here");
			mr.material = materialTypes[num];
		}
        box = block.ToString().Substring(0,6);
        Debug.Log("팩토리에서의" + box);
		return block;
	}
    public string CurrentBox()
    {
        return box;
    }
}
