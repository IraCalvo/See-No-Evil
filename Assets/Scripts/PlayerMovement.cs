using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private Vector2 playerMovement;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void OnMove(InputValue value)
    {
        playerMovement = value.Get<Vector2>();
    }

    void MovePlayer()
    {
        rb.velocity = new Vector3(playerMovement.x * moveSpeed, rb.velocity.y, playerMovement.y * moveSpeed);
    }
}
