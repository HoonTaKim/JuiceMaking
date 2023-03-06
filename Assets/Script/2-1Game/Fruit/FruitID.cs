using UnityEngine;
using UnityEngine.UI;

public class FruitID : MonoBehaviour
{
    private RectTransform saveTr = null;    // 초기위치를 저장할 변수
    private Transform parent = null;        // 초기부모를 저장할 변수
    private Button button = null;           // 위치 초기화할때 사용할 변수

    public int id = 0;                      // 과일의 id
    [SerializeField] private bool caseIn = false;   // 컵 안으로 이동했는지 

    private FruitCase fruitCase = null;

    public RectTransform Get_SaveTr { get { return saveTr; } private set { } }
    public Transform Get_Parent { get { return parent; } private set { } }
    public bool Get_CaseIn { get { return caseIn; } private set { } }
    public int Get_Id { get { return id; } private set { } }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        //돌아갈 위치, 부모 저장
        saveTr = transform.parent.GetComponent<Image>().rectTransform;
        parent = transform.parent;
    }

    // 현재 과일이 들어간 컵의 정보 저장
    public void FruitCaseIn(FruitCase _fruitCase)
    {
        fruitCase = _fruitCase;
    }

    // 컵안에 들어갔을때 혹은 나갔을때 버튼 활성화, 비활성화
    public void ReSetTR_ButtonOn(bool _trriger)
    {
        button.enabled = _trriger;
    }

    //잘못된 과일 넣었을때 위치 초기화해주는 함수.
    public void OnClick_TransformReset()
    {
        this.transform.parent = parent.transform;
        this.transform.position = saveTr.position;
        fruitCase.CountChange(1);
        GameManager.Inst.CaseInFruit(-1);
        fruitCase = null;
        ReSetTR_ButtonOn(false);
    }
}
