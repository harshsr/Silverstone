using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    [Header("Suspension Config")]
    [SerializeField] GameObject SuspensionFrontLeft;
    [SerializeField] GameObject SuspensionFrontRight;
    [SerializeField] GameObject SuspensionRearLeft;
    [SerializeField] GameObject SuspensionRearRight;
    [SerializeField] private float SpringCoefficient = 10f;
    [SerializeField] private float DampingCoefficient = 1f;
    [SerializeField] private float RestLength = 0.25f;
    
    [Header("Movement Config")]
    [SerializeField] GameObject AccelerationPoint;
    [SerializeField] GameObject BreakingPoint;
    [SerializeField] private float ForwardAcceleration = 10f;
    [SerializeField] private float Deceleration = 5f;
    [SerializeField] private float SteerTorque = 10f;
    
    [Header("Input")]
    [SerializeField] InputAction LongitudinalInput;
    [SerializeField] InputAction LateralInput;
    
    
    // Spring Suspension
    private float SpringDeflectionFrontLeft;
    private float SpringDeflectionFrontRight;
    private float SpringDeflectionRearLeft;
    private float SpringDeflectionRearRight;
    
    private RaycastHit FrontLeftHit;
    private RaycastHit FrontRightHit;
    private RaycastHit RearLeftHit;
    private RaycastHit RearRightHit;
    
    private Rigidbody CarRigidbody;
    public Vector3 AverageNormal;
    // Start is called before the first frame update
    void Start()
    {
        LateralInput.Enable();
        LongitudinalInput.Enable();
        CarRigidbody = GetComponent<Rigidbody>();
        SpringDeflectionFrontLeft = 0f;
        SpringDeflectionFrontRight = 0f;
        SpringDeflectionRearLeft = 0f;
        SpringDeflectionRearRight = 0f;
        AverageNormal = Vector3.up;
    }
    
    // Update is called once per frame
    void Update()
    {
        PerformSuspensionChecks();
        //Debug.Log(SpringDeflectionFrontLeft);
        
        AverageNormal = (FrontLeftHit.normal + FrontRightHit.normal + RearLeftHit.normal + RearRightHit.normal) / 4f;
        
    }
    
    private void FixedUpdate()
    {
        ApplySuspensionForces();
        
        // Apply Input
        Vector3 ProjectedForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        if (LongitudinalInput.ReadValue<float>() > 0f)
        {
            CarRigidbody.AddForceAtPosition(ForwardAcceleration * ProjectedForward, AccelerationPoint.transform.position, ForceMode.Acceleration);
        }
        else if (LongitudinalInput.ReadValue<float>() < 0f)
        {
            CarRigidbody.AddForceAtPosition(-Deceleration * ProjectedForward, BreakingPoint.transform.position, ForceMode.Acceleration);
        }
        
        if (LateralInput.ReadValue<float>() > 0f)
        {
            CarRigidbody.AddTorque(transform.up * SteerTorque, ForceMode.Acceleration);
        }
        else if (LateralInput.ReadValue<float>() < 0f)
        {
            CarRigidbody.AddTorque(-transform.up * SteerTorque, ForceMode.Acceleration);
        }
        
    }

    void PerformSuspensionChecks()
    {
        SpringDeflectionFrontLeft = CalculateSpringDeflection(new Ray(SuspensionFrontLeft.transform.position, -SuspensionFrontLeft.transform.up), ref FrontLeftHit);
        SpringDeflectionFrontRight = CalculateSpringDeflection(new Ray(SuspensionFrontRight.transform.position, -SuspensionFrontRight.transform.up), ref FrontRightHit);
        SpringDeflectionRearLeft = CalculateSpringDeflection(new Ray(SuspensionRearLeft.transform.position, -SuspensionRearLeft.transform.up), ref RearLeftHit);
        SpringDeflectionRearRight = CalculateSpringDeflection(new Ray(SuspensionRearRight.transform.position, -SuspensionRearRight.transform.up), ref RearRightHit);
    }

    private void ApplySuspensionForces()
    {
        CarRigidbody.AddForceAtPosition(SpringDeflectionFrontLeft * SpringCoefficient * SuspensionFrontLeft.transform.up, SuspensionFrontLeft.transform.position);
        CarRigidbody.AddForceAtPosition(SpringDeflectionFrontRight * SpringCoefficient * SuspensionFrontRight.transform.up, SuspensionFrontRight.transform.position);
        CarRigidbody.AddForceAtPosition(SpringDeflectionRearLeft * SpringCoefficient * SuspensionRearLeft.transform.up, SuspensionRearLeft.transform.position);
        CarRigidbody.AddForceAtPosition(SpringDeflectionRearRight * SpringCoefficient * SuspensionRearRight.transform.up, SuspensionRearRight.transform.position);
    }
    
    float CalculateSpringDeflection(Ray Ray, ref RaycastHit Hit)
    {
        float SpringDeflection = 0f;
        if (Physics.Raycast(Ray, out Hit, RestLength, LayerMask.GetMask("Ground")))
        {
            SpringDeflection = (RestLength - Hit.distance) / RestLength;
            //Debug.Log(Hit.distance);
        }
        else
        {
            SpringDeflection = 0f;
        }
        Debug.DrawRay(Ray.origin, Ray.direction * RestLength, Color.red);
        
        return SpringDeflection;
        
    }
}
