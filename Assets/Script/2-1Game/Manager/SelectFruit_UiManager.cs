using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SelectFruit_UiManager : MonoBehaviour
{
    [SerializeField] private RawImage orderObj = null;          // ���� ��ܿ��� Ȱ��ȭ�� �ֹ��� ������Ʈ 
    [SerializeField] private GameObject storageBox = null;      // ���� ������
    [SerializeField] private Image endParticle = null;          // ���ϼ��� ���� ����� �����ϴ� �������� ��ƼŬ
    [SerializeField] private Image goodAni = null;              // ���ϼ��� ���� ����� �����ϴ� �� ���߾�� �ִϸ��̼�
    [SerializeField] private RawImage screenCover = null;       // ���ϼ��� ���� ����� �����ϴ� ȭ�� ������Ű�� Ŀ��
    [SerializeField] private RawImage wrongTextImage = null;    // �߸��� �ſ� ������ Ȱ��ȭ�Ǵ� Ʋ�ȴٴ� �̹���
    [SerializeField] private Image orderPanel = null;           // ���� ����� ���̴� �ֹ��� �г�
    [SerializeField] private Image orderList = null;            // �ֹ���
    [SerializeField] private RawImage orderRope = null;         // �ֹ����� �θ� ��ü
    [SerializeField] private Image title = null;                // �ֹ��� �г��� Ÿ��Ʋ
    [SerializeField] private Image storageBoxAni = null;        // ���� ������ ���̵� �ִϸ��̼�
    [SerializeField] private FruitHold fruitHold = null;

    private RectTransform storageBoxRect;                       // ���� �������� RectTransform

    private float storageBopxOpenPos_X = 605f;                  // ���� �������� ���������� X��ǥ
    private float storageBopxClosePos_X = 910f;                 // ���� �������� ���������� X��ǥ

    private bool storageOn = false;                             // ���� �������� ������������ ������������ Ȯ���ϴ� bool��
    private bool storageMoveOn = false;                         // ���� �������� �̵��ϴ������� Ȯ���ϴ� bool��

    public bool Get_StorageMoveOn { get { return storageMoveOn; } private set { } }

    private void Start()
    {
        storageBoxRect = storageBox.GetComponent<RectTransform>();
    }

    // �ֹ����� ��ưŬ���� ����
    public void OnClick_Copy()
    {
        if (GameManager.Inst.Get_DirectionWait) return;
        
        SoundManager.Inst.PlaySFX("SelectOrder");
        // ��ġ�� �ֹ����� ��� ���
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;

        // ���� ��ܿ� ������ �ֹ����� �̹����� �ؽ�Ʈ ����
        Texture juiceTexture = clickObject.transform.parent.GetComponent<RawImage>().texture;
        string juiceName = clickObject.transform.parent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

        GameManager.Inst.Get_JuiceData.JuiceIngredientSeting(juiceName.Trim());
        
        StartCoroutine(OrderListOff());
        StartCoroutine(PanelOff(juiceTexture, juiceName));
    }

    // �ֹ������� �θ� �Ű��ܰ� ���ÿ� �ֹ����� ������ �ִϸ��̼�Ŀ��� ������� �̵���Ű�� �Լ�
    IEnumerator OrderListOff()
    {
        orderList.transform.parent = orderRope.transform;
        orderRope.GetComponent<RopeAnimationCurve>().Curve();

        yield return null;
    }

    // ���� ���۽� ���̴� �ֹ��� ȭ���� ���İ��� ���� ��Ȱ��ȭ ��Ű�� �Լ�
    WaitForSeconds wait001 = new WaitForSeconds(0.001f);
    IEnumerator PanelOff(Texture _texture, string _name)
    {
        while (orderPanel.color.a >= 0)
        {
            orderPanel.color = new Color(orderPanel.color.r, orderPanel.color.g, orderPanel.color.b, orderPanel.color.a - Time.deltaTime);
            title.color = new Color(title.color.r, title.color.g, title.color.b, title.color.a - Time.deltaTime);
            yield return null;
        }

        orderPanel.color = new Color(orderPanel.color.r, orderPanel.color.g, orderPanel.color.b, 0);
        yield return new WaitForSeconds(0.5f);
        orderPanel.gameObject.SetActive(false);

        // �г��� ������� ���ÿ� ���� ��ܿ� �ֹ��� ��ġ
        orderObj.gameObject.SetActive(true);
        orderObj.GetComponent<ReceipInfo>().Init(_texture, _name);
    }

    // Ʋ�� ������ �־����� �۵��ϴ� �ؽ�Ʈ ȣ�� �Լ�
    public void WrongTextImageOn(Transform _parent)
    {
        RawImage wt = Instantiate(wrongTextImage);

        wt.rectTransform.parent = _parent;
        wt.rectTransform.localPosition = new Vector2(0, 0);
    }

    // ���� �ٱ��� ��ư
    public void OnClick_StorageMove()
    {
        if (storageMoveOn) return;

        if (!storageOn)
            StartCoroutine(Storage_Open());
        else
            StartCoroutine(Storage_Close());
    }

    // ���� �ٱ��� ���� �ڷ�ƾ
    IEnumerator Storage_Open()
    {
        storageOn = true;
        storageMoveOn = true;
        storageBoxAni.gameObject.SetActive(false);

        // -340 580 910 
        while (storageBoxRect.anchoredPosition.x > storageBopxOpenPos_X)
        {
            storageBoxRect.anchoredPosition = new Vector2(storageBoxRect.anchoredPosition.x - (Time.deltaTime * 1000), 0);
            yield return null;
        }
        storageMoveOn = false;
    }

    // ���� �ٱ��� �ݴ� �ڷ�ƾ
    IEnumerator Storage_Close()
    {
        storageOn = false;
        storageMoveOn = true;

        while (storageBoxRect.anchoredPosition.x < storageBopxClosePos_X)
        {
            storageBoxRect.anchoredPosition = new Vector2(storageBoxRect.anchoredPosition.x + (Time.deltaTime * 1000), 0);
            yield return null;
        }

        storageBoxAni.gameObject.SetActive(true);
        storageMoveOn = false;
        yield return null;
    }

    // ������ ��� ���� ���� ������ �˷��ִ� ��������ִ� �Լ�
    public void FruitCupText(TextMeshProUGUI _text, int _num)
    {
        _text.text = _num.ToString();
    }

    // ��ġ�� �������� ���ڸ��� ���������� ��ư
    public void OnClick_FruitTransfromReset()
    {
        if (fruitHold == null) fruitHold = FindObjectOfType<FruitHold>();

        fruitHold.FruitListTransform();
    }

    // ���� ���� �̹��� ��� �Լ�
    public void SelectEndAnimation()
    {
        StartCoroutine(EndAnimation());
    }

    // ���� ���� �̹��� ��� �ڷ�ƾ
    IEnumerator EndAnimation()
    {
        endParticle.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        goodAni.gameObject.SetActive(true);
    }

    // ���� ����� ȭ�� ������Ű�� Ŀ�� ���� �Լ�
    public void ScreenCoverOn()
    {
        screenCover.gameObject.SetActive(true);
        RectTransform rt = screenCover.GetComponent<RectTransform>();

        Vector3 pos = new Vector3(0, 0, 0);
        Move_Time(pos, 0.2f, rt);
    }

    // �̵�
    public void  Move_Time(Vector3 destination, float time, RectTransform rt)
    {
        float speed = Vector3.Distance(destination, rt.localPosition) / time;
        StartCoroutine(CO_Move(destination, speed, rt));
    }

    // �̵� �ڷ�ƾ
    IEnumerator CO_Move(Vector3 destination, float speed, RectTransform rt)
    {
        while (rt.localPosition != destination)
        {
            rt.anchoredPosition = Vector2.MoveTowards(rt.localPosition, destination, speed * Time.deltaTime);
            yield return null;
        }
    }
}
