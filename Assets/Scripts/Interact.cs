using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact();
}

public class Interact : MonoBehaviour
{
    private Transform InteractorSource;
    public float interactRange;

    private void Awake()
    {
        InteractorSource = GetComponent<Transform>();
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
