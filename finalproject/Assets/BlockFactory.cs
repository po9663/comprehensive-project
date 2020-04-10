using UnityEngine;
using System.Collections;

public class BlockFactory {

	//public GameObject BLOCKSMALL;
	//public GameObject BLOCKSTICK;
	//public GameObject BLOCKL;
	//public GameObject BLOCKSQUARE;
	GameObject[] blockTypes;
	Material[] materialTypes;

	public BlockFactory(GameObject a, GameObject b, GameObject c, GameObject d,
	                    Material e, Material f, Material g) {
		blockTypes = new GameObject[4];
		blockTypes[0] = a;
		blockTypes[1] = b;
		blockTypes[2] = c;
		blockTypes[3] = d;

		materialTypes = new Material[4];
		materialTypes[0] = e;
		materialTypes[1] = f;
		materialTypes[3] = g;
	}

	public GameObject GetNextBlock() {	
		int num = ((int)Mathf.Round (Random.value * 10000)) % 4;
		GameObject block = blockTypes[num];
		num = ((int)Mathf.Round (Random.value * 10000)) % 3;
		foreach (MeshRenderer mr in block.GetComponentsInChildren<MeshRenderer>())
		{
			Debug.Log("here");
			mr.material = materialTypes[num];
		}
		return block;

	}
}
