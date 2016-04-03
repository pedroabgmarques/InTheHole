using UnityEngine;
using System.Collections;

public class frustum_trigger : MonoBehaviour {


    Ray ray;
    
    RaycastHit rayHit;

    GameObject enemy;
    GameObject player;
    GameObject olhos;
    EnemyPatrolAndFollow enemyPatrol;
    bool alarm;
	// Use this for initialization
	void Start () 
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        //alarm = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyPatrolAndFollow>().alarm;
        enemyPatrol = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyPatrolAndFollow>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        //Debug.Log("TRIGGER");
        //Debug.Log(other.gameObject.tag);
        //if (other.gameObject.tag == "Player")
        //{
        //    Debug.Log("trigger player");
            

            ray.origin = enemy.transform.position + new Vector3(0, 1.5f, 0);
            ray.direction = (player.transform.position) - (enemy.transform.position);


            if (Physics.Raycast(ray, out rayHit))
            {
                if (rayHit.transform.tag == "Player")
                {
                    enemyPatrol.alarm = true;
                    Debug.Log("Inimigo consegue ver o player!");
                }
                else
                {
                    enemyPatrol.alarm = false;
                    Debug.Log("nao é o player");
                    Debug.Log(rayHit.transform.tag);
                }

            }
        //}
    }
}
