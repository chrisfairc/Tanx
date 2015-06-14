using UnityEngine;
using System.Collections;

public class TanxUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ToggleShow(Transform deactivate)
	{
		gameObject.SetActive(!gameObject.activeInHierarchy);
		deactivate.gameObject.SetActive(!gameObject.activeInHierarchy);

	}
}
