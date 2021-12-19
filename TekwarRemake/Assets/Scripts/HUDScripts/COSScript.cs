using UnityEngine;
using System.Collections;

public class COSScript : MonoBehaviour {
	
	GameObject playerobj;
	float tmp;
	float cos;
	float cos_check;
	float cos_max;
	
	void Start()
	{
		cos_max = GetComponent<GUITexture>().pixelInset.width;
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
		if(playerobj == null)
			playerobj = GameObject.Find("Player(Clone)");
		else if(playerobj != null)
		{
			cos = playerobj.GetComponent<MainScript>().cos;
			if(cos_check != cos)
			{
			tmp = cos / 100 * cos_max;
			GetComponent<GUITexture>().pixelInset = new Rect(
				GetComponent<GUITexture>().pixelInset.x,
				GetComponent<GUITexture>().pixelInset.y,
				tmp,
				GetComponent<GUITexture>().pixelInset.height);
			}
			cos_check = playerobj.GetComponent<MainScript>().cos;
		}
	}
}