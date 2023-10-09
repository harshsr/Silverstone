using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCollisions : MonoBehaviour
{
    
    private bool bFellOff = false;
    private float FellOffTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bFellOff)
        {
            FellOffTimer += Time.deltaTime;
            if (FellOffTimer > 0.5f)
            { 
                FellOffTimer = 0f;
                bFellOff = false;
                gameObject.GetComponent<ICarGameState>().ResetToLastCheckpoint();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            bFellOff = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //gameObject.GetComponent<Rigidbody>().AddForce(other.GetContact(0).normal* other.gameObject.GetComponent<Rigidbody>().velocity.magnitude, ForceMode.Impulse);
        }
    }
}
