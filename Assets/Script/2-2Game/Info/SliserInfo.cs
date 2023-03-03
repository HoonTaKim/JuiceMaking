using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliserInfo : MonoBehaviour
{
    private Vector3 pos;

    public Vector3 destination;

    public float speed;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
    }

    public void Move(Vector3 finalPos)
    {
        
    }
}
