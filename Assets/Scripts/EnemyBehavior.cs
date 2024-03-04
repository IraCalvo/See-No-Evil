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

    //String variable to check what state the monster is in
    public string state;

    //PatrolTarget is next location to go when isPatrol is true
    Vector3 patrolTarget;

    //Keeps track on what patrol node the enemy is on
    int patrolIndex;

    //The list of nodes of where to patrol
    public Transform[] patrolRoute;

    float temp;

    void Start()
    {

        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        state = "Patrolling";
   

        //starts the monsters AI
        UpdatePatrolDestination();
        StartCoroutine(movementOpportunity());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("HELLO");
            temp = enemy.speed;
            enemy.speed = 0.001f;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            enemy.speed = temp;
            temp = 0;
        }


        //Always Look at the player
        if (state == "Chasing" || state == "Stalking")
        {
            transform.LookAt(player.position);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(enemy.velocity.normalized);

        }

        switch(state)
        {
            case "Chasing":
                enemy.SetDestination(player.position);
                break;

            case "Wandering":
                if (enemy.remainingDistance <= enemy.stoppingDistance) //done with path
                {
                    Vector3 point;
                    if (wander(transform.position, 5.0f, out point)) //pass in our centre point and radius of area
                    {
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                        enemy.SetDestination(point);
                    }
                }
                break;

            case "Patrolling":
                if (Vector3.Distance(transform.position, patrolTarget) < 1.5f)
                {
                    UpdatePatrolDestination();
                    InterateWaypointIndex();
                }
                break;

            case "Fleeing":
                Flee();
                break;

        }

    }


    IEnumerator movementOpportunity()
    {
        while (true)
        {

            if (Random.value < 0.5f && peakingNodes.Count > 0)
            {
                StartCoroutine(Stalk());
            }
            else
            {
                state = "Patrolling";
                enemy.speed = 2f;
                UpdatePatrolDestination();
            }
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator ChasePlayer()
    {
        Debug.Log("Chasing");
        enemy.speed = 2f;
        state = "Chasing";
        yield return new WaitForSeconds(5f);
        StartCoroutine(Wandering());

    }

    IEnumerator Wandering()
    {
        enemy.speed = 2f;
        state = "Wandering";
        yield return new WaitForSeconds(8f);
        StartCoroutine(movementOpportunity());
    }

    IEnumerator Stalk()
    {
        state = "Stalking";
        if (peakingNodes.Count > 0)
        {
            enemy.speed = 8f;
            enemy.SetDestination(peakingNodes[Random.Range(0, peakingNodes.Count)].transform.position);
        }
        yield return new WaitForSeconds(8f);
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
        state = "Fleeing";
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
            StopAllCoroutines();
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