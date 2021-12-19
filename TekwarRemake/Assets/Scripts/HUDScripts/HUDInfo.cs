using UnityEngine;
using System.Collections;

public class HUDInfo : MonoBehaviour {

	void Start () {
		GetComponent<GUIText>().material.color = new Color(0.0f,0.5f,1.0f);
	}
}
