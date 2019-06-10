using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireboltMove : MonoBehaviour {

    public Vector3 target;
    public float Damage;
    public bool reflected = false;
    bool once = false;
	// Use this for initialization
	void Start () {
        transform.LookAt(target, new Vector3(0, 1, 0));
        reflected = false;
        once = false;
    }
	
	// Update is called once per frame
	void Update () {

        if(reflected && !once)
        {
            transform.Rotate(new Vector3(0, 1, 0), 180f);
            once = true;
        }
        
        transform.Translate(new Vector3(0, 0, 0.1f));
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);


    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 9 && !reflected)
        {
            Debug.Log(collision.gameObject.layer);
            if (collision.gameObject.GetComponent<HitMe>() != null)
            {
                collision.gameObject.GetComponent<HitMe>().Hit(Damage);
            }
            Debug.Log("hi");
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 9 && reflected)
        {
            if (collision.gameObject.GetComponent<HitMe>() != null)
            {
                collision.gameObject.GetComponent<HitMe>().Hit(Damage);
            }
            Debug.Log("Wizard");
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer != 9)
        {
            Destroy(gameObject);
        }

    }
    
}
