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
    public GameObject gun;
    [SerializeField] float timeBetweenShots;
    [SerializeField] AudioClip[] footSteps;
    public bool eyesOpen = true;
    bool canOpenEyes;
    bool canShoot = true;
    bool footStepsPlaying;
    public int ammoCount;

    float verticalRotation = 0f;
    private Vector2 playerMovement;
    private Vector2 mouseDelta;
    Rigidbody rb;
    Transform playerBody;
    AudioSource audioSource;

    [Header("bools for player progression")]
    public bool hasKey;

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
        audioSource = GetComponent<AudioSource>();
        gun.SetActive(false);
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
        if (!eyesOpen)
        {
            Vector3 forwardDirection = playerBody.forward;
            Vector3 movement = forwardDirection * playerMovement.y + playerBody.right * playerMovement.x;
            rb.velocity = movement * moveSpeed;
            if (rb.velocity.magnitude > 0)
            {
                if (footStepsPlaying == false)
                {
                    PlayFootStepSFX();
                    footStepsPlaying = true;
                }
            }
        }

        playerBody.Rotate(Vector3.up * mouseDelta.x * mouseSens * Time.fixedDeltaTime);

        verticalRotation -= mouseDelta.y * mouseSens * Time.fixedDeltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 75f);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
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
        eyes.GetComponent<Animator>().SetTrigger("CloseEyes");
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
