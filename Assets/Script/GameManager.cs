using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour//Singleton<GameManager>
{
    #region Singleton
    static GameManager inst;
    public static GameManager Inst
    {
        get
        {
            if (inst == null)
            {
                inst = FindObjectOfType<GameManager>();

                if (inst == null)
                {
                    inst = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }

            return inst;
        }
    }
    #endregion

    // Enum은 클래스 밖으로 빼야한다
    public enum SCENENUM { Scene1 = 1, Scene2 }
    public enum PAGE { PAGE1, PAGE2, PAGE3, PAGE4 }
    private SCENENUM sceneNum;

    private CommonUI commonUi = null;

    [Header("Scene 1")]
    private JuiceData juiceData = null;                             // 만들어질 주스의 정보 스크립트
    private SelectFruit_UiManager sUiManager;                       // 플로우1의 UIManager
    private int maxScore = 0;                                       // 과일을 넣을 수 있는 최대 개수
    private bool directionWait = false;                             // 시작 연출 시작, 종료 확인
    private int selectFruit = 0;                                    // 현재 컵에 들어간 과일의 개수
    public FruitCase leftCup;                                       // 왼쪽 컵의 정보
    public FruitCase rightCup;                                      // 오른쪽 컵의 정보
    public Image mixedColorImage;                                   // 색이 변경될 이미지
    public Transform leftFruit;                                     //왼쪽 컵에 넣은 과일들 가지고 있는거
    public Transform rightFruit;                                    //오른쪽 컵에 넣은 과일들 가지고 있는거
    private List<Sprite> fruitSpriteList = new List<Sprite>();      // 플로우2에서 사용할 선택한 과일들의 스프라이트 리스트
    private List<Sprite> sliceSpriteList = new List<Sprite>();      // 플로우2에서 사용할 선택환 과일들의 슬라이스 스프라이트 리스트
    public Sprite saveOrderImage = null;                            // 선택한 주문지 스프라이트 저장
    public string saveOrderName;                                    // 선택한 주문지의 이름 저장
    public Color saveJuiceColor;                                    // 완성된 주스의 색 저장

    [Header("Scene 2")]
    private UiManager uiManager;
    private ScoreManager scoreManager;
    private ComboManager comboManager;
    private SlicePooling slicePooling;
    public bool cutGameStart = false;
    public bool cutStart = false;
    private int fruitScore = 0;
    private float maxTime = 0f;
    public float curTime = 0f;
    private float boomTime = 0f;
    private int boomScore = 0;
    public int score = 0;
    private bool gameSet = false;
    private PAGE page;

    [Header("MixerGame")]
    private MixerStart mixerStart = null;
    public bool mixerEnd = false;

    #region Property

    public SelectFruit_UiManager Get_SelectFruitUiManager
    {
        get
        {
            if (sUiManager == null)
            {
                sUiManager = FindObjectOfType<SelectFruit_UiManager>();
            }

            return sUiManager;
        }
        private set { }
    }

    public UiManager Get_UiManager
    {
        get
        {
            if (uiManager == null)
                uiManager = FindObjectOfType<UiManager>();

            return uiManager;
        }
        private set { }
    }

    public JuiceData Get_JuiceData
    {
        get
        {
            if (juiceData == null)
                juiceData = FindObjectOfType<JuiceData>();

            return juiceData;
        }
        private set { }
    }

    public ComboManager Get_ComboManager
    {
        get
        {
            if (comboManager == null)
                comboManager = FindObjectOfType<ComboManager>();

            return comboManager;
        }
        private set { }
    }
    public ScoreManager Get_ScoreManager
    {
        get
        {
            if (scoreManager == null)
            {
                scoreManager = FindObjectOfType<ScoreManager>();
            }

            return scoreManager;
        }
        private set { }
    }

    public SlicePooling Get_SlicePooling
    {
        get
        {
            if (slicePooling == null)
            {
                slicePooling = FindObjectOfType<SlicePooling>();
            }

            return slicePooling;
        }
        private set { }
    }

    public MixerStart Get_MixerStart
    {
        get
        {
            if (mixerStart == null)
            {
                mixerStart = FindObjectOfType<MixerStart>();
            }
            return mixerStart;
        }
        private set { }
    }

    public PAGE Get_PAGE { get { return page; } private set { } }
    public bool Get_DirectionWait { get { return directionWait; } private set { } }
    public bool Get_GameSet { get { return gameSet; } private set { } }
    public bool Get_CutGameStart { get { return cutStart; } private set { } }
    public List<Sprite> Get_FruitSpriteList { get { return fruitSpriteList; } private set { } }
    public List<Sprite> Get_SliceSpriteList { get { return sliceSpriteList; } private set { } }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Init();
    }

    // 변수 초기 세팅 함수
    private void Init()
    {
        commonUi = FindObjectOfType<CommonUI>();

        if (SceneManager.GetActiveScene().name == "2-1.GameScene")
        {
            sceneNum = SCENENUM.Scene1;
        }
    }

    private void Update()
    {
        // 플로우2 진입시 사용될 타이머 
        if (sceneNum == SCENENUM.Scene2)
        {
            CutGameTime();
        }
    }

    #region Select Fruit Game

    // 플로우1의 시작연출 시작, 종료 알림
    public void Set_DirectionWait(bool _set)
    {
        directionWait = _set;
    }

    // 플로우1의 최대 스코어 저장
    public void Set_CupMaxScore(int _score)
    {
        maxScore = _score;
    }

    /// <summary>
    /// Fruit를 드래그 해서 넣으면 호출되는 함수?
    /// </summary>
    /// <param name="_score"></param>
    public void CaseInFruit(int _score) // _num으로 변경
    {
        if (selectFruit + _score <= 0) selectFruit = 0;
        else selectFruit += _score;

        Debug.Log("현재 점수는 : " + selectFruit);
        Debug.Log("점수 최대는 : " + maxScore);

        if (leftFruit.childCount == 0 && rightFruit.childCount == 0)
        {
            //둘다 비어있으면 하얀색
            mixedColorImage.color = Color.white;

        }
        else if (leftFruit.childCount == 0)
        {
            //왼쪽컵이 비어있으면 오른쪽걸로
            mixedColorImage.color = JuiceColor.singleColor(rightCup.color);
        }
        else if (rightFruit.childCount == 0)
        {
            //오른쪽이 비어있으면 왼쪽걸로
            mixedColorImage.color = JuiceColor.singleColor(leftCup.color);

        }
        else
        {
            //둘다 안비어있으면 섞인색으로
            mixedColorImage.color = JuiceColor.mixColor(leftCup.color, rightCup.color);
            saveJuiceColor = mixedColorImage.color;
        }

        if (selectFruit == maxScore)   //변수로변경
        {
            StartCoroutine(GameSetImage());
        }
    }

    // 플로우 1의 종료 연출 코루틴
    IEnumerator GameSetImage()
    {
        Get_SelectFruitUiManager.SelectEndAnimation();
        // 도장 소리
        SoundManager.Inst.PlaySFX("Push_Stamp");
        yield return new WaitForSeconds(1f);

        commonUi.gameObject.SetActive(false);
        Get_SelectFruitUiManager.ScreenCoverOn();
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("2-2.GameScene");

        yield return new WaitUntil(() => cutGameStart);

        CutGameStart();
    }

    #endregion

    #region Fruit Cut Game

    private void CutGameTime()
    {
        if (gameSet || !cutStart) return;


        if (curTime > 1)
        {
            curTime -= Time.deltaTime;
            Get_UiManager.TimeText(curTime);

            if (curTime < maxTime - GameDatas.Inst.timeSection_Min[1])
                page = PAGE.PAGE2;
            if (curTime < maxTime - GameDatas.Inst.timeSection_Min[2])
                page = PAGE.PAGE3;
            if (curTime < maxTime - GameDatas.Inst.timeSection_Min[3])
                page = PAGE.PAGE4;
        }
        else
        {
            curTime = 0;
            gameSet = true;
            GameSet();
        }
    }

    private void CutGameStart()
    {
        commonUi.gameObject.SetActive(true);

        page = PAGE.PAGE1;
        maxTime = GameDatas.Inst.cutGameTime;
        curTime = maxTime;
        fruitScore = GameDatas.Inst.fruitScore;
        boomTime = GameDatas.Inst.boomTime;
        boomScore = GameDatas.Inst.boomScore;
        score = 0;
        gameSet = false;
        sceneNum = SCENENUM.Scene2;

        Get_UiManager.ScreenCoverOn();
        Get_UiManager.StartCount();
    }

    public void SetCutGameScore()
    {
        score += fruitScore;
        Get_UiManager.ScoreText(score);
        Get_ComboManager.Combo(curTime);
    }

    // 폭탄 자를시 실행
    public void CutGameMinus()
    {
        if (curTime - boomTime > 0)
        {
            curTime -= boomTime;
        }
        else
        {
            curTime = 0;
        }

        if (score - boomScore > 0)
        {
            score -= boomScore;
        }
        else
        {
            score = 0;
        }


        Get_UiManager.MinusTime(boomScore, boomTime);
        Get_UiManager.ScoreText(score);
        Get_UiManager.TimeText(curTime);
    }

    public void ComboScore(int _combo)
    {
        score += _combo;
        Get_UiManager.ScoreText(score);
    }

    private void GameSet()
    {
        StartCoroutine(GameSetSeting());
    }

    IEnumerator GameSetSeting()
    {
        // 도장 소리
        SoundManager.Inst.PlaySFX("Push_Stamp");
        Get_UiManager.GameEndImageSetActive(true);
        yield return new WaitForSeconds(1.5f);
        Get_UiManager.GameEndImageSetActive(false);

        Get_UiManager.UiOff();
        GameDatas.Inst.RewardSeting(score);
        Get_UiManager.MixOn();
        Get_MixerStart.MixerGameOn();

    }
    #endregion

    public void MixerGameEnd()
    {
        Get_MixerStart.MixerGameEnd();
    }
}
