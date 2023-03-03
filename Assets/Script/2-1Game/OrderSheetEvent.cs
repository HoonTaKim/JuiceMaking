using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderSheetEvent : MonoBehaviour
{
    private RectTransform rectTransform = null;
    private RawImage juiceImage = null;
    [SerializeField] private TextMeshProUGUI tmp = null;
    [SerializeField] private Button buttonImage = null;
    public bool cloneOrderSheetOn = false;                  // 

    private SelectFruit_UiManager uiManager = null;

    private void Awake()
    {
        juiceImage = GetComponent<RawImage>();
        uiManager = GameManager.Inst.Get_SelectFruitUiManager;
        rectTransform = GetComponent<RectTransform>();
        //rawImage = GetComponent<RawImage>();
    }

    //private void Start()
    //{
    //    uiManager.Get_ReceiptList.Add(this);
    //    StartCoroutine(MoveOn());
    //    StartCoroutine(TransparencyOn(0.01f, true));
    //}

    //public void TransparencyOff()
    //{
    //    StartCoroutine(TransparencyOn(-0.01f, false));
    //}

    //// 위에서 아래로 내려오는 연출
    //private IEnumerator MoveOn()
    //{
    //    while (rectTransform.localPosition.y >= 0)
    //    {
    //        rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y - 5f, 0);
    //        yield return new WaitForSeconds(0.0001f);
    //    }

    //    rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, 0, 0);
    //}

    //// 투명도 조절
    //private IEnumerator TransparencyOn(float _transparency, bool _setActive)
    //{
    //    while (buttonImage.image.color.a >= 0f && buttonImage.image.color.a < 1f)
    //    {
    //        //buttonImage.image.color = new Color(buttonImage.image.color.r, buttonImage.image.color.g, buttonImage.image.color.b, buttonImage.image.color.a + _transparency);
    //        juiceImage.color = new Color(juiceImage.color.r, juiceImage.color.g, juiceImage.color.b, juiceImage.color.a + _transparency);
    //        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a + _transparency);

    //        yield return new WaitForSeconds(0.001f);
    //    }

    //    //buttonImage.image.color = new Color(buttonImage.image.color.r, buttonImage.image.color.g, buttonImage.image.color.b, 1);
    //    juiceImage.color = new Color(juiceImage.color.r, juiceImage.color.g, juiceImage.color.b, 1);
    //    tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1);

    //    gameObject.SetActive(_setActive);
    //}
}
