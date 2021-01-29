using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;

    Vector3 velocity;
    float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMaskl;
    
    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        var move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        //velocity.y += gravity * Time.deltaTime;

        //controller.Move(velocity * Time.deltaTime);
    }
}
