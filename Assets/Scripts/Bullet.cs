using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		velocity += new Vector3(0f,3f,0f);
		transform.Translate(velocity);

	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(velocity);
		velocity *= 0.9f;
  		if(transform.position.y < -10)
		{
			Explode();
		} 
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.name.StartsWith("TanxTerrain"))
		{
			Terrain oTerrain = collision.gameObject.GetComponent<Terrain>();
			float normx =  (transform.position.x - oTerrain.transform.position.x) / oTerrain.terrainData.size.x;
			float normz =  (transform.position.z - oTerrain.transform.position.z) / oTerrain.terrainData.size.z;

			int iBlowX = (int)(normx * oTerrain.terrainData.heightmapWidth);
			int iBlowZ = (int)(normz * oTerrain.terrainData.heightmapHeight);

			float[,] heightBlow = oTerrain.terrainData.GetHeights(iBlowX, iBlowZ, 1, 1);
			heightBlow[0,0] -= 1f;
				
			oTerrain.terrainData.SetHeights(iBlowX, iBlowZ, heightBlow);
		}

		Explode();
	}

	void Explode()
	{
		GameObject xplo = (GameObject)Instantiate(Resources.Load("Xplosion"), transform.position, transform.rotation);
		Destroy(xplo, xplo.GetComponent<ParticleSystem>().duration); 
		GameObject.Destroy(this.gameObject);
	}
}
