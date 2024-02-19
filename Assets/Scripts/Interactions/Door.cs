using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] string interactTextToShow;
    
    public void Interact()
    {
        if (PlayerController.instance.hasKey == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void InteractText()
    {
        if (PlayerController.instance.eyesOpen && PlayerController.instance.hasKey == false)
        {
            CanvasManager.instance.interactText.text = "requires key to escape";
            CanvasManager.instance.interactText.enabled = true;
        }
        if (PlayerController.instance.eyesOpen && PlayerController.instance.hasKey == true)
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
