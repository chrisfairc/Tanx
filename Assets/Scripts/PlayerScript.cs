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
		TurnRight();
	}

	public void RightTrackDrag(BaseEventData data)
	{
		TurnLeft();

	}

	void Update () 
	{
		if(remote) return;

		ellapsedTime += Time.deltaTime;

		if(Input.GetAxis ("Horizontal1") < -.001f )//|| Input.mousePosition)
		{
			TurnLeft();
		}

		if(Input.GetAxis ("Horizontal1") > .001f)
		{
			TurnRight();
		}

		if(Input.GetAxis ("Vertical1") < -.001f)
		{
			Reverse ();
		}

		if(Input.GetAxis ("Vertical1") > .001f)
		{
			Accelerate ();
		}

		if(Input.GetAxis ("Vertical2") < -.001f)
		{
			LowerAim ();
		}
		
		if(Input.GetAxis ("Vertical2") > .001f)
		{
			RaiseAim ();
		}

		if(Input.GetAxis ("Horizontal2") < -.001f)
		{
			AimLeft();
		}
		
		if(Input.GetAxis ("Horizontal2") > .001f)
		{
			AimRight();
		}

		if(Input.GetButtonDown ("Fire") )
		{
			Fire ();
		}
		
		if(Input.GetButtonDown ("Jump"))
		{
			Reground ();
		}
		
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
	
	public void TurnLeft()
	{
		if(isgrounded)
			angular -= 0.1f;

	}
	
	public void TurnRight()
	{
		if(isgrounded)
			angular += 0.1f;

	}
	
	public void Reverse()
	{
		if(isgrounded)
			veloc -= 0.01f;

	}
	
	public void Accelerate()
	{
		if(isgrounded)
			veloc += 0.02f;
	}

	public void RaiseAim()
	{
		if(isgrounded)
			aim.Rotate(-1f,0f,0f);
		
	}
	
	public void LowerAim()
	{
		if(isgrounded)
			aim.Rotate(1f,0f,0f);
	}

	public void AimLeft()
	{
		if(isgrounded)
			aim.Rotate(0f,0f,1f);
		
	}
	
	public void AimRight()
	{
		if(isgrounded)
			aim.Rotate(0f,0f,-1f);
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
