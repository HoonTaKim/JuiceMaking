using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMixImage : MonoBehaviour
{
    [SerializeField] private MixUiManager mixUiManager = null;

    private void Update()
    {
        if (mixUiManager.Get_MixButtonOn)
        {
            if (transform.position.y < 1.7)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, 0);
            }
        }
    }
}
