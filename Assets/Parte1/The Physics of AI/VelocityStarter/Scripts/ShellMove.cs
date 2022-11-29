using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMove : MonoBehaviour
{
    float speed = 1;
    void Update()
    {
        this.transform.Translate(0, speed*Time.deltaTime, speed* Time.deltaTime);
    }
}
