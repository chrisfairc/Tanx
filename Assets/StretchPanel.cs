using UnityEngine;
using System.Collections;

public class StretchPanel : MonoBehaviour {

	public int minScreenPercent = 15;
	ScreenOrientation orientation;

	// Use this for initialization
	void Start () {
		orientation = Screen.orientation;
		FitWidth();
	}
	
	// Update is called once per frame
	void Update () {
		if(Screen.orientation != orientation)
		{
			FitWidth();
		}
	}

	void FitWidth()
	{
		RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
		if(rectTrans == null) return;
		rectTrans.sizeDelta = new Vector2(Screen.width * minScreenPercent / 100, rectTrans.sizeDelta.y);
	}
}
