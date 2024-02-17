using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    //this is the enemy's navMeshAgent
    private NavMeshAgent enemy;

    //this is the players location
    public Transform target;

   

     
   

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Always follow the player >:)
        enemy.SetDestination(target.position);    
    }

  

    
   

}
