using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFruit : MonoBehaviour
{
    [SerializeField] private UiManager uiManager = null;

    private Rigidbody2D rb = null;
    private void OnEnable()
    {
        this.transform.rotation = new Quaternion(0, 0, Random.Range(-30f, 30f), 0);
        this.GetComponent<SpriteRenderer>().sprite = GameManager.Inst.Get_SliceSpriteList[Random.Range(0, GameManager.Inst.Get_SliceSpriteList.Count)];
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (uiManager.Get_MixButtonOn)
        {
            if (Random.Range(0, 2) == 0)
            {
                rb.AddForce(Vector3.left * 40f);
            }
            else
                rb.AddForce(Vector3.right * 40f);
        }
    }
}
