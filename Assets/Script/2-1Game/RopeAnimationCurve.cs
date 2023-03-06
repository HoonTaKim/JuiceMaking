using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RopeAnimationCurve : MonoBehaviour
{
    public AnimationCurve moveCurve = null;     // �ִϸ��̼� Ŀ��
    private RawImage ri = null;                 // ���� �̹���
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

    // �ִϸ��̼� Ŀ�긦���� �̵� �� ����
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
