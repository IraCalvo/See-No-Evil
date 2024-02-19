using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchDoor : MonoBehaviour, IInteractable
{
    [SerializeField] string interactTextToShow;

    public void Interact()
    {
        if (PlayerController.instance.hasWrench == true)
        {
            gameObject.SetActive(false);
        }
    }

    public void InteractText()
    {
        if (PlayerController.instance.eyesOpen && PlayerController.instance.hasWrench == false)
        {
            CanvasManager.instance.interactText.text = "A wrench can break this it seems...";
            CanvasManager.instance.interactText.enabled = true;
        }
        if (PlayerController.instance.eyesOpen && PlayerController.instance.hasWrench == true)
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
