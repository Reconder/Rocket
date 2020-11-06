using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocket : MonoBehaviour {


    Rigidbody rigidBody;
    AudioSource audioSource;
    Transform transform;
    float rcsRotation = 200f;
    public float rcsThrust = 25f;
    float Death_delay = 1f;
    float nextLevel_delay = 2f;
    public AudioClip mainEngine;
    public AudioClip deathExplosion;
    public AudioClip victory;

    public ParticleSystem mainEngineParticles;
    public ParticleSystem deathExplosionParticles;
    public ParticleSystem victoryParticles;

    bool collisionDisabled = false;
    
    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    private bool isNotAlive => state != State.Alive;

    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        transform = GetComponent<Transform>();
    }
	
	void Update () {
        if (state == State.Alive)
        {
            Thrust();
            Rotate(); 
        }
        if (Debug.isDebugBuild)
        { RespondToDebugKeys(); }

    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        { LoadNextLevel(); }
        if (Input.GetKeyDown(KeyCode.C))
        { collisionDisabled = !collisionDisabled; }
    }


    void OnCollisionEnter(Collision collision)
    {
        
        if (isNotAlive || collisionDisabled) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                Victory();
                break;
            case "Last Finish":
                LastVictory();
                break;
            default:
                Dying();
                break;




        }
    }

    private void LastVictory()
    {
        state = State.Transcending;
        audioSource.Stop();
        mainEngineParticles.Stop();
        audioSource.PlayOneShot(victory);
        victoryParticles.Play();
        Invoke("Death", nextLevel_delay);
    }



    private void Dying()
    {
        state = State.Dying;
        audioSource.Stop();
        mainEngineParticles.Stop();
        audioSource.PlayOneShot(deathExplosion);
        deathExplosionParticles.Play();
        Invoke("Death", Death_delay);
        
    }

    private void Victory()
    {
        state = State.Transcending;
        audioSource.Stop();
        mainEngineParticles.Stop();
        audioSource.PlayOneShot(victory);
        victoryParticles.Play();
        Invoke("LoadNextLevel", nextLevel_delay);
        
    }

    private void LoadNextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == SceneManager.sceneCountInBuildSettings ) { index = -1; }
        SceneManager.LoadScene(index + 1);
    }

    private void Death()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Rotate()
    {
        
        
        float rotationSpeed = Time.deltaTime * rcsRotation;
        if ((Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.D)))
        {
            rigidBody.freezeRotation = true;  // take manual control of the rotation
            transform.Rotate(Vector3.forward * rotationSpeed);
            rigidBody.freezeRotation = false;  // release control of the rotation
        }
        if ((Input.GetKey(KeyCode.D)) && (!Input.GetKey(KeyCode.A)))
        {
            rigidBody.freezeRotation = true;  // take manual control of the rotation
            transform.Rotate(-Vector3.forward * rotationSpeed);
            rigidBody.freezeRotation = false;  // release control of the rotation
        }

        
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * rcsThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }
    }
    public void StartAgain()
    {
        SceneManager.LoadScene("Level 1");
    }
}
