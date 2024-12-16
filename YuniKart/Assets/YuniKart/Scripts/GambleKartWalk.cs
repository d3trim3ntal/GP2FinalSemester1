using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambleKartWalk : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;
    public Animator animator;
    public ParticleSystem smoke;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxis("Vertical");
        float rotationInput = Input.GetAxis("Horizontal");

        bool isMoving = Mathf.Abs(moveInput) > 0 || Mathf.Abs(rotationInput) > 0;

        animator.SetBool("isWalking", isMoving);

        if (isMoving && !smoke.isPlaying)
        {
            smoke.Play();
        }
        else if (!isMoving && smoke.isPlaying)
        {
            smoke.Stop();
        }

        Vector3 move = transform.forward * moveInput * moveSpeed * Time.deltaTime;
        float rotation = rotationInput * rotationSpeed * Time.deltaTime;

        rb.MovePosition(rb.position + move);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation, 0));

    }

}
