using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	GameObject playerobj;
	public int score;
	int score_check;
	string tmp = null;
	
	void Start()
	{
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
		if(playerobj == null)
			playerobj = GameObject.Find("Player(Clone)");
		else if(playerobj != null)
		{
			if(score_check != score)
			{
				GetComponent<GUIText>().text = score.ToString();
				if(GetComponent<GUIText>().text.Length < 7)
				{
					tmp = null;
					for(uint i = 0; i < 7-GetComponent<GUIText>().text.Length; i++)
						tmp += "0";
					GetComponent<GUIText>().text = tmp+GetComponent<GUIText>().text;
				}
			}
			score_check = score;
		}
	}
}
