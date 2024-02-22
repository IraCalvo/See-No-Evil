using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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
    bool isStalking = false;

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
        isPatrolling = true;
        UpdatePatrolDestination();
        //starts the monsters AI
        StartCoroutine(movementOpportunity());
    }

    void Update()
    {

        //Always Look at the player
        if (isChasing || isStalking)
        {
            transform.LookAt(player.position);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(enemy.velocity.normalized);

        }

        //When is Chasing is true follow player
        if (isChasing)
        {
            enemy.SetDestination(player.position);
        }


        if (isWandering)
        {
            if (enemy.remainingDistance <= enemy.stoppingDistance) //done with path
            {
                Vector3 point;
                if (wander(transform.position, 5.0f, out point)) //pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                    enemy.SetDestination(point);
                }
            }
        }

        if (isPatrolling)
        {
            Debug.Log(Vector3.Distance(transform.position, patrolTarget));
            if (Vector3.Distance(transform.position, patrolTarget) < 1.5f)
            {
                UpdatePatrolDestination();
                InterateWaypointIndex();
            }
        }


        //NEVER GETS CALLED
        if (isFleeing)
        {
            Flee();
        }


    }


    IEnumerator movementOpportunity()
    {
        while (true)
        {

            if (Random.value < 0.5f && peakingNodes.Count > 0)
            {
                isPatrolling = false;
                isStalking = true;
                StartCoroutine(Stalk());

            }
            else
            {
                isPatrolling = true;
                Debug.Log("Is p " + isPatrolling);
                enemy.speed = 2f;
                UpdatePatrolDestination();
                Debug.Log("Patroling");
            }
            yield return new WaitForSeconds(5f);
            //Debug.Log("New Decesion has been made");
        }
    }

    IEnumerator ChasePlayer()
    {
        Debug.Log("Chasing");
        enemy.speed = 2f;
        isChasing = true;
        yield return new WaitForSeconds(5f);
        isChasing = false;
        StartCoroutine(Wandering());

    }

    IEnumerator Wandering()
    {
        Debug.Log("Wandering");
        enemy.speed = 2f;
        isWandering = true;
        yield return new WaitForSeconds(8f);
        StartCoroutine(movementOpportunity());
        isWandering = false;

    }

    IEnumerator Stalk()
    {
        Debug.Log("Stalking");
        if (peakingNodes.Count > 0)
        {
            enemy.speed = 8f;
            enemy.SetDestination(peakingNodes[Random.Range(0, peakingNodes.Count)].transform.position);
        }
        yield return new WaitForSeconds(8f);
        isStalking = false;
    }

    //Deals with patrols and updating where to go
    void UpdatePatrolDestination()
    {
        patrolTarget = patrolRoute[patrolIndex].position;
        //Debug.Log(patrolRoute[patrolIndex].gameObject);
        enemy.SetDestination(patrolTarget);
    }

    void InterateWaypointIndex()
    {
        patrolIndex++;
        //Debug.Log(patrolIndex == patrolRoute.Length);
        if (patrolIndex == patrolRoute.Length)
        {
            patrolIndex = 0;
        }
    }
    ///////////////////////////////////////////////


    //NEVER IS USED
    public void Flee()
    {
        Debug.Log("Fleeing");
        AudioManager.instance.PlaySFX(1);
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
    public void addToPeakingLocations(GameObject obj)
    {
        //checks if a location is already considered, if not add it
        Debug.Log(obj);
        if (!peakingNodes.Contains(obj))
        {
            peakingNodes.Add(obj);
        }
    }

    //removes a stalking location
    public void removeFromPeakingLocations(GameObject obj)
    {
        peakingNodes.Remove(obj);
    }

    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            //StopAllCoroutines();
            StartCoroutine(ChasePlayer());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("HouseReal");
        }
    }
}