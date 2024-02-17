using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float moveSpeed;
    [SerializeField] float lookSensitivity;

    public bool eyesOpen = true;
    private Vector2 playerMovement;
    Vector2 mouseDelta;
    Rigidbody rb;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MovePlayer();
        RotateCamera();
    }

    void OnMove(InputValue value)
    {
        playerMovement = value.Get<Vector2>();
    }

    void MovePlayer()
    {
        if (eyesOpen == false)
        {
            rb.velocity = new Vector3(playerMovement.x * moveSpeed, rb.velocity.y, playerMovement.y * moveSpeed);
        }
    }

    void OnLook(InputValue delta)
    {
        mouseDelta = delta.Get<Vector2>() * lookSensitivity;
    }

    void RotateCamera()
    {
        //look up and down
        transform.Rotate(Vector3.up * mouseDelta.x);

        //look left and right
        playerCamera.transform.Rotate(Vector3.left * mouseDelta);
    }
}
