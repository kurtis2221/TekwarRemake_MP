using UnityEngine;
using System.Collections;

public class HUDGren2 : MonoBehaviour {
	
	void Start()
	{
		
	}
	
	public void ShowInfo()
	{
		GetComponent<GUITexture>().enabled = true;
		StartCoroutine(HideInfo());
	}
	
	IEnumerator HideInfo()
	{
		yield return new WaitForSeconds(2.0f);
		GetComponent<GUITexture>().enabled = false;
	}
}
