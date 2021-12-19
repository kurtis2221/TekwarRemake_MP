using UnityEngine;
using System.Collections;

public class TimeScript : MonoBehaviour {
	
	public GUIText input;
	int h = 0, m = 0, s = 0;
	
	// Update is called once per frame
	void Start () {
		StartCoroutine(Tick ());
	}
	
	IEnumerator Tick()
	{
		s+=1;
		if(s > 59)
		{
			s = 0;
			m += 1;
		}
		if(m > 59)
		{
			m = 0;
			h += 1;
		}
		input.text =
			((h < 10) ? "0" : null) + h.ToString() + ":" +
			((m < 10) ? "0" : null) + m.ToString() + ":" +
			((s < 10) ? "0" : null) + s.ToString();		
		yield return new WaitForSeconds(1f);
		//Start a loop
		StartCoroutine(Tick ());
	}
}
