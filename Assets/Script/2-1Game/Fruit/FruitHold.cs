using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FruitHold : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private List<Image> fruitList = new List<Image>();          // ID스크립트에 추가

    [SerializeField] SelectFruit_UiManager uiManager = null;    
    private FruitCase fruitCase = null;                         // 선택한 컵을 담을 변수

    [Header("CanvasRayCast")]
    [SerializeField] private Canvas canvas = null;
    private GraphicRaycaster raycast;

    [Header("컵 변수")]
    //[SerializeField] private JuiceColor jc = null;              // 컵에 들어간 과일에 따라 바뀌는 색을 저장할 변수
    [SerializeField] private FruitCase leftCase = null;         // 왼쪽 컵
    [SerializeField] private FruitCase rightCase = null;        // 오른쪽 컵

    [Header("과일 변수")]
    [SerializeField] private Transform moveParent = null;       // 과일이 이동하는동안 들어갈 부모 오브젝트
    private Image fruitSelect = null;                           // 과일의 이미지를 저장할 변수
    private RectTransform fruitRt = null;                       // 과일의 위치를 저장할 변수
    private Transform fruitParent = null;                       // 과일의 부모 저장시킬 변수


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        raycast = canvas.GetComponent<GraphicRaycaster>();
    }

    // 드래그시 처음 한번 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (uiManager.Get_StorageMoveOn) return;

        SoundManager.Inst.PlaySFX("Putting fruits.");

        // Ui RayCast 실행
        List<RaycastResult> results = new List<RaycastResult>();
        Debug.DrawRay(eventData.position, Vector3.forward, Color.red, 500f);
        raycast.Raycast(eventData, results);

        // RayCast로 검출된것들중 FruitID, FruitCase 찾기
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.TryGetComponent<FruitID>(out FruitID fruit))
            {
                // 존재한다면 기존의 위치와 부모를 저장
                fruitSelect = fruit.gameObject.GetComponent<Image>();
                fruitRt = fruit.Get_SaveTr;
                fruitParent = fruit.Get_Parent;
            }
            if (results[i].gameObject.TryGetComponent<FruitCase>(out FruitCase cup))
            {
                fruitCase = cup;
            }
        }

        if (fruitCase != null) fruitCase.Set_CaseOn(false);
        if (fruitSelect == null) return;
        else
        {
            StartCoroutine(SelectCatchScale());
        }
        if (fruitSelect.GetComponent<FruitID>().Get_CaseIn) fruitSelect = null;
    }

    // 모든과일들 위치초기화
    public void FruitListTransform()
    {
        if (fruitList.Count <= 0) return;

        for (int i = 0; i < fruitList.Count; i++)
        {
            // 반복문을 통해 과일들을 원래의 위치로 이동
            FruitID fruitId = fruitList[i].GetComponent<FruitID>();
            fruitId.ReSetTR_ButtonOn(false);
            fruitList[i].rectTransform.position = new Vector3(fruitId.Get_SaveTr.position.x, fruitId.Get_SaveTr.position.y, fruitId.Get_SaveTr.position.z);
            fruitList[i].transform.parent = fruitId.Get_Parent;
            GameManager.Inst.CaseInFruit(-1);
        }

        leftCase.CountChange(2);
        rightCase.CountChange(2);
        GameManager.Inst.CaseInFruit(-100);

        fruitList.Clear();
    }

    // 드래그하는 동안 마우스 커서가 움직일 때마다 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (fruitSelect == null) return;

        // 드래그동안 터치 포인트를 따라가며 부모는 moveParent로 이동
        fruitSelect.rectTransform.position = eventData.position;
        fruitSelect.transform.SetParent(moveParent);
    }

    /// <summary>
    /// 특정 과일을 선택하고 있는 동안 해당 과일의 localScale을 커지게 만들어주는코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator SelectCatchScale()
    {
        while (fruitSelect.rectTransform.localScale.x <= 1.5f)
        {
            fruitSelect.rectTransform.localScale = new Vector2(fruitSelect.rectTransform.localScale.x + 0.1f, fruitSelect.rectTransform.localScale.y + 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        fruitSelect.rectTransform.localScale = new Vector2(1.5f, 1.5f);
    }

    // 드래그 종료시에 한번 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        if (fruitSelect == null) return;
        Image hitCase = null;
        ScrollRect hitCaseChild = null;

        fruitSelect.rectTransform.localScale = new Vector2(1f, 1f);

        List<RaycastResult> results = new List<RaycastResult>();
        raycast.Raycast(eventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.TryGetComponent<FruitCase>(out FruitCase obj))
            {
                hitCase = obj.gameObject.GetComponent<Image>();
                hitCaseChild = obj.gameObject.transform.GetChild(0).GetComponent<ScrollRect>();
            }
        }

        if (hitCase == null)
        {
            fruitSelect.transform.SetParent(fruitParent);
            fruitSelect.rectTransform.position = fruitRt.position;
        }


        if (hitCase != null)
        {
            SoundManager.Inst.PlaySFX("Putting fruits.");

            //마우스를 땐 시점에 select에 들고 있는 과일을 들고 있었고, 과일 바구니에 넣어줌.
            FruitID fruit = fruitSelect.GetComponent<FruitID>();
            FruitCase fruitCase = hitCase.GetComponent<FruitCase>();

            //fruit.get_id를 통해 이 컵에 이 과일이 들어갈 수 있는지 확인 하고, 맞으면 이 블럭으로 들어감
            if (fruit.Get_Id == fruitCase.Get_CaseId && fruitCase.Get_CupCount > 0)
            {
                SoundManager.Inst.PlaySFX("AnswerFruit");

                fruitSelect.transform.SetParent(hitCaseChild.content.transform);
                fruitSelect.rectTransform.position = hitCaseChild.content.transform.position;
                fruit.FruitCaseIn(fruitCase);

                fruitCase.CountChange(-1);
                fruit.ReSetTR_ButtonOn(true);

                fruitList.Add(fruitSelect);
                GameManager.Inst.CaseInFruit(1);
            }
            else
            {
                SoundManager.Inst.PlaySFX("WrongFruit");

                fruitSelect.transform.SetParent(fruitParent);
                fruitSelect.rectTransform.position = fruitRt.position;
                GameManager.Inst.Get_SelectFruitUiManager.WrongTextImageOn(fruitCase.transform);
            }
        }
        else
        {
            fruitSelect.transform.SetParent(fruitParent);
            fruitSelect.rectTransform.position = fruitRt.position;
        }

        fruitSelect = null;
    }
}