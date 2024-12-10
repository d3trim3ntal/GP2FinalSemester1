using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public List<Wheel> wheels;

    float moveInput;

    private Rigidbody carRb;

    void LateUpdate()
    {
        Move();
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * maxAcceleration * Time.deltaTime;
        }
    }

}
