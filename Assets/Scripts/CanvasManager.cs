using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    public TextMeshProUGUI interactText;

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
    }
}
