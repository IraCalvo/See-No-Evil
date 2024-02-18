using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    //this is the enemy's navMeshAgent
    private NavMeshAgent enemy;
    
    //this is the players location
    private Transform player;
    
    //List of potential stalking locations that enemy might chose 
    public List<GameObject> peakingNodes = new List<GameObject>();

    //boolean variable to check what state the monster is in
    bool isChasing = false;
    bool isWandering = false;
    bool isFleeing = false;
    bool isPatrolling = false;

    //PatrolTarget is next location to go when isPatrol is true
    Vector3 patrolTarget;
    
    //Keeps track on what patrol node the enemy is on
    int patrolIndex;
    
    //The list of nodes of where to patrol
    public Transform[] patrolRoute;

    

    void Start()
    {
       
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;

        //starts the monsters AI
        StartCoroutine(movementOpportunity());
    }

    void Update()
    {
       //Always Look at the player
        transform.LookAt(player.position);
        
        //When is Chasing is true follow player
        if(isChasing){
            enemy.SetDestination(player.position);
        } 


        if(isWandering){
            if(enemy.remainingDistance <= enemy.stoppingDistance) //done with path
            {
                Vector3 point;
                if (wander(transform.position, 5.0f, out point)) //pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                    enemy.SetDestination(point);
                }
            }  
        }

        if(isPatrolling){
            if(Vector3.Distance(transform.position, patrolTarget) < 1){
                UpdatePatrolDestination();
                InterateWaypointIndex();
            }
        }


        //NEVER GETS CALLED
        if(isFleeing){
            Flee();
        }
    }
    

    IEnumerator movementOpportunity(){
        while(true){
            if(Random.value < 0.6f){
                isPatrolling = true;
                enemy.speed = 2f;
                UpdatePatrolDestination();
                Debug.Log("Patroling");
            }else{
                isPatrolling = false;
                Stalk(); 
                
            }
            yield return new WaitForSeconds(5f);
            Debug.Log("New Decesion has been made");
        }
    }

    IEnumerator ChasePlayer(){
        Debug.Log("Chasing");
        enemy.speed = 2f;
        isChasing = true;
        yield return new WaitForSeconds(5f); 
        isChasing = false; 
        StartCoroutine(Wandering());    

    }

    IEnumerator Wandering(){
        Debug.Log("Wandering");
        enemy.speed= 2f;
        isWandering = true;
        yield return new WaitForSeconds(8f);
        StartCoroutine(movementOpportunity());
        isWandering = false;

    }
    
    //Deals with patrols and updating where to go
    void UpdatePatrolDestination(){
        patrolTarget = patrolRoute[patrolIndex].position;
        enemy.SetDestination(patrolTarget);
    }

    void InterateWaypointIndex(){
        patrolIndex++;
        if(patrolIndex == patrolRoute.Length){
            patrolIndex = 0;
        }
    }
    ///////////////////////////////////////////////
   
   void Stalk(){
        Debug.Log("Stalking");
        if(peakingNodes.Count > 0){
            enemy.speed = 10f;
            enemy.SetDestination(peakingNodes[Random.Range(0,peakingNodes.Count)].transform.position);
        }
        
    }

    //NEVER IS USED
    void Flee(){
        Debug.Log("Fleeing");
        enemy.speed = 10f;
        Vector3 FleeDestination = transform.position - player.position;
        Vector3 newPos = transform.position + FleeDestination * 2;
        enemy.SetDestination(newPos);
    }

    
    bool wander(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range;  
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 0.4f, NavMesh.AllAreas)) 
        { 
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    //Add potential location for the enemy to stalk the player
    public void addToPeakingLocations(GameObject obj ){
        //checks if a location is already considered, if not add it
        if(!peakingNodes.Contains(obj)){
            peakingNodes.Add(obj);
        }
        
    }

    //removes a stalking location
    public void removeFromPeakingLocations(GameObject obj){
        peakingNodes.Remove(obj);
    }


    void OnTriggerStay(Collider col){        
        if(col.CompareTag("Player")){       
            StopAllCoroutines();
            StartCoroutine(ChasePlayer());
        }
    }

    
}
