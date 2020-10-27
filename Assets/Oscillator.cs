using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementRange = new Vector3(12f, 0, 0);
    [SerializeField] float period = 4f;
    [SerializeField] float phase = 0f;

    //[Range(0,1)] [SerializeField] float movementFactor;

    Vector3 startingPos;


    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period<=Mathf.Epsilon) { return; }
        float cycles = Time.time / period + phase;
        
        Vector3 offset = movementRange * Mathf.Sin(cycles*Mathf.PI*2f);
        transform.position = startingPos + offset;
    }
}
