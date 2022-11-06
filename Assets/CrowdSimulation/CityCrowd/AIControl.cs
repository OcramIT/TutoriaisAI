using UnityEngine;

public class AIControl : MonoBehaviour {

    GameObject[] goalLocations;
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;


    // Use this for initialization
    void Start() {
        float sm = Random.Range(0.5f, 2);

        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        anim = this.GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0, 1));
        anim.SetTrigger("isWalking");
        anim.SetFloat("speedMulti", sm);
        agent.speed *= sm;
    }

    // Update is called once per frame
    void Update() {

        if (agent.remainingDistance < 1) {

            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
    }
}
