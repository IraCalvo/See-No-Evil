using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("PlayerStats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float mouseSens;
    [SerializeField] float gravity = 10f;

    [Header("Player's Eyes")]
    [SerializeField] GameObject eyes;
    [SerializeField] float timerToOpenEyes;
    public bool eyesOpen = true;
    bool canOpenEyes;
    bool canShoot = true;
    bool hasBlinkedOnce = false;

    [Header("Player's Gun")]
    public GameObject gun;
    [SerializeField] float timeBetweenShots;
    public int ammoCount;

    [Header("Misc/Hookups")]
    [SerializeField] AudioClip[] footSteps;
    public Transform playerLight;
    bool footStepsPlaying;

    CharacterController characterController;
    float verticalRotation = 0f;
    private Vector2 playerMovement;
    private Vector2 mouseDelta;
    private Vector3 moveDirection = Vector3.zero;
    Transform playerBody;
    AudioSource audioSource;

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
        playerBody = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        //gun.SetActive(false);
        //playerLight.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Gravity();
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
        if(value.isPressed && !hasBlinkedOnce)
        {
            hasBlinkedOnce = true;
            CanvasManager.instance.RemoveStartScreen();
            StartCoroutine(StartOpenEyeTimerCoroutine());
        }
        else if (value.isPressed && eyesOpen)
        {
            StartCoroutine(StartOpenEyeTimerCoroutine());
        }
        //eyes open cant move
        else if (!eyesOpen && canOpenEyes)
        {
            eyesOpen = true;
            CanvasManager.instance.Eyes.gameObject.GetComponent<Animator>().SetTrigger("OpenEyes");
        }
    }

    void OnFire(InputValue value)
    {
        //if (canShoot && ammoCount >= 1 && !eyesOpen && GameManager.instance.hasGun)
        //{
        //    StartCoroutine(ShootGunCoroutine());
        //}
    }

    void Gravity()
    {
        moveDirection.y += gravity * Time.deltaTime;
        var flags = characterController.Move(moveDirection * Time.deltaTime);
    }

    void MovePlayer()
    {
        //eyes are closed and can move
        if (!eyesOpen)
        {
            Vector3 forwardDirection = playerBody.TransformDirection(Vector3.right);
            Vector3 rightDirection = playerBody.TransformDirection(Vector3.forward);

            float currentSpeedX = (moveSpeed * playerMovement.x);
            float currentSpeedY = (moveSpeed) * playerMovement.y;
            moveDirection = (forwardDirection * currentSpeedX) + (rightDirection * currentSpeedY);

            characterController.Move(moveDirection * Time.deltaTime);

            //rotates camera
            verticalRotation -= mouseDelta.y * mouseSens;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 75f);
            Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            transform.rotation *= Quaternion.Euler(0, mouseDelta.x * mouseSens, 0);

            //footsteps sfx
            if (footStepsPlaying == false)
            {
                PlayFootStepSFX();
                footStepsPlaying = true;
            }
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    void PlayFootStepSFX()
    {
        int n = Random.Range(1, footSteps.Length);

        audioSource.clip = footSteps[n];
        audioSource.PlayOneShot(audioSource.clip);

        StartCoroutine(WaitForFoorStepCoroutine(audioSource.clip.length));

        footSteps[n] = footSteps[0];
        footSteps[0] = audioSource.clip;
    }

    IEnumerator StartOpenEyeTimerCoroutine()
    {
        eyesOpen = false;
        CanvasManager.instance.Eyes.gameObject.GetComponent<Animator>().SetTrigger("CloseEyes");
        canOpenEyes = false;
        yield return new WaitForSeconds(timerToOpenEyes);
        canOpenEyes = true;
    }

    IEnumerator ShootGunCoroutine()
    {
        canShoot = false;
        ammoCount--;
        CanvasManager.instance.ammoText.text = "Ammo: " + ammoCount.ToString();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.GetComponent<EnemyBehavior>())
            {
                hit.collider.GetComponent<EnemyBehavior>().Flee();
            }
            else
            {
                Debug.Log("hit the:" + hit.collider.gameObject.name);
            }
        }
        //play sfx
        AudioManager.instance.PlaySFX(0);
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }

    IEnumerator WaitForFoorStepCoroutine(float delay)
    {
        footStepsPlaying = true;
        yield return new WaitForSeconds(delay);
        footStepsPlaying = false;
    }
}
