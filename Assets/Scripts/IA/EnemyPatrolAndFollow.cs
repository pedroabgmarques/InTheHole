using UnityEngine;
using System.Collections;

public class EnemyPatrolAndFollow : MonoBehaviour {

    GameObject player;
    //PlayerHealth playerHealth;
    public Transform[] patrolPoints;
    int patrolPointID;
    NavMeshAgent enemyAgent;
    //DroneAlarm droneAlarm;
    Transform PlayerPosition;
    Animator anim;
    //EnemyHealth enemyHealth;
    //public AudioSource die;
    //public AudioSource shoot;
    public bool alarm;
    bool ativarFear;
	void Start () 
    {
        patrolPointID = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        //playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        enemyAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nextPatrolSpot();
        //droneAlarm = GameObject.FindGameObjectWithTag("Drone").GetComponent<DroneAlarm>();
        //enemyHealth = gameObject.GetComponent<EnemyHealth>();
        alarm = false;
        ativarFear = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //se inimigo esta morto desativa navAgent
     
        //se inimigo esta vivo procura proximo alvo
        if (enemyAgent.enabled == true )
        {
            //se alarme esta desligado vai para o proximo ponto de patrulha
            if (enemyAgent.remainingDistance < 0.5f /*&& !droneAlarm.alarm*/)
            {
                nextPatrolSpot();
            }
            //se alarme esta ligado vai de encontro ao player
            if (alarm)
            {
                goToPlayer();
            }
            else
            {
                anim.SetBool("run", false);
                anim.SetBool("walk", true);
            }
        }

        if (alarm && Vector3.Distance(transform.position, player.transform.position) > 10)
        {
            alarm = false;
        }

	}

    void nextPatrolSpot()
    {
        if (patrolPointID < patrolPoints.Length - 1)
        {
            patrolPointID++;
        }
        else
        {
            patrolPointID = 0;
        }

        enemyAgent.destination = patrolPoints[patrolPointID].position;
    }
    //vai se encontro ao jogador
    void goToPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 1.5f)        {
            enemyAgent.destination = player.transform.position;
            anim.SetBool("walk", false);
            anim.SetBool("run", true);
        }
        else
        {
            
                enemyAgent.destination = transform.position;
                anim.SetBool("run", false);
                anim.SetBool("idle", true);

                StartCoroutine("wait");
            
 
        }
        
       
    }

    IEnumerator wait()
    {
        
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene("test_gameplay");
    }

}
