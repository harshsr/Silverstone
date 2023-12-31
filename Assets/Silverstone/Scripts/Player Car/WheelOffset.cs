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
            if (Car.GetComponent<IEnemyCarMovement>().IsGrounded())
            {
                UpVector = Car.GetComponent<IEnemyCarMovement>().GetAverageNormal();
            }
            else
            {
                UpVector = Car.transform.up;
            }
            transform.position = Car.transform.position - Car.transform.up * Offset.magnitude;
            transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.FromToRotation(transform.up, UpVector) * transform.rotation, 0.1f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Car.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
}
