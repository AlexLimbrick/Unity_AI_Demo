using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WizardFSM : MonoBehaviour,HitMe {
    private IEnumerator currState;
    private IEnumerator nextState;
    public GameObject player;
    public GameObject FireBolt;

    //public GameObject Sensor;
    NavMeshAgent agent;
    Animator anim;
    public float health;
    bool dying = false;
    float AttackCooldown;
    RaycastHit hit;
    float FireBoltDelay;

    public bool flipX
    {
        set
        {
            var scale = transform.localScale;
            scale.x = value ? -1f : 1f;
            transform.localScale = scale;
        }
        get
        {
            return transform.localScale.x < 0;
        }
    }
    private IEnumerator _FSM()
    {
        while (currState != null)
        {
            yield return StartCoroutine(currState);
            currState = nextState;
            nextState = null;
        }
    }
    public void Hit(float damage)
    {
        if(health>0)
        {
            health -= damage;
            anim.Play("WizardInjured");
        }
       
    }
    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        currState = Idle();
        StartCoroutine(_FSM());
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private IEnumerator Idle()
    {
        while (nextState == null)
        {
            Debug.DrawRay(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z));

            if (Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), out hit))
            {
                //Debug.DrawRay(transform.position, player.transform.position - transform.position);
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject == player)
                {
                    nextState = Chase();
                }
                if (health <= 0)
                {
                    nextState = Dead();
                }
                GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(90, 0, 0);
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            yield return null;
        }

    }
    private IEnumerator Chase()
    {
        while (nextState == null)
        {
            FireBoltDelay -= Time.deltaTime;
            if (player.transform.position.z > transform.position.z)
            {
                GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            if (player.transform.position.x > transform.position.x)
            {
                flipX = true;
            }
            else
            {

                flipX = false;
            }
            if (health <= 0)
            {
                nextState = Dead();
                agent.isStopped = true;
            }
            Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), out hit);
            
            if (Vector3.Distance(transform.position, player.transform.position) < 4)
            {
                
                agent.isStopped = true;
                
                transform.Translate(Vector3.Normalize(transform.position - player.transform.position) * 0.02f);
                
            }
            else if (Vector3.Distance(transform.position, player.transform.position) > 5 || hit.collider.gameObject != player)
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
            else
            {
                FireBolt.GetComponent<FireboltMove>().target = player.transform.position;
                FireBolt.transform.position = transform.position+new Vector3(0,1,0);
                if(FireBoltDelay <=0)
                {
                    Instantiate(FireBolt);
                    FireBoltDelay = 3f;
                }
                
                agent.isStopped = true;
            }
            
            GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(90, 0, 0);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            //anim.SetFloat("Speed", agent.velocity.magnitude);
            yield return null;
        }
    }
    private IEnumerator Dead()
    {
        while (nextState == null)
        {
            GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(90, 0, 0);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            if (!dying)
            {
                anim.Play("WizardDeath");
                dying = true;
            }

            if (!GetComponent<SpriteRenderer>().enabled)
            {
                Destroy(gameObject);
            }

            yield return null;
        }
    }
}
