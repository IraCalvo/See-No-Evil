using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] string interactTextToShow;
    
    public void Interact()
    {
        gameObject.SetActive(false);
    }

    public void InteractText()
    {
        if (PlayerController.instance.eyesOpen)
        {
            CanvasManager.instance.interactText.text = interactTextToShow;
            CanvasManager.instance.interactText.enabled = true;
        }
        else
        {
            CanvasManager.instance.interactText.enabled = false;
        }
    }
}
