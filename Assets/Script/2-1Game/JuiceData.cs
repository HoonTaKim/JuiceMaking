using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class JuiceInfo
{
    public string juiceName;
    public Sprite juiceImage;

    public int fruitCount = 0;          // 과일의 전체 개수

    public int leftCupCount = 0;
    public Sprite leftColorCup_Image = null;
    public List<Sprite> leftCupFruit_Image = new List<Sprite>();

    public int rightCupCount = 0;
    public Sprite rightColorCup_Image = null;
    public List<Sprite> rightCupFruit_Image = new List<Sprite>();
}

public class JuiceData : MonoBehaviour
{
    [SerializeField] private Transform storageParent = null;
    [SerializeField] private Image fruitPrefab = null;
    [SerializeField] private List<Sprite> fruitTextuerList = null;
    [SerializeField] private List<Sprite> sliceTextuerList = null;
    [SerializeField] private List<JuiceInfo> fruitInfoList = null;
    private List<Sprite> wrongFruitInfoList = null;
    [SerializeField] private Image leftCup = null;
    [SerializeField] private Image rightCup = null;

    private List<Vector2> fruitPosList = new List<Vector2>();
    private List<Sprite> saveFruitTextuerList = new List<Sprite>();

    public List<Sprite> Get_SaveFruitTextuerList { get { return saveFruitTextuerList; } private set { } }

    private void Start()
    {
        wrongFruitInfoList = new List<Sprite>();

        fruitPosList.Add(new Vector2(-20, 230));
        fruitPosList.Add(new Vector2(130, 230));
        fruitPosList.Add(new Vector2(-20, 70));
        fruitPosList.Add(new Vector2(130, 70));
        fruitPosList.Add(new Vector2(-20, -100));
        fruitPosList.Add(new Vector2(130, -100));
        fruitPosList.Add(new Vector2(-20, -260));
        fruitPosList.Add(new Vector2(130, -260));
    }

    public void JuiceIngredientSeting(string _juiceName)
    {
        // 기준은 RectTransform의 Pos를 기준으로 작성함 + 부모를 설정해둔 상태로 진행함
        // 바구니의 너비는 1270 ~ 1490, 높이는 -330 ~ 330
        // 바구니의 너비는 -42 ~ 130, 높이는 -300 ~ 300
        // 바구니의 너비는 -20 ~ 130, 높이는 -290 ~ 280 
        // 왼쪽컵이 1 오른쪽 컵이 2 해당없음이 0
        
        JuiceInfo savejuiceInfo = null;

        for (int i = 0; i < fruitInfoList.Count; i++)
        {
            if (fruitInfoList[i].juiceName == _juiceName)
            {
                savejuiceInfo = fruitInfoList[i];
            }
        }

        TextuerListCheck(savejuiceInfo.leftCupFruit_Image);
        TextuerListCheck(savejuiceInfo.rightCupFruit_Image);

        GameManager.Inst.Set_MaxScore(savejuiceInfo.leftCupCount + savejuiceInfo.rightCupCount);

        leftCup.GetComponent<FruitCase>().SetCupMaxCount(savejuiceInfo.leftCupCount);
        rightCup.GetComponent<FruitCase>().SetCupMaxCount(savejuiceInfo.rightCupCount);

        //각 컵의 Color 및 Sprite 넣기
        leftCup.GetComponent<FruitCase>().color = getCupColorFromSprite(savejuiceInfo.leftColorCup_Image);
        leftCup.sprite = savejuiceInfo.leftColorCup_Image;
        leftCup.GetComponent<FruitCase>().isRight = false;

        rightCup.GetComponent<FruitCase>().color = getCupColorFromSprite(savejuiceInfo.rightColorCup_Image);
        rightCup.sprite = savejuiceInfo.rightColorCup_Image;
        rightCup.GetComponent<FruitCase>().isRight = true;

        AnswerFruintSeting(savejuiceInfo.leftCupCount, savejuiceInfo.leftCupFruit_Image, 1, true);  // 왼쪽 컵
        AnswerFruintSeting(savejuiceInfo.rightCupCount, savejuiceInfo.rightCupFruit_Image, 2, true);  // 오른쪽 컵
        AnswerFruintSeting(savejuiceInfo.fruitCount - (savejuiceInfo.leftCupCount + savejuiceInfo.rightCupCount), fruitTextuerList, 0, false);


        // 게임매니저로 전달
        SaveTexture(GameManager.Inst.Get_FruitTexterList, saveFruitTextuerList);
        SaveSlice(GameManager.Inst.Get_SliceTexterList, sliceTextuerList, saveFruitTextuerList, "1");
        SaveSlice(GameManager.Inst.Get_SliceTexterList, sliceTextuerList, saveFruitTextuerList, "2");

        Debug.Log("추가한 슬라이스의 개수는 : " + GameManager.Inst.Get_SliceTexterList.Count);

        GameManager.Inst.saveJuiceImage = savejuiceInfo.juiceImage;
        GameManager.Inst.saveJuiceName = savejuiceInfo.juiceName;
        Debug.Log("List : " + saveFruitTextuerList.Count);
    }
    //수정할수있으면해보자
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


    // 저장할 _list와 저장시킬 데이터를 가지고있는 _addList
    public void SaveTexture(List<Sprite> _list, List<Sprite> _addList)
    {
        for (int i = 0; i < _addList.Count; i++)
        {
            _list.Add(_addList[i]);
        }
    }

    public void SaveSlice(List<Sprite> _list, List<Sprite> _addList, List<Sprite> _chackList, string _number)
    {
        for (int i = 0; i < _addList.Count; i++)
        {
            for (int j = 0; j < _chackList.Count; j++)
            {
                if (_addList[i].name == _chackList[j].name + _number)
                {
                    _list.Add(_addList[i]);
                }
            }
        }
    }

    public void TextuerListCheck(List<Sprite> _selectList)
    {
        for (int i = 0; i < _selectList.Count; i++)
        {
            fruitTextuerList.Remove(_selectList[i]);
        }
    }

    // 랜덤한 위치에 과일 출현시키는 함수
    // 과일이 생성될 개수, 
    public void AnswerFruintSeting(int _fruitNum, List<Sprite> _fruitTexterList, int _id, bool _dirListOn)
    {
        Image fruit = null;
        int posIdx = 0;
        int fruitIdx = 0;
        
        for (int i = 0; i < _fruitNum; i++)
        {
            fruitIdx = Random.Range(0, _fruitTexterList.Count);
            fruit = Instantiate(fruitPrefab);
            fruit.transform.GetChild(0).GetComponent<Image>().sprite = _fruitTexterList[fruitIdx];
            fruit.transform.GetChild(0).GetComponent<FruitID>().id = _id;

            if (_dirListOn)
            {
                Debug.Log(_fruitTexterList[fruitIdx].name);
                saveFruitTextuerList.Add(_fruitTexterList[fruitIdx]);
            }
            _fruitTexterList.Remove(_fruitTexterList[fruitIdx]);

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
