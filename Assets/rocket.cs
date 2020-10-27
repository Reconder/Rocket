using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocket : MonoBehaviour {


    Rigidbody rigidBody;
    AudioSource audioSource;
    Transform transform;
    [SerializeField] float rcsRotation = 200f;
    [SerializeField] public float rcsThrust = 25f;
    [SerializeField] float Death_delay = 1f;
    [SerializeField] float nextLevel_delay = 2f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathExplosion;
    [SerializeField] AudioClip victory;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathExplosionParticles;
    [SerializeField] ParticleSystem victoryParticles;

    bool collisionDisabled = false;
    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        transform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
            Thrust();
            Rotate(); //use BetterRotate() to rotate with torques
        }
        if (Debug.isDebugBuild)
        { RespondToDebugKeys(); }
        
        //BetterRotate();

    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        { LoadNextLevel(); }
        if (Input.GetKeyDown(KeyCode.C))
        { collisionDisabled = !collisionDisabled; }
    }

    //Rotate with torques... feels awkward to control
    private void BetterRotate()
    {


        float rotationSpeed = Time.deltaTime * rcsRotation;
        if ((Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.D)))
        {
            rigidBody.AddRelativeTorque(Vector3.forward * rotationSpeed);
        }
        if ((Input.GetKey(KeyCode.D)) && (!Input.GetKey(KeyCode.A)))
        {
            rigidBody.AddRelativeTorque(-Vector3.forward * rotationSpeed);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionDisabled) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                // do nothing
                Victory();
                break;
            case "Fuel":
                //add fuel
                print("Fuel");
                break;
            case "Last Finish":
                LastVictory();
                
                break;
            default:
                //kill the player
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
