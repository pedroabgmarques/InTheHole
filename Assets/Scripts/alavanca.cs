using UnityEngine;
using System.Collections;

public class alavanca : MonoBehaviour {

    Animation anim;
    public ParticleSystem particle;
	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animation>();
        //particle = GetComponentInParent<ParticleSystem>();
        anim.Stop();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void OnTriggerEnter()
    {
        anim.Play();
        particle.Play();
    }
}
