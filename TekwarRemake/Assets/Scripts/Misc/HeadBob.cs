using UnityEngine;
using System.Collections;

public class HeadBob : MonoBehaviour {
	
	Vector3 tmp;
	float count = 0.0f;
	bool isup = true;
	
	public float move = 0.01f;
	public float limit = 0.1f;
	
	void FixedUpdate()
    {
		if(Vector3.Distance(tmp,transform.position) > 0.1)
		{
			if(count >= limit) isup = false;
			else if(count <= 0) isup = true;
				
			if(isup)
			{
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y + move,transform.localPosition.z);
			count += move;
			}
			else
			{
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y - move,transform.localPosition.z);
			count -= move;	
			}
		}
		tmp = transform.position;
	}
}