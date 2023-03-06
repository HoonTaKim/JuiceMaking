using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomFogInfo : MonoBehaviour
{
    // 과일이 닿았을때 실행
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<FruitInfo>())
        {
            Color color = collision.GetComponent<SpriteRenderer>().color;
            collision.GetComponent<SpriteRenderer>().color = new Color(color.r / 2, color.g / 2, color.b / 2, 0.5f);
            // 과일이 잘리지 않게 bool값 변경
            collision.GetComponent<FruitInfo>().Set_CutOn(false);

            Destroy(this.gameObject, 0.5f);
        }
    }
}
