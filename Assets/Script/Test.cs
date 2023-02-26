using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private Texture2D test = null;

    private void Awake()
    {
        this.GetComponent<RawImage>().texture = test;

        DontDestroyOnLoad(this);
    }
}
