using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using System.Diagnostics;
using System.Text;

public class TanxPlayer : MonoBehaviour 
{
	public float ellapsedTime;
	public float veloc = 0f;
	public float angle = 0f;
	public float angular = 0f;
	public bool isgrounded = false;
	public Transform aim;
	public Transform barrel;

	public float leftTrackSpeed = 0f;
	public float rightTrackSpeed = 0f;

	public bool remote = false;

	public float bulletDelaySecs = 1f;	//second delay for next bullet 
	Stopwatch stopwatch = new Stopwatch();

	void Start () 
	{
		stopwatch.Start();

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
		leftTrackSpeed = (fDist/2)/Screen.height;

	}
	
	public void LeftTrackDragUp(BaseEventData data)
	{
		leftTrackSpeed = 0;
	}

	public void RightTrackDrag(BaseEventData data)
	{
		PointerEventData pointerData = data as PointerEventData;
		float fDist = pointerData.position.y - pointerData.pressPosition.y;
		rightTrackSpeed = (fDist/2)/Screen.height;

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

		if(Input.GetAxis("Vertical1") != 0)
		{
			leftTrackSpeed = Input.GetAxis("Vertical1")/5;
		}
		if(Input.GetAxis("Vertical2") != 0)
		{
			rightTrackSpeed = Input.GetAxis("Vertical2")/5;
		}

		if(Input.GetButtonDown ("Jump"))
		{
			Reground ();
		}

		if(isgrounded)
		{
			float speed = rightTrackSpeed + leftTrackSpeed;
			veloc += speed / 10;
			angular = leftTrackSpeed - rightTrackSpeed ;
		}

		transform.Translate(0f, 0f, veloc);
		transform.Rotate (0f, angle, 0f);
		angle += angular;

		//apply friction
		veloc *= 0.9f;
		angular *= 0.6f;
		angle *= 0.9f;
		if(angle < 0.0001f && angle > -0.0001f) angle = 0f;
		if(angular < 0.0001f && angular > -0.0001f) angular = 0f;
		if(veloc < 0.0001f) veloc = 0f;

		if(transform.position.y < -10)
		{
			Explode();
		} 
	}


	public void Fire()
	{
		if(stopwatch.ElapsedMilliseconds < 1000) return;
		stopwatch.Reset();
		stopwatch.Start();
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

	public void RandomSpawn()
	{
		transform.position = new Vector3(1600 + UnityEngine.Random.Range(10,200) ,3,1400 + UnityEngine.Random.Range(10,200));
		transform.rotation = new Quaternion();

	}

	void Explode()
	{
		GameObject xplo = (GameObject)Instantiate(Resources.Load("Xplosion"), transform.position, transform.rotation);
		Destroy(xplo, xplo.GetComponent<ParticleSystem>().duration); 
		RandomSpawn();
	}

	public void LogSerialize()
	{
		UnityEngine.Debug.Log(SerializeInfo());
	}

	public string SerializeInfo()
	{
		StringBuilder builder = new StringBuilder();
		builder.Append(gameObject.transform.position);
		builder.Append("|");
		builder.Append(gameObject.transform.rotation);
		builder.Append("|");
		builder.Append(veloc);
		builder.Append("|");
		builder.Append(angle);
		builder.Append("|");
		builder.Append(angular);
		builder.Append("|");
		builder.Append(aim.rotation);


		return builder.ToString();
	}

	public bool DeSerializeInfo(string sInfo)
	{
		return false;
	}

	public void ApplyUpdates (string str)
	{

	}
}
