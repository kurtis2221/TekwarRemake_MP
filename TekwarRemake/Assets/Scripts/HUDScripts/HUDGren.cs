using UnityEngine;
using System.Collections;

public class HUDGren : MonoBehaviour {
	
	public GameObject gren_img;
	GameObject playerobj;
	int current, g_check;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(playerobj == null)
			playerobj = GameObject.Find("Player(Clone)");
		else if(playerobj != null)
		{
			current = playerobj.GetComponentInChildren<WeaponScript>().gren_ammo;
			if(g_check != current)
			{
				ShowInfo();
				gren_img.GetComponent<HUDGren2>().ShowInfo();	
			}
			g_check = playerobj.GetComponentInChildren<WeaponScript>().gren_ammo;
		}
	}
	
	public void ShowInfo()
	{
		GetComponent<GUIText>().text = current.ToString();
		GetComponent<GUIText>().enabled = true;
		StartCoroutine(HideInfo());
	}
	
	IEnumerator HideInfo()
	{
		yield return new WaitForSeconds(2.0f);
		GetComponent<GUIText>().enabled = false;
	}
}
