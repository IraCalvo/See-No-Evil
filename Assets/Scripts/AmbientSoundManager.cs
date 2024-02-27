using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    [Header("This is the list of potenitial sounds")]
    [SerializeField] AudioClip[] ambientSounds;
    [SerializeField] float minimumAmountOfTimeBetweenSounds;
    [SerializeField] float maximumAmountOfTimeBetweenSounds;
    private float randomTimer;
    private bool audioCurrentlyPlaying;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ChooseRandomTimer();
    }

    private void Update()
    {
        randomTimer -= Time.deltaTime;

        if (randomTimer <= 0 && audioCurrentlyPlaying == false)
        {
            int n = Random.Range(1, ambientSounds.Length);
            audioSource.clip = ambientSounds[n];
            audioSource.PlayOneShot(audioSource.clip);

            StartCoroutine(WaitForAmbientSoundToFinishCoroutine(audioSource.clip.length));
        }
    }

    void ChooseRandomTimer()
    {
        randomTimer = Random.Range(minimumAmountOfTimeBetweenSounds, maximumAmountOfTimeBetweenSounds);
        Debug.Log(randomTimer);
    }

    IEnumerator WaitForAmbientSoundToFinishCoroutine(float delay)
    {
        audioCurrentlyPlaying = true;
        yield return new WaitForSeconds(delay);
        ChooseRandomTimer();
        audioCurrentlyPlaying = false;
    }
}
