using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControlV2 : MonoBehaviour {

    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator anim;
    float speedMulti;
    float detectionRadius = 20;
    float fleeRadius = 10;

    void ResetAgent()
    {
        float speedMult = Random.Range(0.1f, 1.5f);
        agent.speed = 2 * speedMult;
        agent.angularSpeed = 120;
        anim.SetFloat("speedMulti", speedMulti);
        anim.SetTrigger("isWalking");
        agent.ResetPath();
    }


    public void DetectNewObstacle(Vector3 positon)
    {
        if (Vector3.Distance(positon, this.transform.position) < detectionRadius)
        {
            Vector3 fleeDirection = (this.transform.position - positon).normalized;
            Vector3 newGoal = this.transform.position + fleeDirection * fleeRadius;

            NavMeshPath path= new NavMeshPath();
            agent.CalculatePath(newGoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                anim.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }

    // Use this for initialization
    void Start() {
        
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        anim = this.GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0, 1));
        ResetAgent();
    }

    // Update is called once per frame
    void Update() {

        if (agent.remainingDistance < 1) {
            ResetAgent();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
    }
}
