using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour 
{
	public float strafeSpeed = 2f;

	public float ellapsedTime;
	public float veloc = 0f;
	public float angle = 0f;
	public float angular = 0f;
	public bool isgrounded = false;
	public Transform aim;
	public Transform barrel;

	private float leftTrackSpeed = 0f;
	private float rightTrackSpeed = 0f;

	public bool remote = false;

	void Start () 
	{
		if(remote)
		{
			this.gameObject.SetActive(false);
		}
		ellapsedTime = 0f;
	}

	public void LeftTrackDrag(BaseEventData data)
	{
		PointerEventData pointerData = data as PointerEventData;
		float fDist = pointerData.position.y - pointerData.pressPosition.y;
		leftTrackSpeed = fDist/Screen.height;

	}
	
	public void LeftTrackDragUp(BaseEventData data)
	{
		leftTrackSpeed = 0;
	}

	public void RightTrackDrag(BaseEventData data)
	{
		PointerEventData pointerData = data as PointerEventData;
		float fDist = pointerData.position.y - pointerData.pressPosition.y;
		rightTrackSpeed = fDist/Screen.height;

	}
	
	public void RightTrackDragUp(BaseEventData data)
	{
		rightTrackSpeed = 0;
	}

	public void AimDrag(BaseEventData data)
	{
		PointerEventData pointerData = data as PointerEventData;
		aim.Rotate( -pointerData.delta.y * 100 / Screen.width, 0f, -pointerData.delta.x * 100 / Screen.height);

	}

	void Update () 
	{
		if(remote) return;

		ellapsedTime += Time.deltaTime;

		if(Input.GetButtonDown ("Jump"))
		{
			Reground ();
		}

		veloc = rightTrackSpeed + leftTrackSpeed;
		angular = leftTrackSpeed - rightTrackSpeed ;

		transform.Translate(0f, 0f, veloc);
		transform.Rotate (0f, angle, 0f);
		angle += angular;

		//apply friction
		veloc *= 0.9f;
		angular *= 0.6f;
		angle *= 0.9f;

		if(transform.position.y < -10)
		{
			Explode();
		} 
	}


	void FixedUpdate()
	{


	}

	public void Fire()
	{
		GameObject bullet = (GameObject)Instantiate(Resources.Load("Bullet"), barrel.position, aim.rotation);
		Bullet oBullet = bullet.GetComponent<Bullet>();
		oBullet.velocity = new Vector3(0f, veloc, 0f);

	}

	void Restart()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	public void Reground()
	{
		Explode();

	}

	void OnCollisionEnter(Collision collision) 
	{
		if(collision.gameObject.name.StartsWith("TanxTerrain"))
		{
			isgrounded = true;
		}
		
	}

	void OnCollisionExit(Collision collision) 
	{
		if(collision.gameObject.name.StartsWith("TanxTerrain"))
		{
			isgrounded = false;
		}
		
	}

	void RandomSpawn()
	{
		transform.position = new Vector3(1650,3,1450);
		transform.rotation = new Quaternion();

	}

	void Explode()
	{
		GameObject xplo = (GameObject)Instantiate(Resources.Load("Xplosion"), transform.position, transform.rotation);
		Destroy(xplo, xplo.GetComponent<ParticleSystem>().duration); 
		RandomSpawn();
	}
}
