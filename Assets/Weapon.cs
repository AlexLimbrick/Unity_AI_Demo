using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float Damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider collision)
    {

        if(collision.gameObject.GetComponent<HitMe>() != null)
        {
            collision.gameObject.GetComponent<HitMe>().Hit(Damage);
        }
        if(collision.gameObject.layer == 10)
        {
            collision.GetComponent<FireboltMove>().reflected = true;
        }
        
    }
}
