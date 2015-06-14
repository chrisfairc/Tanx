using UnityEngine;
using System.Collections;

public class TanxTerrain : MonoBehaviour {


	public IEnumerator DentTerrain(Vector3 oPosn, float fAmt )
	{
		Terrain oTerrain = gameObject.GetComponent<Terrain>();
		float normx =  (oPosn.x - oTerrain.transform.position.x) / oTerrain.terrainData.size.x;
		float normz =  (oPosn.z - oTerrain.transform.position.z) / oTerrain.terrainData.size.z;
		
		int iBlowX = (int)(normx * oTerrain.terrainData.heightmapWidth);
		int iBlowZ = (int)(normz * oTerrain.terrainData.heightmapHeight);
		
		float[,] heightBlow = oTerrain.terrainData.GetHeights(iBlowX, iBlowZ, 1, 1);
		heightBlow[0,0] -= fAmt;
		
		oTerrain.terrainData.SetHeights(iBlowX, iBlowZ, heightBlow);
		yield return null;
	}
}
