using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WrongText : MonoBehaviour
{
    public AnimationCurve moveCurve = null;     // �ִϸ��̼� Ŀ��
    private RawImage rImage = null;             // �ؽ�Ʈ���� �̹���
    private RectTransform rt = null;            // �̹����� RectTransform

    private void Awake()
    {
        rImage = GetComponent<RawImage>();
        rt = rImage.rectTransform;
    }

    public void OnEnable()
    {
        rt.localPosition = new Vector2(0, 0);
        rImage.color = new Color(rImage.color.r, rImage.color.g, rImage.color.b, 1);
        StartCoroutine(Move());
    }

    // �ִϸ��̼� Ŀ�긦 ���� �ؽ�Ʈ �̵��� ��Ȱ��ȭ
    IEnumerator Move()
    {
        float time = 0f;
        while (time < 1.3)
        {

            time += Time.deltaTime;

            rt.localPosition = Vector2.up * moveCurve.Evaluate(time);
            rImage.color = new Color(rImage.color.r, rImage.color.g, rImage.color.b, rImage.color.a - 0.01f);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
