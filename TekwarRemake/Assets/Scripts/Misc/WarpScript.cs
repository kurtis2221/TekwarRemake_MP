using UnityEngine;
using System.Collections;

public class WarpScript : MonoBehaviour {
	
	public float warpdist;
	public Transform targetobj;
	public GUIText hud_info;
	GameObject playerobj;
	
	// Update is called once per frame
    	void FixedUpdate()
    	{
		playerobj = GameObject.Find("Player(Clone)");
		if(playerobj != null)
		{
			if(playerobj.GetComponent<NetworkView>().isMine)
			{
				if(Vector3.Distance(playerobj.transform.position,transform.position) < warpdist)
				{
					hud_info.enabled = true;
					hud_info.text = "LOADING...";
					playerobj.GetComponent<CharacterMotor>().enabled = false;
					playerobj.GetComponent<FPSInputController>().enabled = false;
					StartCoroutine(WarpTimer());
				}
			}
		}
	}
	
	IEnumerator WarpTimer()
	{
		yield return new WaitForSeconds(1.0f);
		hud_info.enabled = false;
		playerobj.GetComponent<CharacterMotor>().enabled = true;
		playerobj.GetComponent<FPSInputController>().enabled = true;
		playerobj.transform.position = targetobj.transform.position;
		playerobj.transform.rotation = targetobj.transform.rotation;
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawRay(targetobj.position,targetobj.forward);
		Gizmos.color = Color.green;
		Gizmos.DrawCube(targetobj.position,new Vector3(1,2,1));
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position,warpdist);
		Gizmos.DrawLine(transform.position,targetobj.position);
	}
}
