using UnityEngine;
using System.Collections;

public class nav_path : MonoBehaviour {
	
	public Transform target;
	public Color boxcolor = Color.white;
	
	void OnDrawGizmos()
	{
		Gizmos.color = boxcolor;
		Gizmos.DrawCube(transform.position,new Vector3(1,2,1));
	}
	
	void OnDrawGizmosSelected()
	{
		if(target != null)
		{
	        	Gizmos.color = Color.red;
	        	Gizmos.DrawLine(transform.position, target.position);
		}
	}
}
