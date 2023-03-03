using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInfo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<FruitInfo>())
            FruitPooling.Inst.In(collision.GetComponent<FruitInfo>());
        if (collision.GetComponent<SliceInfo>())
            SlicePooling.Inst.In(collision.GetComponent<SliceInfo>());
        if (collision.GetComponent<BoomInfo>())
        {
            Destroy(collision.GetComponent<BoomInfo>().gameObject);
        }
        else return;
    }
}
