using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float mouseSens;

    float verticalRotation = 0f;
    private Vector2 playerMovement;
    private Vector2 mouseDelta;
    Rigidbody rb;
    Transform playerBody;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        playerBody = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void OnMove(InputValue value)
    {
        playerMovement = value.Get<Vector2>();
    }

    void OnLook(InputValue delta)
    {
        mouseDelta = delta.Get<Vector2>();
    }

    void MovePlayer()
    {
        Vector3 forwardDirection = playerBody.forward;
        Vector3 movement = forwardDirection * playerMovement.y + playerBody.right * playerMovement.x;
        rb.velocity = movement * moveSpeed;

        playerBody.Rotate(Vector3.up * mouseDelta.x * mouseSens * Time.fixedDeltaTime);

        verticalRotation -= mouseDelta.y * mouseSens * Time.fixedDeltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}
