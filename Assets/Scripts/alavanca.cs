using UnityEngine;
using System.Collections;

public class alavanca : MonoBehaviour {

    Animation anim;
    public ParticleSystem particle;
    Camera cam;
    public Transform target;
    GameObject player;
    CameraControllerCena3 camController;
    GameObject predios;
    GameObject entulhos;
    GameObject ObstaculosEsconder;
    GameObject obstaculos;
    GameObject ObstaculosTapados;
    GameObject candeeiros;
    GameObject estradas;
    Rigidbody rb;
    
	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animation>();
        //particle = GetComponentInParent<ParticleSystem>();
        anim.Stop();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        camController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControllerCena3>();
        predios = GameObject.Find("predios");
        entulhos = GameObject.Find("entulhos");
        ObstaculosEsconder = GameObject.Find("ObstaculosEsconder");
        obstaculos = GameObject.Find("obstaculos");
        ObstaculosTapados = GameObject.Find("ObstaculosTapados");
        candeeiros = GameObject.Find("candeeiros");
        estradas = GameObject.Find("estradas");
        rb = player.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void OnTriggerEnter()
    {
        anim.Play();
        particle.Play();

        //cam.transform.Rotate(cam.transform.TransformDirection(cam.transform.up), -45);
        camController.enabled = false;
        cam.transform.LookAt(target.position);
        cam.transform.position = player.transform.position + new Vector3(10,3, -10);

        //StartCoroutine("destroyScene");

        foreach (Transform child in predios.transform)
        {
            Destroy(child.gameObject,Random.Range(0.1f, 5f));
            
        }
        foreach (Transform child in entulhos.transform)
        {
            Destroy(child.gameObject, Random.Range(0.1f, 5f));

        }
        foreach (Transform child in ObstaculosEsconder.transform)
        {
            Destroy(child.gameObject, Random.Range(0.1f, 5f));

        }
        foreach (Transform child in obstaculos.transform)
        {
            Destroy(child.gameObject, Random.Range(0.1f, 5f));

        }
        foreach (Transform child in ObstaculosTapados.transform)
        {
            Destroy(child.gameObject, Random.Range(0.1f, 5f));

        }
        foreach (Transform child in candeeiros.transform)
        {
            Destroy(child.gameObject, Random.Range(0.1f, 5f));

        }
        foreach (Transform child in estradas.transform)
        {
            Destroy(child.gameObject, Random.Range(5f, 5.5f));

        }
        
        rb.isKinematic = false;
        Destroy(player.GetComponent<NavMeshAgent>(), 6f);


        Destroy(GameObject.FindGameObjectsWithTag("Enemy")[0]);
        Destroy(GameObject.FindGameObjectsWithTag("Enemy")[1]);
        Destroy(gameObject, 5f);
        Destroy(gameObject.transform.parent.gameObject, 5f);
    }

    IEnumerator destroyScene()
    {
        foreach (Transform child in predios.transform)
        {
            Destroy(child.gameObject);
            yield return new WaitForSeconds(1);
            
            
        }
       

    }
}
