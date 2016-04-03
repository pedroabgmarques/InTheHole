using UnityEngine;
using System.Collections;

public class PlayerBehaviourScript : MonoBehaviour {

    //NAVIGATION
    NavMeshAgent agent;

    Transform targetPosition;
    Ray ray;
    RaycastHit newTargetPosition;
    RaycastHit rayHit;
    Vector3 targetRotation;
    Rigidbody playerRigidBody;
    //JumpOverObstacle jumpEnabled;
    bool enemyTarget;

    GameObject enemy;
    //ANIMATION
    public Animator anim;
    public bool isMoving;
    public bool isCrouch;
    public float RunSpeed = 6;
    public float CrouchSpeed = 3;
    float playerSpeed;
    public bool jump;
    float animationEndTimer=0;
    float numberOfJumps = 0;
	void Awake () 
    {
        anim = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
        isMoving = false;
        isCrouch = false;
        jump = false;
        enemyTarget = false;
        playerSpeed = gameObject.GetComponent<NavMeshAgent>().speed;
        //jumpEnabled = gameObject.GetComponent<JumpOverObstacle>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        
        //PLAYER STANCE----------------------------------------------------
        setStance();
        setMovementAnimation();
        //VERIFICAR SE EXISTE ALVO A ABATER
        if(enemyTarget)
        {
            agent.destination = enemy.transform.position;
            if(agent.remainingDistance < 1f)
            {
                //agent.transform.LookAt(enemy.transform.position);
                enemyTarget = false;
                agent.ResetPath();
                anim.SetTrigger("SneakKill");
                //anim.SetBool("Walk", false);
                   
                
            }
        }
        //PLAYER MOVEMENT-------------------------------------------------
        //se pressionar botao direito do rato calcula nova posicao
        InputManager();
        //quando chega a nova posicao, para
        stopOnPosition();

             
	}

    void setTargetPosition()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out newTargetPosition, 200))
        {
            if (newTargetPosition.transform.tag == "Enemy")
            {
                enemyTarget = true;
                enemy = newTargetPosition.transform.gameObject;
                agent.destination = newTargetPosition.transform.position;
                

            }
            else
            {
                enemyTarget = false;
                agent.stoppingDistance = 0f;
                agent.destination = newTargetPosition.point;
            }

            if (newTargetPosition.transform.tag != "hidingWall")
            {
                isCrouch = false;
                //jumpEnabled.jumpEnabled = true;
            }
            else if (newTargetPosition.transform.tag == "hidingWall")
            {
                isCrouch = true;
                //jumpEnabled.jumpEnabled = false;
            }
        }

    }

    void setStance()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouch = !isCrouch;
        }
    }

    void setMovementAnimation()
    {
        if (isCrouch && !isMoving)
        {
            anim.SetBool("IdleCrouch", true);
            anim.SetBool("Idle", false);
            anim.SetBool("Walk", false);
            anim.SetBool("WalkCrouch", false);
            //jumpEnabled.jumpEnabled = false;

        }
        if (isCrouch && isMoving)
        {
            playerSpeed = CrouchSpeed;
            anim.SetBool("IdleCrouch", false);
            anim.SetBool("Walk", false);
            anim.SetBool("WalkCrouch", true);
            anim.SetBool("Idle", false);
            //jumpEnabled.jumpEnabled = false;
        }
        if (!isCrouch && !isMoving)
        {
            anim.SetBool("IdleCrouch", false);
            anim.SetBool("Idle", true);
            anim.SetBool("Walk", false);
            anim.SetBool("WalkCrouch", false);
            //jumpEnabled.jumpEnabled = true;
        }
        if (!isCrouch && isMoving)
        {
            playerSpeed = RunSpeed;
            anim.SetBool("Walk", true);
            anim.SetBool("WalkCrouch", false);
            anim.SetBool("IdleCrouch", false);
            anim.SetBool("Idle", false);
            //jumpEnabled.jumpEnabled = true;
        }
    }

    void InputManager()
    {
        if (Input.GetMouseButton(0))
        {

            StartCoroutine("animationControl");
            //setTargetPosition();

           

            //if (isCrouch)
            //{
            //    anim.SetBool("Walk", false);
            //    anim.SetBool("WalkCrouch", true);

            //}
            //if (!isCrouch)
            //{
            //    anim.SetBool("Walk", true);
            //    anim.SetBool("WalkCrouch", false);
            //}

            //isMoving = true;
        }
        
    }

    void stopOnPosition()
    {
        if (agent.remainingDistance < 0.2f)//antes 0.2f
        {
            anim.SetBool("Walk", false);
            anim.SetBool("WalkCrouch", false);
            if (isCrouch)
            {
                anim.SetBool("IdleCrouch", true);
                anim.SetBool("Idle", false);
            }
            else
            {
                anim.SetBool("Idle", true);
                anim.SetBool("IdleCrouch", false);
            }
            isMoving = false;
        }
        //define velocidade do movimento
        agent.speed = playerSpeed;
    }

    IEnumerator animationControl()
    {
        setTargetPosition();



        if (isCrouch)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("WalkCrouch", true);

        }
        if (!isCrouch)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("WalkCrouch", false);
        }

        isMoving = true;

        yield return null;
    }

   
}
