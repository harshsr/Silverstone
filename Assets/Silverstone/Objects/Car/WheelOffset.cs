using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelOffset : MonoBehaviour
{
    // Start is called before the first frame update
    
    Vector3 Offset;
    Vector3 UpVector;
    [SerializeField]
    GameObject Car;

    private void Awake()
    {
        
    }

    void Start()
    {
        if (Car)
        {
            Offset = transform.position - Car.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Car)
        {
            UpVector = Car.GetComponent<CarMovement>().AverageNormal;
            transform.position = Car.transform.position + Offset;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, UpVector);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Car.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
}
