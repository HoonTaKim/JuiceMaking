using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WrongText : MonoBehaviour
{
    public AnimationCurve moveCurve = null;     // 애니메이션 커브
    private RawImage rImage = null;             // 텍스트가될 이미지
    private RectTransform rt = null;            // 이미지의 RectTransform

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

    // 애니메이션 커브를 통해 텍스트 이동후 비활성화
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
