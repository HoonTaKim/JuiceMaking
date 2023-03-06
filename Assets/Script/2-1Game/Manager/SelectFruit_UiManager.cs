using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SelectFruit_UiManager : MonoBehaviour
{
    [SerializeField] private RawImage orderObj = null;          // 좌측 상단에서 활성화될 주문서 오브젝트 
    [SerializeField] private GameObject storageBox = null;      // 과일 보관함
    [SerializeField] private Image endParticle = null;          // 과일선택 게임 종료시 등장하는 종이폭죽 파티클
    [SerializeField] private Image goodAni = null;              // 과일선택 게임 종료시 등장하는 참 잘했어요 애니메이션
    [SerializeField] private RawImage screenCover = null;       // 과일선택 게임 종료시 등장하는 화면 암전시키는 커버
    [SerializeField] private RawImage wrongTextImage = null;    // 잘못된 컵에 넣을시 활성화되는 틀렸다는 이미지
    [SerializeField] private Image orderPanel = null;           // 게임 실행시 보이는 주문서 패널
    [SerializeField] private Image orderList = null;            // 주문서
    [SerializeField] private RawImage orderRope = null;         // 주문서의 부모 객체
    [SerializeField] private Image title = null;                // 주문서 패널의 타이틀
    [SerializeField] private Image storageBoxAni = null;        // 과일 보관함 가이드 애니메이션
    [SerializeField] private FruitHold fruitHold = null;

    private RectTransform storageBoxRect;                       // 과일 보관함의 RectTransform

    private float storageBopxOpenPos_X = 605f;                  // 과일 보관함이 열렸을때의 X좌표
    private float storageBopxClosePos_X = 910f;                 // 과일 보관함이 닫혔을때의 X좌표

    private bool storageOn = false;                             // 과일 보관함이 열린상태인지 닫힌상태인지 확인하는 bool값
    private bool storageMoveOn = false;                         // 과일 보관함이 이동하는중인지 확인하는 bool값

    public bool Get_StorageMoveOn { get { return storageMoveOn; } private set { } }

    private void Start()
    {
        storageBoxRect = storageBox.GetComponent<RectTransform>();
    }

    // 주문서의 버튼클릭시 실행
    public void OnClick_Copy()
    {
        if (GameManager.Inst.Get_DirectionWait) return;
        
        SoundManager.Inst.PlaySFX("SelectOrder");
        // 터치한 주문서를 담는 기능
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;

        // 좌측 상단에 생성될 주문서의 이미지와 텍스트 저장
        Texture juiceTexture = clickObject.transform.parent.GetComponent<RawImage>().texture;
        string juiceName = clickObject.transform.parent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

        GameManager.Inst.Get_JuiceData.JuiceIngredientSeting(juiceName.Trim());
        
        StartCoroutine(OrderListOff());
        StartCoroutine(PanelOff(juiceTexture, juiceName));
    }

    // 주문서들의 부모를 옮겨줌과 동시에 주문서와 로프를 애니메이션커브로 상단으로 이동시키는 함수
    IEnumerator OrderListOff()
    {
        orderList.transform.parent = orderRope.transform;
        orderRope.GetComponent<RopeAnimationCurve>().Curve();

        yield return null;
    }

    // 게임 시작시 보이는 주문서 화면을 알파값을 낮춰 비활성화 시키는 함수
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

        // 패널이 사라짐과 동시에 좌측 상단에 주문서 설치
        orderObj.gameObject.SetActive(true);
        orderObj.GetComponent<ReceipInfo>().Init(_texture, _name);
    }

    // 틀린 과일을 넣었을때 작동하는 텍스트 호출 함수
    public void WrongTextImageOn(Transform _parent)
    {
        RawImage wt = Instantiate(wrongTextImage);

        wt.rectTransform.parent = _parent;
        wt.rectTransform.localPosition = new Vector2(0, 0);
    }

    // 과일 바구니 버튼
    public void OnClick_StorageMove()
    {
        if (storageMoveOn) return;

        if (!storageOn)
            StartCoroutine(Storage_Open());
        else
            StartCoroutine(Storage_Close());
    }

    // 과일 바구니 여는 코루틴
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

    // 과일 바구니 닫는 코루틴
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

    // 과일이 담길 컵의 남은 개수를 알려주는 변경시켜주는 함수
    public void FruitCupText(TextMeshProUGUI _text, int _num)
    {
        _text.text = _num.ToString();
    }

    // 터치시 모든과일이 제자리로 돌려보내는 버튼
    public void OnClick_FruitTransfromReset()
    {
        if (fruitHold == null) fruitHold = FindObjectOfType<FruitHold>();

        fruitHold.FruitListTransform();
    }

    // 게임 종료 이미지 출력 함수
    public void SelectEndAnimation()
    {
        StartCoroutine(EndAnimation());
    }

    // 게임 종료 이미지 출력 코루틴
    IEnumerator EndAnimation()
    {
        endParticle.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        goodAni.gameObject.SetActive(true);
    }

    // 게임 종료시 화면 암전시키는 커버 실행 함수
    public void ScreenCoverOn()
    {
        screenCover.gameObject.SetActive(true);
        RectTransform rt = screenCover.GetComponent<RectTransform>();

        Vector3 pos = new Vector3(0, 0, 0);
        Move_Time(pos, 0.2f, rt);
    }

    // 이동
    public void  Move_Time(Vector3 destination, float time, RectTransform rt)
    {
        float speed = Vector3.Distance(destination, rt.localPosition) / time;
        StartCoroutine(CO_Move(destination, speed, rt));
    }

    // 이동 코루틴
    IEnumerator CO_Move(Vector3 destination, float speed, RectTransform rt)
    {
        while (rt.localPosition != destination)
        {
            rt.anchoredPosition = Vector2.MoveTowards(rt.localPosition, destination, speed * Time.deltaTime);
            yield return null;
        }
    }
}
