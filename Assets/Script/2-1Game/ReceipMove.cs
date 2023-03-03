using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReceipMove : MonoBehaviour
{
    private RectTransform rectTransform = null;
    private RawImage rawImage = null;
    [SerializeField] private RawImage juiceImage = null;
    [SerializeField] private TextMeshProUGUI tmp = null;
    [SerializeField] private Button buttonImage = null;

    private SelectFruit_UiManager uiManager = null;

    private void Awake()
    {
        uiManager = GameManager.Inst.Get_SelectFruitUiManager;
        rectTransform = GetComponent<RectTransform>();
        rawImage = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        StartCoroutine(MoveOn());
        StartCoroutine(TransparencyOn());
    }

    // 이동
    private IEnumerator MoveOn()
    {
        while (rectTransform.localPosition.y >= 0)
        {
            rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y - 5f, 0);
            yield return new WaitForSeconds(0.0001f);
        }

        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, 0, 0);
    }

    public void TransparencyOff()
    {
        StartCoroutine(TransparencyOff_Start());
    }

    // 투명도 조절
    private IEnumerator TransparencyOn()
    {
        while (rawImage.color.a >= 0f && rawImage.color.a < 1f)
        {
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, rawImage.color.a + 0.01f);
            juiceImage.color = new Color(juiceImage.color.r, juiceImage.color.g, juiceImage.color.b, juiceImage.color.a + 0.01f);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a + 0.01f);
            buttonImage.image.color = new Color(buttonImage.image.color.r, buttonImage.image.color.g, buttonImage.image.color.b, buttonImage.image.color.a + 0.01f);

            yield return new WaitForSeconds(0.001f);
        }

        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 1);
        juiceImage.color = new Color(juiceImage.color.r, juiceImage.color.g, juiceImage.color.b, 1);
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1);
        buttonImage.image.color = new Color(buttonImage.image.color.r, buttonImage.image.color.g, buttonImage.image.color.b, 1);
    }

    private IEnumerator TransparencyOff_Start()
    {
        while (rawImage.color.a >= 0)
        {
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, rawImage.color.a - 0.01f);
            juiceImage.color = new Color(juiceImage.color.r, juiceImage.color.g, juiceImage.color.b, juiceImage.color.a - 0.01f);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a - 0.01f);
            buttonImage.image.color = new Color(buttonImage.image.color.r, buttonImage.image.color.g, buttonImage.image.color.b, buttonImage.image.color.a - 0.01f);

            yield return new WaitForSeconds(0.001f);
        }

        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0);
        juiceImage.color = new Color(juiceImage.color.r, juiceImage.color.g, juiceImage.color.b, 0);
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0);
        buttonImage.image.color = new Color(buttonImage.image.color.r, buttonImage.image.color.g, buttonImage.image.color.b, 0);

        gameObject.SetActive(false);
    }
}
