using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteract : MonoBehaviour, IInteractable
{
    [SerializeField] string interactTextToShow;

    public void Interact()
    {
        PlayerController.instance.gun.SetActive(true);
        CanvasManager.instance.crossHair.SetActive(true);
        CanvasManager.instance.ammoText.enabled = true;
        CanvasManager.instance.ammoText.text = "Ammo: " + PlayerController.instance.ammoCount.ToString();
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
