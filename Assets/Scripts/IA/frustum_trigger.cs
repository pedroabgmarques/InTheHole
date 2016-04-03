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
    private Vector3 offset;
	// Use this for initialization
	void Start () 
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        //alarm = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyPatrolAndFollow>().alarm;
        enemyPatrol = transform.root.GetComponent<EnemyPatrolAndFollow>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {

        print(other.transform.name + ": " + other.transform.tag);

        offset = new Vector3(0, 1.6f, 0);

        ray.origin = enemy.transform.position + offset;
        ray.direction = (player.transform.position + offset) - (enemy.transform.position + offset);

        print(other.transform.name +": "+ other.transform.tag);
        Debug.DrawRay(ray.origin, ray.direction * 200, Color.red);

        if (Physics.Raycast(ray, out rayHit))
        {
            if (rayHit.transform.tag == "Player" || rayHit.transform.tag == "pescoco_player")
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
    }
}
