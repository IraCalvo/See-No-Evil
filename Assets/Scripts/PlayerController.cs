using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] float moveSpeed;
    [SerializeField] float mouseSens;
    [SerializeField] float timerToOpenEyes;
    [SerializeField] GameObject eyes;
    [SerializeField] float timeBetweenShots;
    public bool eyesOpen = true;
    bool canOpenEyes;
    bool canShoot = true;
    [SerializeField] int ammoCount;

    float verticalRotation = 0f;
    private Vector2 playerMovement;
    private Vector2 mouseDelta;
    Rigidbody rb;
    Transform playerBody;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
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

    void OnBlink(InputValue value)
    {
        if (value.isPressed && eyesOpen)
        {
            StartCoroutine(StartOpenEyeTimerCoroutine());
        }
        else if (!eyesOpen && canOpenEyes)
        {
            eyesOpen = true;
            eyes.GetComponent<Animator>().SetTrigger("OpenEyes");
            rb.velocity = Vector3.zero;
        }
    }

    void OnFire(InputValue value)
    {
        if (canShoot && ammoCount >= 1 && !eyesOpen)
        {
            StartCoroutine(ShootGunCoroutine());
        }
    }

    void MovePlayer()
    {
        //eyes are closed and can move
        if(!eyesOpen)
        {
            Vector3 forwardDirection = playerBody.forward;
            Vector3 movement = forwardDirection * playerMovement.y + playerBody.right * playerMovement.x;
            rb.velocity = movement * moveSpeed;
        }

        playerBody.Rotate(Vector3.up * mouseDelta.x * mouseSens * Time.fixedDeltaTime);

        verticalRotation -= mouseDelta.y * mouseSens * Time.fixedDeltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 75f);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    IEnumerator StartOpenEyeTimerCoroutine()
    {
        eyesOpen = false;
        eyes.GetComponent<Animator>().SetTrigger("CloseEyes");
        canOpenEyes = false;
        yield return new WaitForSeconds(timerToOpenEyes);
        canOpenEyes = true;
    }

    IEnumerator ShootGunCoroutine()
    {
        canShoot = false;
        ammoCount--;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.GetComponent<EnemyBehavior>())
            {
                Debug.Log("Do stuff here");
            }
            else 
            {
                Debug.Log("hit the:" + hit.collider.gameObject.name);
            }
        }
        //play sfx
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }
}
