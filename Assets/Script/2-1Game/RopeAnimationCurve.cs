using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RopeAnimationCurve : MonoBehaviour
{
    public AnimationCurve moveCurve = null;     // 애니메이션 커브
    private RawImage ri = null;                 // 로프 이미지
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

    // 애니메이션 커브를통해 이동 후 삭제
    IEnumerator Move()
    {
        float time = 0f;
        while (time < 1.3)
        {
            time += Time.deltaTime;

            rt.localPosition = Vector2.up * moveCurve.Evaluate(time);
            yield return null;
        }

        Destroy(this);
    }
}
