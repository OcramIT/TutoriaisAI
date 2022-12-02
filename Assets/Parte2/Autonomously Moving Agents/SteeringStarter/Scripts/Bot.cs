using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AMA
{
    public class Bot : MonoBehaviour
    {
        NavMeshAgent agent;
        public GameObject target;
        Drive dScript;
        // Start is called before the first frame update
        void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            dScript = target.GetComponent<Drive>();
        }

        void Seek(Vector3 location)
        {
            agent.SetDestination(location);
        }

        void Flee(Vector3 location)
        {
            Vector3 fleeVector = location - this.transform.position;
            agent.SetDestination(this.transform.position - fleeVector);
        }

        void Pursue()
        {
            Vector3 targetDir = target.transform.position - this.transform.position;

            float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
            float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

            if ((toTarget > 90 && relativeHeading < 20) || dScript.currentSpeed < 0.01f)
            {
                Seek(target.transform.position);
                return;
            }

            float lookAhead = targetDir.magnitude / (agent.speed + dScript.currentSpeed);
            Seek(target.transform.position + target.transform.forward * lookAhead);
        }

        void Evade()
        {
            Vector3 targetDir = target.transform.position - this.transform.position;
            float lookAhead = targetDir.magnitude / (agent.speed + dScript.currentSpeed);
            Flee(target.transform.position + target.transform.forward * lookAhead);
        }

        // Update is called once per frame
        void Update()
        {
            Evade();
        }
    }
}