using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour, ICarMovement
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
    [SerializeField] private float DriftTorque = 25f;
    [SerializeField] private float DriftDrag = 1f;
    [SerializeField] private float AntiDrifDrag = 10f;
    [SerializeField] private float GroundDrag = 2.5f;
    [SerializeField] private float AirDrag = 0f;
    [SerializeField] private float SteerLeanTorque = 15f;

    [Header("Input")]
    [SerializeField] InputAction LongitudinalInput;
    [SerializeField] InputAction LateralInput;
    [SerializeField] InputAction DriftInput;
    
    
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
    private float InternalDrag = 0f;
    private float InternalSteerTorque = 0f;
    private float InternalForwardAcceleration = 0f;
    bool bDrift = false;
    
    
    // Ground Check
    float GroundCheckDistance = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
        LateralInput.Enable();
        LongitudinalInput.Enable();
        DriftInput.Enable();
        CarRigidbody = GetComponent<Rigidbody>();
        SpringDeflectionFrontLeft = 0f;
        SpringDeflectionFrontRight = 0f;
        SpringDeflectionRearLeft = 0f;
        SpringDeflectionRearRight = 0f;
        AverageNormal = Vector3.up;
        GroundCheckDistance = RestLength + 0.35f;
        InternalDrag = AntiDrifDrag;
        InternalSteerTorque = SteerTorque;
        InternalForwardAcceleration = ForwardAcceleration;
    }
    
    // Update is called once per frame
    void Update()
    {
        PerformSuspensionChecks();

        if (DriftInput.ReadValue<float>()>0f)
        {
            bDrift = true;
            
        }
        else
        {
            bDrift = false;
        }

        // Drift Trail
        if (IsGrounded() && bDrift && CarRigidbody.velocity.magnitude > 5f)
        {
            gameObject.GetComponentInChildren<ICarEffects>().EmmitDriftTrail();
        }
        else
        {
            gameObject.GetComponentInChildren<ICarEffects>().StopDriftTrail();
        }


        Debug.Log(InternalForwardAcceleration);
        //Debug.Log(bDrift);
    }

    private void FixedUpdate()
    {
        ApplySuspensionForces();
        AverageNormal = (FrontLeftHit.normal + FrontRightHit.normal + RearLeftHit.normal + RearRightHit.normal) / 4f;

        // Apply Input
        if (!IsFlipped())
        {
            ApplyMovementForces();
        }
        AntiDriftDrag();
        ManageDrag();
    }

    private void ApplyMovementForces()
    {
        
        float LongitudinalInputValue = LongitudinalInput.ReadValue<float>();
        
        Vector3 ProjectedForward = Vector3.ProjectOnPlane(transform.forward, AverageNormal);

        if (IsGrounded())
        {
            if (LongitudinalInputValue > 0f)
            {
                CarRigidbody.AddForceAtPosition(InternalForwardAcceleration * ProjectedForward, AccelerationPoint.transform.position,
                    ForceMode.Acceleration);
            }
            else if (LongitudinalInputValue < 0f)
            {
                CarRigidbody.AddForceAtPosition(-Deceleration * ProjectedForward, BreakingPoint.transform.position,
                    ForceMode.Acceleration);
            }  
        }
  
        
        InternalSteerTorque = bDrift ? DriftTorque : SteerTorque;
        if (IsGrounded())
        {
            float SpeedFactor = Mathf.Clamp01(CarRigidbody.velocity.magnitude / 10f);
           
            if (LateralInput.ReadValue<float>() > 0f)
            {
                CarRigidbody.AddTorque(transform.up * (InternalSteerTorque * Mathf.Sign(LongitudinalInputValue) * SpeedFactor), ForceMode.Acceleration);
                // Body lean torque purely for visual effect
                 CarRigidbody.AddTorque(transform.forward * SteerLeanTorque , ForceMode.Acceleration);
            }
            else if (LateralInput.ReadValue<float>() < 0f)
            {
                CarRigidbody.AddTorque(-transform.up * (InternalSteerTorque * Mathf.Sign(LongitudinalInputValue) * SpeedFactor), ForceMode.Acceleration);
                // Body lean torque purely for visual effect
                CarRigidbody.AddTorque(-transform.forward * SteerLeanTorque , ForceMode.Acceleration);
            }
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
            //Debug.Log(SpringDeflection);
        }
        else
        {
            Hit.normal = Vector3.up;
            SpringDeflection = 0f;
        }
        //Debug.DrawRay(Ray.origin, Ray.direction * RestLength, Color.red);
        
        return SpringDeflection;
    }
    
    void AntiDriftDrag()
    { 
        InternalDrag = bDrift ? DriftDrag : AntiDrifDrag;
       Vector3 ProjectedVelocity = Vector3.ProjectOnPlane(CarRigidbody.velocity, AverageNormal);
       Vector3 SideVelocity = Vector3.Dot(transform.right, ProjectedVelocity) * transform.right;
       CarRigidbody.AddForce(-SideVelocity * InternalDrag, ForceMode.Acceleration);
    }

    void ManageDrag()
    {
        if (IsGrounded())
        {
            CarRigidbody.drag = GroundDrag;
        }
        else
        {
            CarRigidbody.drag = AirDrag;
        }
    }
    
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, GroundCheckDistance, LayerMask.GetMask("Ground"));
    }
    
    public bool IsFlipped()
    {
        return Physics.Raycast(transform.position, transform.up, GroundCheckDistance * 2, LayerMask.GetMask("Ground"));
    }

    void ICarMovement.Dash(float DashImpulse)
    {
        CarRigidbody.AddForce(transform.forward * DashImpulse, ForceMode.Impulse);
        gameObject.GetComponentInChildren<ICarEffects>().EmmitDashParticles();
    }

    void ICarMovement.Spin(float SpinImpulse)
    {
        CarRigidbody.AddTorque(transform.up * SpinImpulse, ForceMode.Impulse);
    }
    
    void ICarMovement.SpeedBoost(float BoostedAcceleration)
    {
        InternalForwardAcceleration = BoostedAcceleration;
        gameObject.GetComponentInChildren<ICarEffects>().EmmitSpeedBoostParticles();
    }
    
    void ICarMovement.EndSpeedBoost()
    {
        InternalForwardAcceleration = ForwardAcceleration;
        gameObject.GetComponentInChildren<ICarEffects>().StopSpeedBoostParticles();
    }
    
    float ICarMovement.GetSpeed()
    {
        return CarRigidbody.velocity.magnitude;
    }
}
