using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Horzontal")>0)
        {
            transform.localScale = new Vector3(1, 1);
        }
        if (Input.GetAxis("Horzontal") < 0)
        {
            transform.localScale = new Vector3(-1, 1);
        }
    }
}
