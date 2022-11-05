using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject shellPrefab;
    public GameObject shellSpawnPos;
    public GameObject target;
    public GameObject parent;

    float turnSpeed=2;
    float speed = 30;
    bool canShoot = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void CanShootAgain()
    {
        canShoot = true;
    }
    void Fire()
    {
        if (canShoot)
        {
            GameObject shell = Instantiate(shellPrefab, shellSpawnPos.transform.position, shellSpawnPos.transform.rotation);
            shell.GetComponent<Rigidbody>().velocity = speed * this.transform.forward;
            canShoot = false;
            Invoke("CanShootAgain", 0.5f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        

        Vector3 direction = (target.transform.position - parent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        float? angle = RotateTurret();

        if(angle!=null && Vector3.Angle(direction, parent.transform.forward) < 10)
        {
            Fire();
        }
    }

    float? RotateTurret()
    {
        float? angle = CalculateAngle(true); // "true" escolhe o low angle, "false" escolhe o high angle
        if (angle != null)
        {
            this.transform.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f);
        }
        return angle;
    }

    float? CalculateAngle(bool low) // "float?" -> retorn float ou null
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float y = targetDir.y;
        targetDir.y = 0f;
        float x = targetDir.magnitude;
        float gravity = 9.81f;
        float sSqr = speed * speed;
        float underTheSqrRoot = (sSqr * sSqr) - gravity * (gravity * x * x + 2 * y * sSqr);

        if (underTheSqrRoot>=0f)
        {
            float root = Mathf.Sqrt(underTheSqrRoot);
            float highAngle = sSqr + root;
            float lowAngle = sSqr - root;

            if (low)
            {
                return (Mathf.Atan2(lowAngle, gravity + x) * Mathf.Rad2Deg);
            }
            else
            {
                return (Mathf.Atan2(highAngle, gravity + x) * Mathf.Rad2Deg);
            }
        }
        else
        {
            return null;
        }
    }
}
