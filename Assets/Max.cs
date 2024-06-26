using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Max : MonoBehaviour
{
    // References to other components
    public Animator playerAnim;
    public Rigidbody playerRigid;
    public Transform playerTrans;

    // Movement speed variables
    public float w_speed = 5f; // Walking speed
    public float rn_speed = 5f; // Running speed
    public float ro_speed = 100f; // Rotation speed

    // State tracking
    private bool isWalking = false; // Tracking if the player is currently walking

    void FixedUpdate()
    {
        // Handle forward movement (W key)
        if (Input.GetKey(KeyCode.W))
        {
            playerRigid.velocity = transform.forward * w_speed;
        }
        // Handle backward movement (S key)
        if (Input.GetKey(KeyCode.S))
        {
            playerRigid.velocity = -transform.forward * w_speed;
        }
    }

    void Update()
    {
        // Handle walking forward (W key)
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnim.SetTrigger("walk");
            isWalking = true; // Update walking state
        }
        // Handle stopping walking (W key)
        if (Input.GetKeyUp(KeyCode.W))
        {
            playerAnim.ResetTrigger("walk");
            playerAnim.SetTrigger("idle");
            isWalking = false; // Update walking state
        }
        // Handle walking backward (S key)
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerAnim.SetTrigger("jogBack");
            isWalking = false; // Update walking state
        }
        // Handle stopping walking backward (S key)
        if (Input.GetKeyUp(KeyCode.S))
        {
            playerAnim.ResetTrigger("jogBack");
            playerAnim.SetTrigger("idle");
        }
        // Handle rotation left (A key)
        if (Input.GetKey(KeyCode.A))
        {
            playerTrans.Rotate(0, -ro_speed * Time.deltaTime, 0);
        }
        // Handle rotation right (D key)
        if (Input.GetKey(KeyCode.D))
        {
            playerTrans.Rotate(0, ro_speed * Time.deltaTime, 0);
        }
        // Handle running while walking (Left Shift key)
        if (isWalking && Input.GetKeyDown(KeyCode.LeftShift))
        {
            w_speed += rn_speed; // Increase walking speed for running
            playerAnim.SetTrigger("run");
            playerAnim.ResetTrigger("walk");
        }
        // Handle stopping running (Left Shift key)
        if (isWalking && Input.GetKeyUp(KeyCode.LeftShift))
        {
            w_speed -= rn_speed; // Decrease speed back to original walking speed
            playerAnim.ResetTrigger("run");
            playerAnim.SetTrigger("walk");
        }
    }
}