using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public int floorToGoTo;

    void OnTriggerEneter(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            GameManager.instance.PlayerFloorUpdate(floorToGoTo);
        }
    }
}
