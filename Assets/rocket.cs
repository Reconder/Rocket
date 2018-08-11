using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour {


    Rigidbody rigidBody;
    AudioSource audioSource;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
		
	}

    private void ProcessInput()
    {
   

        if ((Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.D)))
        {
            transform.Rotate(Vector3.forward * Time.deltaTime*50);
        }
        if ((Input.GetKey(KeyCode.D)) && (!Input.GetKey(KeyCode.A)))
            {
            transform.Rotate(-Vector3.forward * Time.deltaTime*50);
        }

        
        if (Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!audioSource.isPlaying)
            {
                audioSource.Play(100);
            }
        }
        if ((!Input.GetKey(KeyCode.Space))&&((audioSource.isPlaying)))
        {
            audioSource.Stop();
        }
    }
}
