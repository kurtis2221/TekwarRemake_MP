using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
	
	public int menuid;
	public GameObject gcamera;
	public AudioClip menu_hover;
	public Object[] image;
	
	void Start()
	{
		GetComponent<Renderer>().material.mainTexture = (Texture)image[0];
		Cursor.visible = true;
		Screen.lockCursor = false;
	}
	
	// Use this for initialization
	void OnMouseEnter () {
		GetComponent<Renderer>().material.mainTexture = (Texture)image[1];
		gcamera.GetComponent<AudioSource>().PlayOneShot(menu_hover);
	}
	
	// Update is called once per frame
	void OnMouseExit () {
		GetComponent<Renderer>().material.mainTexture = (Texture)image[0];
	}
	
	void OnMouseDown()
	{
		if(menuid == 0) Application.LoadLevel(2);
		else if(menuid == 1) Application.LoadLevel(3);
	}
}