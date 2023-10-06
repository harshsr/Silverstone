using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarMovement : MonoBehaviour, IEnemyCarMovement
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
    [SerializeField] private float GroundDrag = 2.5f;
    [SerializeField] private float AirDrag = 0f;
    [SerializeField] private float SteerLeanTorque = 15f;
    [SerializeField] private float AntiDrifDrag = 10f;

    private float SpringDeflectionFrontLeft = 0f;
    private float SpringDeflectionFrontRight = 0f;
    private float SpringDeflectionRearLeft = 0f;
    private float SpringDeflectionRearRight = 0f;
    
    private RaycastHit FrontLeftHit;
    private RaycastHit FrontRightHit;
    private RaycastHit RearLeftHit;
    private RaycastHit RearRightHit;
    
    private Rigidbody CarRigidbody;
    public Vector3 AverageNormal;
    float GroundCheckDistance = 1f;
    
    Vector3 NextWaypointPosition;
    AIPathManager PathManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        CarRigidbody = gameObject.GetComponent<Rigidbody>();
        AverageNormal = Vector3.up;
        PathManager = GameObject.FindWithTag("AIPath").GetComponent<AIPathManager>();
        GroundCheckDistance = RestLength + 0.35f;
        NextWaypointPosition = PathManager.GetCurrentWaypointPosition();
    }

    // Update is called once per frame
    void Update()
    {
        PerformSuspensionChecks();
    }

    private void FixedUpdate()
    {
        ApplySuspensionForces();
        AverageNormal = (FrontLeftHit.normal + FrontRightHit.normal + RearLeftHit.normal + RearRightHit.normal) / 4;
        ApplyMovementForces();
        //AntiDriftDrag();
        //ManageDrag();
    }

    void PerformSuspensionChecks()
    {
        SpringDeflectionFrontLeft = CalculateSpringDeflection(new Ray(SuspensionFrontLeft.transform.position, -SuspensionFrontLeft.transform.up), ref FrontLeftHit);
        SpringDeflectionFrontRight = CalculateSpringDeflection(new Ray(SuspensionFrontRight.transform.position, -SuspensionFrontRight.transform.up), ref FrontRightHit);
        SpringDeflectionRearLeft = CalculateSpringDeflection(new Ray(SuspensionRearLeft.transform.position, -SuspensionRearLeft.transform.up), ref RearLeftHit);
        SpringDeflectionRearRight = CalculateSpringDeflection(new Ray(SuspensionRearRight.transform.position, -SuspensionRearRight.transform.up), ref RearRightHit);
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
    
    private void ApplySuspensionForces()
    {
        CarRigidbody.AddForceAtPosition(SpringDeflectionFrontLeft * SpringCoefficient * SuspensionFrontLeft.transform.up, SuspensionFrontLeft.transform.position);
        CarRigidbody.AddForceAtPosition(SpringDeflectionFrontRight * SpringCoefficient * SuspensionFrontRight.transform.up, SuspensionFrontRight.transform.position);
        CarRigidbody.AddForceAtPosition(SpringDeflectionRearLeft * SpringCoefficient * SuspensionRearLeft.transform.up, SuspensionRearLeft.transform.position);
        CarRigidbody.AddForceAtPosition(SpringDeflectionRearRight * SpringCoefficient * SuspensionRearRight.transform.up, SuspensionRearRight.transform.position);
    }
    
    void AntiDriftDrag()
    { 
       Vector3 ProjectedVelocity = Vector3.ProjectOnPlane(CarRigidbody.velocity, AverageNormal);
       Vector3 SideVelocity = Vector3.Dot(transform.right, ProjectedVelocity) * transform.right;
       CarRigidbody.AddForce(-SideVelocity * AntiDrifDrag, ForceMode.Acceleration);
    }

    void ApplyMovementForces()
    {
        if (Vector3.Distance(transform.position, NextWaypointPosition) < 5f)
        {
            NextWaypointPosition = PathManager.GetNextWaypointPosition();
        }
        Vector3 Direction = (NextWaypointPosition - transform.position).normalized;
        CarRigidbody.AddForceAtPosition(Direction * ForwardAcceleration, AccelerationPoint.transform.position);
        
        transform.forward = Vector3.Lerp(transform.forward, Direction, 0.1f);
    }
    
    void ManageDrag()
    {
        CarRigidbody.drag = IsGrounded() ? GroundDrag : AirDrag;
    }
    
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, GroundCheckDistance, LayerMask.GetMask("Ground"));
    }
    
    public Vector3 GetAverageNormal()
    {
        return AverageNormal;
    }
}
