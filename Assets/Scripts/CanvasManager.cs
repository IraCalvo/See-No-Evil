using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    public TextMeshProUGUI title;
    public TextMeshProUGUI pressSpaceBar;
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

    public void RemoveStartScreen()
    {
        if (title == null)
        {
            return;
        }
        title.enabled = false;
        pressSpaceBar.enabled = false;
    }
}
