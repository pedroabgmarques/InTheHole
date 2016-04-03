using UnityEngine;
using System.Collections;

public class CameraControllerCena3 : MonoBehaviour {

    public GameObject player;
    bool cameraFollow;

    int cameraWidth, cameraHeight;
    float speed;
    float posicaoX;
    float posicaoY;
	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        cameraFollow = true;
        cameraWidth = Screen.width;
        cameraHeight = Screen.height;
        speed = 6;
        
        //transform.position = player.transform.position + new Vector3(0, 16, -15);//0, 16, -15
        posicaoY = -15f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            cameraFollow = !cameraFollow;
        }
        if(cameraFollow)
        {

            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), 0.8f * Time.deltaTime);

            //transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0, 16, -15), 0.8f * Time.deltaTime);
            posicaoX = transform.position.x;
            posicaoY = transform.position.z;
        }
        else
        {
            if(Input.mousePosition.x > cameraWidth - 5)
            {
                posicaoX += speed * Time.deltaTime;
                transform.position = new Vector3(posicaoX, transform.position.y, transform.position.z);
            }
            if (Input.mousePosition.x < 0 + 5)
            {
                posicaoX -= speed * Time.deltaTime;
                transform.position = new Vector3(posicaoX, transform.position.y, transform.position.z);
            }
            if (Input.mousePosition.y > cameraHeight - 5)
            {
                posicaoY += speed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, transform.position.y, posicaoY);
            }
            if (Input.mousePosition.y < 0 + 5)
            {
                posicaoY -= speed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, transform.position.y, posicaoY);
            }
        }
        
	}
}
