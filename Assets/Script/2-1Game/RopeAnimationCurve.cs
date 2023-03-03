using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RopeAnimationCurve : MonoBehaviour
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

    public void Curve()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float time = 0f;
        //Debug.Log("코루틴은 들어옴?");
        while (time < 1.3)
        {
            //Debug.Log("while문 실행?");

            time += Time.deltaTime;

            rt.localPosition = Vector2.up * moveCurve.Evaluate(time);
            yield return null;
        }

        Destroy(this);
    }
}
