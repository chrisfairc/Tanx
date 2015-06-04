using UnityEngine;
using System.Collections;

public class TanxTerrain : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		Terrain t = gameObject.GetComponent<Terrain>();
		TerrainData dataClone = Instantiate(t.terrainData) as TerrainData;

		GameObject newTerrainObj = Terrain.CreateTerrainGameObject(dataClone);
		newTerrainObj.name = "TanxTerrain1";

		newTerrainObj.transform.position = t.transform.position;
		newTerrainObj.transform.rotation = t.transform.rotation;
		Terrain newTerrain = newTerrainObj.GetComponent(typeof(Terrain)) as Terrain;
		
		newTerrain.treeDistance = t.treeDistance;
		newTerrain.treeBillboardDistance = t.treeBillboardDistance;
		newTerrain.treeCrossFadeLength = t.treeCrossFadeLength;
		newTerrain.treeMaximumFullLODCount = t.treeMaximumFullLODCount;
		newTerrain.detailObjectDistance = t.detailObjectDistance;
		newTerrain.heightmapPixelError = t.heightmapPixelError;
		newTerrain.heightmapMaximumLOD = t.heightmapMaximumLOD;
		newTerrain.basemapDistance = t.basemapDistance;
		newTerrain.castShadows = t.castShadows;

		DestroyImmediate(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
