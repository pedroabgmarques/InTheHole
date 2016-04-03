using UnityEngine;
using System.Collections;

public class DialogueNPC : MonoBehaviour {

    private bool istrigered, response;

    public GameObject Background, NPCText;

    float timer;


 

    //1= player 2 = npc
    public Transform object1, object2;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        

        float distance = Vector3.Distance(object1.position, object2.position);

        if (distance < 3.0f)
        {
            timer += Time.deltaTime;
            istrigered = true;
            if(timer>5f)
            {
                NPCText.GetComponent<UnityEngine.UI.Text>().text = "Cuidado com os soldados!\n Usa os destroços para te esconderes";
            }
        }
        else
        {
            istrigered = false;
            response = false;

            Background.SetActive(false);

            NPCText.SetActive(false);

        }
    }

    public void OnGUI()
    {
        if (istrigered)
        {





                response = true;


            if (response)
            {

                Background.SetActive(true);

                NPCText.SetActive(true);


            }

        }




    }
}
