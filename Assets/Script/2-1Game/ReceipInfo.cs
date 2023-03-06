using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 좌측 상단의 로프에 달릴 주문서 스크립트
public class ReceipInfo : MonoBehaviour
{
    RectTransform rt = null;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    // 초기화 함수
    public void Init(Texture _texture, string _name)
    {
        GetComponent<RawImage>().texture = _texture;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _name;

        StartCoroutine(ScaleOn());
    }

    // 스케일을 조절해 작아졌다 커지는 연출 코루틴
    IEnumerator ScaleOn()
    {
        while (rt.localScale.x <= 1.1f)
        {
            rt.localScale = new Vector3(rt.localScale.x + 0.05f,
                                        rt.localScale.y + 0.05f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        while (rt.localScale.x >= 0.95f)
        {
            rt.localScale = new Vector3(rt.localScale.x - 0.05f, rt.localScale.y - 0.05f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        while (rt.localScale.x >= 1f)
        {
            rt.localScale = new Vector3(rt.localScale.x + 0.05f, rt.localScale.y + 0.05f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        rt.localScale = new Vector3(1, 1, 0);
    }
}
