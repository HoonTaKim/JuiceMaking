using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JuiceData : MonoBehaviour
{
    private bool setComplete = false;

    [Header("과일 데이터")]
    [SerializeField] private int fruitCount = 0;          // 과일의 전체 개수
    [SerializeField] private Transform storageParent = null;            // 과일의 초기 위치
    [SerializeField] private Image fruitPrefab = null;                  // 과일 프리팹
    private List<Vector2> fruitPosList = new List<Vector2>();           // 과일 바구니안 과일이 놓일 위치 리스트
    private List<Sprite> saveFruit_List = new List<Sprite>();           // 모든 과일들의 스프라이트를 저장할 리스트
    public List<string> fruit_DataList = new List<string>();            // csv를 통해 받을 과일 스프라이트 정보 리스트
    public List<Sprite> leftCupFruit_Image = new List<Sprite>();
    public List<Sprite> rightCupFruit_Image = new List<Sprite>();

    [Header("컵 데이터")]
    [SerializeField] private Image leftCup = null;                      // 왼쪽 컵 정보
    [SerializeField] private Image rightCup = null;                     // 오른쪽 컵 정보
    public List<string> cup_DataList = new List<string>();

    [Header("주문서 데이터")]
    [SerializeField] private List<RawImage> orderSheetList = new List<RawImage>();      // 주문서 정보
    public List<string> orderSheet_DataList = new List<string>();                       // csv를 통해 받을 주문서 정보 리스트


    int leftCup_Count = 0;
    int leftCup_Id = 0;

    int rightCup_Count = 0;
    int rightCup_Id = 0;

    #region 프로퍼티
    public List<Sprite> Get_SaveFruit_List { get { return saveFruit_List; } private set { } }
    #endregion

    private void Start()
    {
        orderSheet_DataList = CSVLoad.LoadData("JuiceMaking_OrderDataSheet");
        fruit_DataList = CSVLoad.LoadData("JuiceMaking_FruitData");
        cup_DataList = CSVLoad.LoadData("JuiceMaking_CupData");

        NullDataDel(orderSheet_DataList);
        NullDataDel(fruit_DataList);
        NullDataDel(cup_DataList);

        OrderSheetSeting();

        // 과일 위치 저장
        fruitPosList.Add(new Vector2(-20, 230));
        fruitPosList.Add(new Vector2(130, 230));
        fruitPosList.Add(new Vector2(-20, 70));
        fruitPosList.Add(new Vector2(130, 70));
        fruitPosList.Add(new Vector2(-20, -100));
        fruitPosList.Add(new Vector2(130, -100));
        fruitPosList.Add(new Vector2(-20, -260));
        fruitPosList.Add(new Vector2(130, -260));
    }

    public void OrderSheetSeting()
    {
        string[] data;

        while (orderSheetList.Count != orderSheet_DataList.Count)
        {
            orderSheet_DataList.Remove(orderSheet_DataList[Random.Range(0, orderSheet_DataList.Count)]);
        }

        for (int i = 0; i < orderSheetList.Count; i++)
        {
            data = orderSheet_DataList[i].Split(",");
            orderSheetList[i].GetComponent<RawImage>().texture = Resources.Load<Texture>("SetJuiceImage/" + data[7]);
            orderSheetList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data[1];
        }
    }

    // 주문서의 적힌 주스의 이름을 통해 과일과 컵의 정보를 입력하는 함수
    public void JuiceIngredientSeting(string _juiceName)
    {

        if (setComplete) return;

        // 기준은 RectTransform의 Pos를 기준으로 작성함 + 부모를 설정해둔 상태로 진행함
        // 바구니의 너비는 1270 ~ 1490, 높이는 -330 ~ 330
        // 바구니의 너비는 -42 ~ 130, 높이는 -300 ~ 300
        // 바구니의 너비는 -20 ~ 130, 높이는 -290 ~ 280 
        // 왼쪽컵이 1 오른쪽 컵이 2 해당없음이 0

        string[] orderData = new string[0];

        for (int i = 0; i < orderSheet_DataList.Count; i++)
        {
            orderData = orderSheet_DataList[i].Split(",");
            if (_juiceName == orderData[1])
            {
                break;
            }
        }

        GameManager.Inst.saveOrderImage = Resources.Load<Sprite>("JuiceImage/" + orderData[7] + "_Result");
        GameManager.Inst.saveOrderName = orderData[1];

        FruitCase leftCase = leftCup.GetComponent<FruitCase>();
        FruitCase rightCase = rightCup.GetComponent<FruitCase>();

        leftCup_Count = int.Parse(orderData[3]);
        leftCup_Id = int.Parse(orderData[2]);

        rightCup_Count = int.Parse(orderData[5]);
        rightCup_Id = int.Parse(orderData[4]);

        AddFruit(orderData, 2, leftCupFruit_Image, true);
        AddFruit(orderData, 4, rightCupFruit_Image, false);

        // 게임 스코어를 왼쪽과 오른쪽 컵의 개수를 더해 계산
        GameManager.Inst.Set_CupMaxScore(leftCup_Count + rightCup_Count);

        // 왼쪽컵과 오른쪽컵의 입력가능한 개수 입력
        leftCase.SetCupMaxCount(leftCup_Count);
        rightCase.SetCupMaxCount(rightCup_Count);

        // 왼쪽컵과 오른쪽컵에 ID값 부여
        leftCase.case_Id = leftCup_Id;
        rightCase.case_Id = rightCup_Id;

        //각 컵의 Color 및 Sprite 넣기
        leftCup.sprite = CupSpriteSeting(leftCase.case_Id);
        leftCase.color = getCupColorFromSprite(leftCup.sprite);
        leftCase.isRight = false;

        rightCup.sprite =  CupSpriteSeting(rightCase.case_Id);
        rightCase.color = getCupColorFromSprite(rightCup.sprite);
        rightCase.isRight = true;

        // 과일 생성
        AnswerFruintSeting(leftCup_Count, leftCupFruit_Image, leftCup_Id, true);                        // 왼쪽 컵에 들어갈 과일들
        AnswerFruintSeting(rightCup_Count, rightCupFruit_Image, rightCup_Id, true);                     // 오른쪽 컵 들어갈 과일들
        AnswerFruintSeting(fruitCount - (leftCup_Count + rightCup_Count), saveFruit_List, 0, false);    // 이외의 과일들

        setComplete = true;
    }

    public void NullDataDel(List<string> dataList)
    {
        for (int i = dataList.Count - 1; i > 0; i--)
        {
            if (dataList[i] == null)
            {
                dataList.Remove(dataList[i]);
            }
            else break;
        }
    }

    public void AddFruit(string[] orderData, int idx, List<Sprite> spriteList, bool chack)
    {
        string[] tempArr;
        for (int i = 0; i < fruit_DataList.Count; i++)
        {
            tempArr = fruit_DataList[i].Split(",");

            if (chack)
            {
                saveFruit_List.Add(Resources.Load<Sprite>("Fruit/" + tempArr[1]));
            }

            if (orderData[idx] == tempArr[2])
            {
                spriteList.Add(Resources.Load<Sprite>("Fruit/" + tempArr[1]));
                saveFruit_List.Remove(Resources.Load<Sprite>("Fruit/" + tempArr[1]));
            }
        }
    }

    // 컵 색깔 입력
    FRUITCOLOR getCupColorFromSprite(Sprite sp)
    {
        FRUITCOLOR color = FRUITCOLOR.DEFAULT;

        switch (sp.name)
        {
            case "Cup_Blue":
                color = FRUITCOLOR.BLUE;
                break;
            case "Cup_Green":
                color = FRUITCOLOR.GREEN;
                break;
            case "Cup_Orange":
                color = FRUITCOLOR.ORANGE;
                break;
            case "Cup_Puple":
                color = FRUITCOLOR.PURPLE;
                break;
            case "Cup_Red":
                color = FRUITCOLOR.RED;
                break;
            case "Cup_White":
                color = FRUITCOLOR.WHITE;
                break;
            case "Cup_Yellow":
                color = FRUITCOLOR.YELLOW;
                break;
        }

        if(color == FRUITCOLOR.DEFAULT)
        {
            Debug.Log("뭔가잘못됬어");
        }    

        return color;
    }

    // 컵 스프라이트 입력
    public Sprite CupSpriteSeting(int id)
    {
        string[] data = new string[0];
        Sprite sp = null;

        switch (id)
        {
            case 1:
                // 빨강
                data = cup_DataList[0].Split(",");
                break;
            case 2:
                // 주황
                data = cup_DataList[1].Split(",");
                break;
            case 3:
                // 노랑
                data = cup_DataList[2].Split(",");
                break;
            case 5:
                // 초록
                data = cup_DataList[4].Split(",");
                break;
            case 7:
                // 파랑
                data = cup_DataList[3].Split(",");
                break;
            case 8:
                // 보라
                data = cup_DataList[5].Split(",");
                break;
            case 10:
                // 하양
                data = cup_DataList[6].Split(",");
                break;
        }

        sp = Resources.Load<Sprite>("ColorCup/" + data[1]);

        return sp;
    }

    // 랜덤한 위치에 과일 출현시키는 함수
    // 생성할 과일의 개수, 생성시킬 과일의 스프라이트 리스트, 컵의 ID, 컵안에 들어가는과일인지 아닌지
    public void AnswerFruintSeting(int _fruitNum, List<Sprite> _fruitSpriteList, int _id, bool _dirListOn)
    {
        Image fruit = null;
        int posIdx = 0;
        int fruitIdx = 0;

        for (int i = 0; i < _fruitNum; i++)
        {
            // 컵의 id값을 통해 과일을 가져오는 기능
            fruitIdx = Random.Range(0, _fruitSpriteList.Count);
            fruit = Instantiate(fruitPrefab);
            fruit.transform.GetChild(0).GetComponent<Image>().sprite = _fruitSpriteList[fruitIdx];
            fruit.transform.GetChild(0).GetComponent<FruitID>().id = _id;

            if (_dirListOn)
            {
                GameManager.Inst.Get_FruitSpriteList.Add(_fruitSpriteList[fruitIdx]);
                GameManager.Inst.Get_SliceSpriteList.Add(Resources.Load<Sprite>("SliceFruit/" + _fruitSpriteList[fruitIdx].name + "1"));
                GameManager.Inst.Get_SliceSpriteList.Add(Resources.Load<Sprite>("SliceFruit/" + _fruitSpriteList[fruitIdx].name + "2"));
            }
            _fruitSpriteList.Remove(_fruitSpriteList[fruitIdx]);

            if (fruitPosList.Count == 1)
                posIdx = 0;
            else
                posIdx = Random.Range(0, fruitPosList.Count - 1);

            fruit.rectTransform.parent = storageParent;
            fruit.rectTransform.localPosition = fruitPosList[posIdx];
            fruit.rectTransform.localPosition = new Vector2(fruit.rectTransform.localPosition.x + Random.Range(-35f, 35f), fruit.rectTransform.localPosition.y + Random.Range(-35f, 35f));
            fruitPosList.Remove(fruitPosList[posIdx]);
        }
    }
}
