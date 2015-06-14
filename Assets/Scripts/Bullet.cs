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
			TanxTerrain oTerrain = collision.gameObject.GetComponent<TanxTerrain>();
			StartCoroutine(oTerrain.DentTerrain(transform.position, 0.03f));
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
