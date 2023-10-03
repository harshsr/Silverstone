using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] GameObject CarOne;
    [SerializeField] GameObject CarTwo;
    [SerializeField] float Distance = 10f;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Distance = Vector3.Distance(CarOne.transform.position, CarTwo.transform.position);
        Distance = Distance +15f;
        Distance = Mathf.Clamp(Distance, 10f, 150f);
        transform.position = (CarOne.transform.position + CarTwo.transform.position) / 2 + Vector3.up * Distance + Vector3.forward * -Distance/2;
        
        transform.LookAt((CarOne.transform.position + CarTwo.transform.position) / 2);
    }

    private void FixedUpdate()
    {
        
    }
}
