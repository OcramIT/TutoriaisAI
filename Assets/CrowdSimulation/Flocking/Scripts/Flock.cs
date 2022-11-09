using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager fManager;
    float speed;
    bool turning = false;

    void Start()
    {
        speed = Random.Range(fManager.minSpeed, fManager.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        // dá set dos limites do manager
        Bounds b = new Bounds(fManager.transform.position, fManager.swimLimits * 2);

        // o peixe vira se sair dos limites
        if (!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            // rodar em direção ao centro do manager
            Vector3 direction = fManager.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), fManager.rotationSpeed * Time.deltaTime);

        }
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(fManager.minSpeed, fManager.maxSpeed);

            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }

        transform.Translate(0, 0, Time.deltaTime * speed); 
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = fManager.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (nDistance <= fManager.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if(nDistance < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (fManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), fManager.rotationSpeed * Time.deltaTime);

            }
        }
        
    }
}
