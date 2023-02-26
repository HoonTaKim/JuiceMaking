using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class SelectFruit_UiManager : MonoBehaviour
{
    [SerializeField] private GameObject juiceParent = null;
    [SerializeField] private GameObject canvas = null;
    [SerializeField] private RawImage receip_Obj = null;
    [SerializeField] private GameObject storageBox = null;
    [SerializeField] private Image goodA = null;
    [SerializeField] private Image goodAni = null;
    [SerializeField] private RawImage screenCover = null;
    [SerializeField] private RawImage wrongTextImage = null;
    [SerializeField] private Image panel = null;
    [SerializeField] private Image orderList = null;
    [SerializeField] private RawImage orderRope = null;
    [SerializeField] private Image title = null;
    [SerializeField] private Image storageBoxAni = null;
    private List<OrderSheetEvent> receiptList = new List<OrderSheetEvent>();

    private RectTransform storageBoxRect;

    private float storageBopxOpenPos_X = 600f;
    private float storageBopxClosePos_X = 910f;
    private FruitHold fruitHold = null;

    //public static 

    private bool storageOn = false;
    private bool storageMoveOn = false;
    private bool directionWait = false;

    public bool Get_StorageMoveOn { get { return storageMoveOn; } private set { } }
    public List<OrderSheetEvent> Get_ReceiptList { get { return receiptList; } private set { } }

    private void Start()
    {
        storageBoxRect = storageBox.GetComponent<RectTransform>();
    }

    // 주문서의 버튼클릭시 실행
    public void OnClick_Copy()
    {
        SoundManager.Inst.PlaySFX("SelectOrder");

        GameObject clickObject = EventSystem.current.currentSelectedGameObject;

        directionWait = GameManager.Inst.Get_DirectionWait;
        if (directionWait) return;
        
        Texture juiceTexture = clickObject.transform.parent.GetComponent<RawImage>().texture;
        string juiceName = clickObject.transform.parent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

        GameManager.Inst.Get_JuiceData.JuiceIngredientSeting(juiceName.Trim());
        
        StartCoroutine(OrderListOff());
        StartCoroutine(PanelOff(juiceTexture, juiceName));
    }

    IEnumerator OrderListOff()
    {
        orderList.transform.parent = orderRope.transform;
        orderRope.GetComponent<RopeAnimationCurve>().Curve();

        yield return null;
    }

    IEnumerator PanelOff(Texture _texture, string _name)
    {
        while (panel.color.a >= 0)
        {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - 0.01f);
            title.color = new Color(title.color.r, title.color.g, title.color.b, title.color.a - 0.01f);
            yield return new WaitForSeconds(0.001f);
        }

        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
        yield return new WaitForSeconds(0.5f);
        panel.gameObject.SetActive(false);

        receip_Obj.gameObject.SetActive(true);
        receip_Obj.GetComponent<ReceipInfo>().Init(_texture, _name);
    }

    public void WrongTextImageOn(Transform _parent)
    {
        RawImage wt = Instantiate(wrongTextImage);

        wt.rectTransform.parent = _parent;
        wt.rectTransform.localPosition = new Vector2(0, 0);
    }

    public void StorageMove()
    {
        if (storageMoveOn) return;

        if (!storageOn)
            StartCoroutine(Storage_Open());
        else
            StartCoroutine(Storage_Close());
    }

    IEnumerator Storage_Open()
    {
        storageOn = true;
        storageMoveOn = true;
        storageBoxAni.gameObject.SetActive(false);

        // -340 580 910 
        while (storageBoxRect.anchoredPosition.x > storageBopxOpenPos_X)      // 변수로 변경
        {
            storageBoxRect.anchoredPosition = new Vector2(storageBoxRect.anchoredPosition.x - 10f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        storageMoveOn = false;
        yield return null;
    }

    IEnumerator Storage_Close()
    {
        storageOn = false;
        storageMoveOn = true;

        while (storageBoxRect.anchoredPosition.x < storageBopxClosePos_X)
        {
            storageBoxRect.anchoredPosition = new Vector2(storageBoxRect.anchoredPosition.x + 10f, 0);
            yield return new WaitForSeconds(0.01f);
        }

        storageBoxAni.gameObject.SetActive(true);
        storageMoveOn = false;
        yield return null;
    }

    public void FruitCupText(TextMeshProUGUI _text, int _num)
    {
        _text.text = _num.ToString();
    }

    public void OnFruitTransfromReset()
    {
        if (fruitHold == null) fruitHold = FindObjectOfType<FruitHold>();

        fruitHold.FruitListTransform();
    }

    public void GoodImageOn()
    {
        goodA.gameObject.SetActive(true);
    }

    public void NextImageOn()
    {
        goodAni.gameObject.SetActive(true);
    }

    public void ScreenCoverOn()
    {
        screenCover.gameObject.SetActive(true);
        RectTransform rt = screenCover.GetComponent<RectTransform>();

        Vector3 pos = new Vector3(0, 0, 0);
        Move_Time(pos, 0.2f, rt);
    }

    public void  Move_Time(Vector3 destination, float time, RectTransform rt)
    {
        float speed = Vector3.Distance(destination, rt.localPosition) / time;
        StartCoroutine(CO_Move(destination, speed, rt));
    }

    IEnumerator CO_Move(Vector3 destination, float speed, RectTransform rt)
    {
        while (rt.localPosition != destination)
        {
            rt.anchoredPosition = Vector2.MoveTowards(rt.localPosition, destination, speed * Time.deltaTime);
            yield return null;
        }
    }
}
