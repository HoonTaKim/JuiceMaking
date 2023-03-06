using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliserInfo : MonoBehaviour
{
    public Vector3 destination;

    public float speed;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }
}
