using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Teleportation")]
    public Transform Floor1Stairs;
    public Transform Floor2Stairs;
    public GameObject[] enemyNodesFloor1;

    [Header("Player current progression things")]
    public float currentFloorPlayerIsOn;
    public bool hasKey = false;
    public bool hasGun = false;
    public bool hasWrench = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(this);
        }
    }

    public void PlayerFloorUpdate(int floorPlayerOn)
    {
        if (floorPlayerOn == 1)
        {
            PlayerController.instance.transform.position = Floor1Stairs.position;
            updateEnemyFloorPosition(floorPlayerOn);
        }
        if (floorPlayerOn == 2)
        {
            PlayerController.instance.transform.position = Floor2Stairs.position;
            updateEnemyFloorPosition(floorPlayerOn);
        }
    }

    void updateEnemyFloorPosition(int moveToFloor)
    { 
        
    }

    public void GunAcquired()
    {
        hasGun = true;
        PlayerController.instance.gameObject.SetActive(true);
    }
}

//these functions will be called when the player goes on the stairs
//have a list of possible nodes that the enemy can randomly spawn on when the player goes to another floor
//then randomly choose one of said nodes and start from there additionally can factor what nodes the enemy can spawn on
//based on the player's progress such as if they have the Key then the enemy can spawn on other nodes that weren't possible previously
