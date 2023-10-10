using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] GameObject CarOne;
    [SerializeField] GameObject CarTwo;
    [SerializeField] float Distance = 10f;
    
    Vector3 CameraPosition;
    Vector3 LookAtPosition;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CarOne && CarTwo)
        {
            Distance = Vector3.Distance(CarOne.transform.position, CarTwo.transform.position);
            Distance += 15f;
            Distance = Mathf.Clamp(Distance, 10f, 175f);
            CameraPosition = (CarOne.transform.position + CarTwo.transform.position) / 2 + Vector3.up * Distance + Vector3.forward * -0/4;
            LookAtPosition = (CarOne.transform.position + CarTwo.transform.position) / 2;
        }
        else if (CarOne && !CarTwo)
        {
            CameraPosition = CarOne.transform.position + Vector3.up * Distance + Vector3.forward * -Distance*1.5f;
            LookAtPosition = CarOne.transform.position;
        }
        else if (CarTwo && !CarOne)
        {
            CameraPosition = CarTwo.transform.position + Vector3.up * Distance + Vector3.forward * -Distance*1.5f;
            LookAtPosition = CarTwo.transform.position;
        }
        else
        {
            CameraPosition = Vector3.zero;
            LookAtPosition = Vector3.zero;
        }
        transform.position = Vector3.Lerp(transform.position, CameraPosition, 1f);
        
        transform.LookAt(LookAtPosition);
    }

    private void FixedUpdate()
    {
        
    }
}
