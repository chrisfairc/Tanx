using UnityEngine;
using System.Collections;

public class TanxNetworking : MonoBehaviour {

	public PlayerScript player;
	public int port = 89763;
	public bool foundPeer = false;
	public string peerIPAddress;

	// Use this for initialization
	void Start () {
	
		StartCoroutine(StartListening());
		StartCoroutine(StartScanning());
	}
	
	// Update is called once per frame
	void Update () {

		if(foundPeer)
		{
			GetPeerUpdates();
			UpdatePeer();
		}
	
	}

	public IEnumerator StartScanning()
	{
		//send a query packet to every ip on local subnet

		//wait for ack
		yield return null;
	}

	public IEnumerator StartListening()
	{
		//listen for a query packet
		
		//send ack
		yield return null;

	}

	public void UpdatePeer()
	{
		//send position info

		//send bullet info

	}

	public void GetPeerUpdates()
	{
		//get position info
		
		//get bullet info
		
	}

}
