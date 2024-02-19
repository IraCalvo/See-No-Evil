using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact();
    public void InteractText();
}

public class Interact : MonoBehaviour
{
    private Transform InteractorSource;
    [SerializeField] float interactRange;

    private void Awake()
    {
        InteractorSource = GetComponent<Transform>();
    }

    private void Update()
    {
        Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.InteractText();
            }
        }
        else
        {
            if (CanvasManager.instance.interactText != null)
            {
                CanvasManager.instance.interactText.enabled = false;
            }

        }
    }

    void OnInteract(InputValue value)
    {
        Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
