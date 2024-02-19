using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI ammoText;
    public GameObject crossHair; 

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

        interactText.enabled = false;
        crossHair.SetActive(false);
    }
}
