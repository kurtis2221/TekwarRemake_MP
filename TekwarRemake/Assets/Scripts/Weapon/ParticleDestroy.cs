using UnityEngine;
using System.Collections;

public class ParticleDestroy : MonoBehaviour {

	void Start () {
		StartCoroutine(AutoDestroy());
	}
	
	IEnumerator AutoDestroy()
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}
}
