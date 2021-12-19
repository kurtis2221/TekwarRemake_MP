using UnityEngine;
using System.Collections;

public class BarrelScript : MonoBehaviour {

	public AudioClip barrel_explosion;
	
	void Start ()
	{
	
	}
	
	public void Explode()
	{
		GetComponent<NetworkView>().RPC("ExplodeMP",RPCMode.All);
	}
	
	[RPC]
	void ExplodeMP()
	{
		name = "ebarrel_exp";
		Collider[] other = Physics.OverlapSphere(transform.position,10.0f);
		for(int i = 0; i < other.Length; i++)
		{
			if(other[i].name == "ebarrel")
				other[i].GetComponent<BarrelScript>().Explode();
			else if(other[i].name.Contains("Player") && other[i].GetComponent<NetworkView>().isMine)
				other[i].GetComponent<MainScript>().hp = 0;
			else if(other[i].name == "cop")
				other[i].GetComponent<cop_nav>().DeathTrigger();
			else if(other[i].name == "civilian")
				other[i].GetComponent<civil_nav>().DeathTrigger();
		}
			
		GetComponent<Light>().enabled = true;
		StartCoroutine(Wait2());
		GetComponent<AudioSource>().PlayOneShot(barrel_explosion);
		GetComponent<Renderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
		StartCoroutine(Wait());
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(2.0f);
		Destroy(gameObject);
	}
	
	IEnumerator Wait2()
	{
		yield return new WaitForSeconds(0.2f);
		GetComponent<Light>().enabled = false;
	}
}
