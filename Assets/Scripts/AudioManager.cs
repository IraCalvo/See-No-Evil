using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource[] sfx;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(instance);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(int sfxToPlay)
    { 
        
    }
}
