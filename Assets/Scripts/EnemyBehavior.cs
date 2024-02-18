using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    //this is the enemy's navMeshAgent
    private NavMeshAgent enemy;
    private float enemySpeed;
    //this is the players location
    private Transform player;
    
    //List of potential stalking locations that enemy might chose 
    public List<GameObject> peakingNodes = new List<GameObject>();


    bool isChasing = false;
    bool isWandering = false;
    bool isFleeing = false;

    bool isPatrolling = false;
    Vector3 patrolTarget;
    int patrolIndex;
    public Transform[] patrolRoute;

    

    void Start()
    {
        enemySpeed = 2.0f;
        enemy = GetComponent<NavMeshAgent>();
        enemy.acceleration = 200f;
        player = GameObject.FindWithTag("Player").transform;
        
        //isPatrolling = true;
        //UpdatePatrolDestination();
        StartCoroutine(movementOpportunity());
    }

    void Update()
    {
        //Always follow the player >:)
        //enemy.SetDestination(player.position);
        transform.LookAt(player.position);
        enemy.speed = enemySpeed; 
        
        if(isChasing){
            enemy.SetDestination(player.position);
        } 

        if(isWandering){
            if(enemy.remainingDistance <= enemy.stoppingDistance) //done with path
            {
                Vector3 point;
                if (wander(transform.position, 5.0f, out point)) //pass in our centre point and radius of area
                {
                    //Debug.Log("activating");
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                    enemy.SetDestination(point);
                }
            }  
        }

        if(isPatrolling){
            //Debug.Log(Vector3.Distance(transform.position, patrolTarget));
            if(Vector3.Distance(transform.position, patrolTarget) < 1){
                Debug.Log("We In");
                UpdatePatrolDestination();
                InterateWaypointIndex();
            }
        }

        if(isFleeing){
            Flee();
        }
    }

    IEnumerator movementOpportunity(){
        while(true){
            if(Random.value < 1f){
                ChasePlayer();
                yield return new WaitForSeconds(8f);
                isFleeing = true;
                enemySpeed = 10f;
                isChasing = false;
                Debug.Log("Stopped Chasing");
                //StartCoroutine(movementOpportunity());
                yield break;
            }else if(Random.value < 0.5f){
                Stalk(); 
            } else{
                Wandering();
                yield return new WaitForSeconds(5f);
                isWandering = false;
            }
            
           

            yield return new WaitForSeconds(5f);
            Debug.Log("New Decesion has been made");
        }
    }
    
    void UpdatePatrolDestination(){
        Debug.Log("Hey");
        patrolTarget = patrolRoute[patrolIndex].position;
        enemy.SetDestination(patrolTarget);
    }

    void InterateWaypointIndex(){
        patrolIndex++;
        if(patrolIndex == patrolRoute.Length){
            patrolIndex = 0;
        }
    }

   void Stalk(){
        Debug.Log("Stalking");
        if(peakingNodes.Count > 0){
            enemySpeed = 10f;
            enemy.SetDestination(peakingNodes[Random.Range(0,peakingNodes.Count)].transform.position);
        }
        
    }

    void ChasePlayer(){
        Debug.Log("Chasing");
        enemySpeed = 2f;
        isChasing = true;

    }

    void Flee(){
        Debug.Log("Fleying");
        enemySpeed = 10f;
        Vector3 FleeDestination = transform.position - player.position;
        Vector3 newPos = transform.position + FleeDestination * 2;
        enemy.SetDestination(newPos);
    }

    void Wandering(){
        Debug.Log("Wandering");
        enemySpeed = 2f;
        isWandering = true;
    }

    bool wander(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 0.4f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //Debug.Log("working");
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }


    







    //Add potential location for the enemy to stalk the player
    public void addToPeakingLocations(GameObject obj ){
        Debug.Log("added");
        //checks if a location is already considered, if not add it
        if(!peakingNodes.Contains(obj)){
            peakingNodes.Add(obj);
        }
        
    }

    //removes a stalking location
    public void removeFromPeakingLocations(GameObject obj){
        peakingNodes.Remove(obj);
    }

    
}
