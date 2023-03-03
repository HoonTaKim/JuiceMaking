using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrongText : MonoBehaviour
{
    public AnimationCurve moveCurve = null;

    //public RectTransform childTr;
    private RawImage ri = null;
    private RectTransform rt = null;

    private void Awake()
    {
        ri = GetComponent<RawImage>();
        rt = ri.rectTransform;
    }

    public void OnEnable()
    {
        rt.localPosition = new Vector2(0, 0);
        ri.color = new Color(ri.color.r, ri.color.g, ri.color.b, 1);
        //moveCurve.Evaluate(0.3f);
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float time = 0f;
        while (time < 1.3)
        {

            time += Time.deltaTime;

            rt.localPosition = Vector2.up * moveCurve.Evaluate(time);
            //rt.localPosition = new Vector2(rt.localPosition.x, rt.localPosition.y + moveCurve.Evaluate(time));
            ri.color = new Color(ri.color.r, ri.color.g, ri.color.b, ri.color.a - 0.01f);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
