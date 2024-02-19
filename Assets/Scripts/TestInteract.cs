using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestInteract : MonoBehaviour, IInteractable
{
    [SerializeField] string interactTextToShow;
    public void Interact()
    {
        Debug.Log(Random.Range(0, 100));
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
