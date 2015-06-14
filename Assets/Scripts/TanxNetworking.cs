using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.Collections.Generic;

public class TanxNetworking : MonoBehaviour {

	public TanxPlayer myTank;
	public int discoveryport 	= 19763;
	public int updateport 		= 19764;
	public int multiport		= 19765;
	public string myIP;

	//UdpClient oDiscoClient 		= null;
	//IPEndPoint oDiscoEndPoint 	= null;
	UdpClient oUpdateClient 	= null;
	IPEndPoint oUpdateEndPoint 	= null;
	bool bCheckNet 				= true;
	bool bDoDisco 				= true;

	UdpClient multiUdpClient	= null;
	IPEndPoint multiREndPoint 	= null;

	public static string sMulti = "239.3.17.19";

	public List<string> connectedIPs = new List<string>();
	public Dictionary<string, TanxPlayer> oRemoteTanks = new Dictionary<string, TanxPlayer>();

	// Use this for initialization
	void Start () {
		myIP = Network.player.ipAddress;
		//StartListening();
		//StartCoroutine(StartBroadcasting());
		RegisterMulticast();
		MulticastMe();
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePeers();

	}

	void OnDestroy()
	{
		UnRegisterMulticast();

	}


	public void StartListening()
	{
		/*
		try{
			oDiscoClient = new UdpClient(discoveryport);
			oDiscoClient.EnableBroadcast = true;
			oDiscoEndPoint = new IPEndPoint(IPAddress.Any, discoveryport);
			oDiscoClient.BeginReceive(DicoveryCallback, null);
			
			Debug.Log ("TanxNetworking opened discovery listener" );
		}catch(Exception ex){
			Debug.Log ("TanxNetworking StartListening discovery ex: " + ex.ToString());
		}
*/
		try{
			oUpdateClient = new UdpClient(updateport);
			oUpdateEndPoint = new IPEndPoint(IPAddress.Any, updateport);
			oUpdateClient.BeginReceive(UpdateCallback, null);
			
			Debug.Log ("TanxNetworking opened update listener" );
		}catch(Exception ex){
			Debug.Log ("TanxNetworking StartListening update ex: " + ex.ToString());
		}

	}

	/*
	private void DicoveryCallback(IAsyncResult oASyncRes)
	{
		byte[] oBytes = oDiscoClient.EndReceive(oASyncRes, ref oDiscoEndPoint);
		string received_data = Encoding.ASCII.GetString(oBytes, 0, oBytes.Length);
		Debug.Log("TanxNetworking Recieved: " + received_data);

		string sIP = received_data.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
		if(!connectedIPs.Contains(sIP) && sIP != myIP)
		{
			connectedIPs.Add(sIP);
		}

		if(bCheckNet)
		{
			//keep listening
			oDiscoClient.BeginReceive(DicoveryCallback, null);
		}

	}
*/

	private void UpdateCallback(IAsyncResult oASyncRes)
	{
		byte[] oBytes = oUpdateClient.EndReceive(oASyncRes, ref oUpdateEndPoint);
		string received_data = Encoding.ASCII.GetString(oBytes, 0, oBytes.Length);
		Debug.Log("TanxNetworking Recieved: " + received_data);

		string[] split = received_data.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
		if(split.Length > 2)
		{
			string sIP = split[1].Trim();
			if(!connectedIPs.Contains(sIP) && sIP != myIP)
			{
				connectedIPs.Add(sIP);
			}

			//do update of remote controlled tank
			TanxPlayer tank = RemoteTanx.AddGetRemoteTank(sIP);
			tank.ApplyUpdates(split[2].Trim());
		}

		if(bCheckNet)
		{
			//keep listening
			oUpdateClient.BeginReceive(UpdateCallback, null);
		}
	}

	private void MultiCallback(IAsyncResult oASyncRes)
	{
		byte[] oBytes = multiUdpClient.EndReceive(oASyncRes, ref multiREndPoint);
		string received_data = Encoding.ASCII.GetString(oBytes, 0, oBytes.Length);
		Debug.Log("TanxNetworking Recieved Multicast: " + received_data);
		
		string sIP = received_data.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
		if(!connectedIPs.Contains(sIP) && sIP != myIP)
		{
			connectedIPs.Add(sIP);
		}
		
		if(bCheckNet)
		{
			//keep listening
			multiUdpClient.BeginReceive(MultiCallback, null);
		}
		
	}

	void MulticastMe()
	{
		Debug.Log("TanxNetworking Multicasting Me " + myIP);
		try{
			UdpClient udpClient = new UdpClient(discoveryport);
			IPEndPoint multiSEndPoint = new IPEndPoint(IPAddress.Parse(sMulti), multiport);
			udpClient.MulticastLoopback = true;
			byte[] sendBytes = Encoding.ASCII.GetBytes("TanxConnect," + myIP);

			udpClient.Send(sendBytes, sendBytes.Length, multiSEndPoint);
		}
		catch ( Exception ex ){
			Debug.Log("TanxNetworking MulticastMe ex: " + ex.ToString());
		}

		
	}
	
	/*
	public IEnumerator StartBroadcasting()
	{
		//send broadcast discovery packets
		while(bDoDisco)
		{
			Debug.Log("TanxNetworking StartScanning sending discovery broadcast");
			//try class C broadcast
			int iFirstDot = myIP.IndexOf(".");
			int iSecondDot = myIP.IndexOf(".", iFirstDot+1);
			int iThirdDot = myIP.IndexOf(".", iSecondDot+1);
			string sBroadC = myIP.Substring(0, iThirdDot) + ".255";

			UdpClient udpClient = new UdpClient(sBroadC, discoveryport);
			udpClient.EnableBroadcast = true;
			Byte[] sendBytes = Encoding.ASCII.GetBytes("TanxConnect," + myIP);
			try{
				udpClient.Send(sendBytes, sendBytes.Length);
			}
			catch ( Exception ex ){
				Debug.Log("TanxNetworking StartScanning C ex: " + ex.ToString());
			}
			//try class B broadcast
			string sBroadB = myIP.Substring(0, iSecondDot) + ".255.255";
			udpClient = new UdpClient(sBroadB, discoveryport);
			udpClient.EnableBroadcast = true;
			sendBytes = Encoding.ASCII.GetBytes("TanxConnect," + myIP);
			try{
				udpClient.Send(sendBytes, sendBytes.Length);
			}
			catch ( Exception ex ){
				Debug.Log("TanxNetworking StartScanning B ex: " + ex.ToString());
			}

			//try generic broadcast
			udpClient = new UdpClient(IPAddress.Broadcast.ToString(), discoveryport);
			udpClient.EnableBroadcast = true;
			sendBytes = Encoding.ASCII.GetBytes("TanxConnect," + myIP);
			try{
				udpClient.Send(sendBytes, sendBytes.Length);
			}
			catch ( Exception ex ){
				Debug.Log("TanxNetworking StartScanning Gen ex: " + ex.ToString());
			}

			yield return new WaitForSeconds(1f);
		}

		yield return null;
	}
*/
	public void RegisterMulticast()
	{
		RegisterMulticast(sMulti);
	}

	public void RegisterMulticast(string sNewMultiIP)
	{
		try{
			multiUdpClient = new UdpClient(multiport);
			multiREndPoint = new IPEndPoint(IPAddress.Parse(sMulti), multiport);
			multiUdpClient.JoinMulticastGroup(IPAddress.Parse(sNewMultiIP));
			multiUdpClient.BeginReceive(MultiCallback, null);
			
			Debug.Log ("TanxNetworking RegisterMulticast at " + sMulti + ":" + multiport );
		}catch(Exception ex){
			Debug.Log ("TanxNetworking StartListening update ex: " + ex.ToString());
		}

	}

	public void UnRegisterMulticast()
	{
		Debug.Log ("TanxNetworking UnRegisterMulticast at " + sMulti + ":" + multiport );

		multiUdpClient.DropMulticastGroup(IPAddress.Parse(sMulti));

	}

	public void UpdatePeers()
	{
		foreach(string sIP in connectedIPs)
		{

			Debug.Log("TanxNetworking UpdatePeer " + sIP);

			UdpClient udpClient = new UdpClient(sIP, updateport);
			string sUpdate = myIP + "," + myTank.SerializeInfo();

			Byte[] sendBytes = Encoding.ASCII.GetBytes("TanxUpdate," + sUpdate);
			try{
				udpClient.Send(sendBytes, sendBytes.Length);
			}
			catch ( Exception ex ){
				Debug.Log("TanxNetworking StartScanning ex: " + ex.ToString());
			}
		}


	}


}
