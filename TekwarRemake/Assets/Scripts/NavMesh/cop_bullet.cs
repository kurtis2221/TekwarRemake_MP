using UnityEngine;
using System.Collections;

public class cop_bullet : MonoBehaviour {
	
	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3 (0, 0, 100));
		GetComponent<Renderer>().enabled = false;
		ShootBullet();
		StartCoroutine(ShowUp());
		StartCoroutine(DestroyAuto());
	}
	
	Ray ray;
	RaycastHit hit = new RaycastHit();
	bool hit_before = false;
	
	void FixedUpdate()
	{
		ShootBullet();
	}
	
	void ShootBullet()
	{
		if(!hit_before)
		{
			ray = new Ray(transform.position,transform.forward);
			Physics.SphereCast(ray,0.5f,out hit,2.0f);
			if(hit.collider)
			{
				if(hit.collider.gameObject.name.Contains("Player"))
					hit.collider.GetComponent<NetHit>().SendHit(15, 10, 15,
							hit.collider.GetComponent<NetworkView>().viewID,gameObject.GetComponent<NetworkView>().owner,true);
				else if(hit.collider.gameObject.name == "cop" && gameObject != hit.collider)
					hit.collider.GetComponent<cop_nav>().DeathTrigger();
				else if(hit.collider.gameObject.name == "civilian" && gameObject)
					hit.collider.GetComponent<civil_nav>().DeathTrigger();
				InitDestroy();
			}
		}	
	}
	
	void InitDestroy()
	{
		if(GetComponent<NetworkView>().isMine)
		{
		Network.RemoveRPCs(gameObject.GetComponent<NetworkView>().viewID);
		Network.Destroy(gameObject);
		Destroy(gameObject);
		}
	}
	
	IEnumerator DestroyAuto()
	{
		if(GetComponent<NetworkView>().isMine)
		{
		yield return new WaitForSeconds(3);
		Network.RemoveRPCs(gameObject.GetComponent<NetworkView>().viewID);
		Network.Destroy(gameObject);
		Destroy(gameObject);	
		}
	}
	
	IEnumerator ShowUp()
	{
		yield return new WaitForSeconds(0.04f);
		GetComponent<Renderer>().enabled = true;
	}
}