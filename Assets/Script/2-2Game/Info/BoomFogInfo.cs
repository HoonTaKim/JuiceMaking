using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomFogInfo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<FruitInfo>())
        {
            Color color = collision.GetComponent<SpriteRenderer>().color;
            collision.GetComponent<SpriteRenderer>().color = new Color(color.r / 2, color.g / 2, color.b / 2, 0.5f);
            collision.GetComponent<FruitInfo>().Set_CutOn(false);

            Destroy(this.gameObject, 0.5f);
        }
    }
}
