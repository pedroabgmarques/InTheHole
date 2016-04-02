using UnityEngine;
using System.Collections;

public class HideSpotPlacement : MonoBehaviour {

    Ray ray;
    RaycastHit rayHit;

    PlayerBehaviourScript playerController;

	void Start () 
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviourScript>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayHit, 200))
        {
            if (rayHit.collider.gameObject.tag == "hidingWall" && rayHit.normal.y > 0 && rayHit.normal.x == 0 && rayHit.normal.z == 0)
           {
               gameObject.GetComponent<Renderer>().enabled = true;
               gameObject.transform.position = rayHit.point - rayHit.normal * 0.5f;
           }
           else if(rayHit.collider.gameObject.tag == "Jump" && rayHit.normal.y > 0 && rayHit.normal.x == 0 && rayHit.normal.z == 0)
            {
                gameObject.GetComponent<Renderer>().enabled = true;
                gameObject.transform.position = rayHit.point + rayHit.normal * 0.5f;
            }
           else
           {
               gameObject.GetComponent<Renderer>().enabled = false;
           }
        }
      
	}
}
