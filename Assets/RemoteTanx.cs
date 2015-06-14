using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoteTanx : MonoBehaviour {

	public static Dictionary<string, TanxPlayer> remoteTanx = new Dictionary<string, TanxPlayer>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	public static TanxPlayer AddGetRemoteTank(string sIP)
	{
		if(remoteTanx.ContainsKey(sIP))
	   	{
			return remoteTanx[sIP];
		}else{
			TanxPlayer tank = Instantiate(Resources.Load("Tank")) as TanxPlayer;
			remoteTanx.Add(sIP, tank);
			return tank;
		}

	}

}
