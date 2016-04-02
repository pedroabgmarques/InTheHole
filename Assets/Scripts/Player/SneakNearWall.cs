using UnityEngine;
using System.Collections;

public class SneakNearWall : MonoBehaviour {

    GameObject player;
    PlayerBehaviourScript playerController;

	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerBehaviourScript>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject == player)
    //    {
            
    //        playerController.isCrouch = true;
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject == player)
    //    {
            
    //        playerController.isCrouch = false;
    //    }
    //}
}
