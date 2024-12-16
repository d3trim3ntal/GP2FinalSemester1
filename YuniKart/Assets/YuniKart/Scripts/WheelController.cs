using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    //Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    //Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    public float rotationSpeed = 1f;

    [SerializeField] private GameObject rearRightMarks, rearLeftMarks;

    public ParticleSystem colpart, powpart, smokepart;


    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        WheelEffects();
        Application();
    }

    private void GetInput()
    {
        //Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        //Acceleration Input
        verticalInput = Input .GetAxis("Vertical");

        //Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        smokepart.Play();
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void Application()
    {
        if (isBreaking)
        {
            ApplyBreaking();
        }
        else
        {
            HandleMotor();
        }
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    void WheelEffects()
    {
       if(Input.GetKey(KeyCode.Space))
       {
           rearLeftMarks.GetComponent<TrailRenderer>().emitting = true;
           rearRightMarks.GetComponent<TrailRenderer>().emitting = true;
       }
       else
       {
           rearLeftMarks.GetComponent<TrailRenderer>().emitting = false;
           rearRightMarks.GetComponent<TrailRenderer>().emitting = false;
       }
    }

    private void OnCollisionEnter(Collision collision) 
    { // Play collision particles and sound when a collision occurs 
        if (colpart != null)  // Instantiate the collision particles at the collision point 
        {   
            Debug.Log("Has collided with object");
            ParticleSystem instantiatedColPart = Instantiate(colpart, collision.contacts[0].point, Quaternion.identity); 
            instantiatedColPart.Play(); // Destroy the particle system after its duration to avoid clutter 
            Destroy(instantiatedColPart.gameObject, instantiatedColPart.main.duration); 
        }
    }
}