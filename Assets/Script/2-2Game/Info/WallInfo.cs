using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInfo : MonoBehaviour
{
    // 과일, 잘린과일, 썩은과일들이 닿을시 오브젝트 풀링에 저장
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
