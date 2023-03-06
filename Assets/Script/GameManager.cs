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

    // Enum�� Ŭ���� ������ �����Ѵ�
    public enum SCENENUM { Scene1 = 1, Scene2 }
    public enum PAGE { PAGE1, PAGE2, PAGE3, PAGE4 }
    private SCENENUM sceneNum;

    private CommonUI commonUi = null;

    [Header("Scene 1")]
    private JuiceData juiceData = null;                             // ������� �ֽ��� ���� ��ũ��Ʈ
    private SelectFruit_UiManager sUiManager;                       // �÷ο�1�� UIManager
    private int maxScore = 0;                                       // ������ ���� �� �ִ� �ִ� ����
    private bool directionWait = false;                             // ���� ���� ����, ���� Ȯ��
    private int selectFruit = 0;                                    // ���� �ſ� �� ������ ����
    public FruitCase leftCup;                                       // ���� ���� ����
    public FruitCase rightCup;                                      // ������ ���� ����
    public Image mixedColorImage;                                   // ���� ����� �̹���
    public Transform leftFruit;                                     //���� �ſ� ���� ���ϵ� ������ �ִ°�
    public Transform rightFruit;                                    //������ �ſ� ���� ���ϵ� ������ �ִ°�
    private List<Sprite> fruitSpriteList = new List<Sprite>();      // �÷ο�2���� ����� ������ ���ϵ��� ��������Ʈ ����Ʈ
    private List<Sprite> sliceSpriteList = new List<Sprite>();      // �÷ο�2���� ����� ����ȯ ���ϵ��� �����̽� ��������Ʈ ����Ʈ
    public Sprite saveOrderImage = null;                            // ������ �ֹ��� ��������Ʈ ����
    public string saveOrderName;                                    // ������ �ֹ����� �̸� ����
    public Color saveJuiceColor;                                    // �ϼ��� �ֽ��� �� ����

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

    // ���� �ʱ� ���� �Լ�
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
        // �÷ο�2 ���Խ� ���� Ÿ�̸� 
        if (sceneNum == SCENENUM.Scene2)
        {
            CutGameTime();
        }
    }

    #region Select Fruit Game

    // �÷ο�1�� ���ۿ��� ����, ���� �˸�
    public void Set_DirectionWait(bool _set)
    {
        directionWait = _set;
    }

    // �÷ο�1�� �ִ� ���ھ� ����
    public void Set_CupMaxScore(int _score)
    {
        maxScore = _score;
    }

    /// <summary>
    /// Fruit�� �巡�� �ؼ� ������ ȣ��Ǵ� �Լ�?
    /// </summary>
    /// <param name="_score"></param>
    public void CaseInFruit(int _score) // _num���� ����
    {
        if (selectFruit + _score <= 0) selectFruit = 0;
        else selectFruit += _score;

        Debug.Log("���� ������ : " + selectFruit);
        Debug.Log("���� �ִ�� : " + maxScore);

        if (leftFruit.childCount == 0 && rightFruit.childCount == 0)
        {
            //�Ѵ� ��������� �Ͼ��
            mixedColorImage.color = Color.white;

        }
        else if (leftFruit.childCount == 0)
        {
            //�������� ��������� �����ʰɷ�
            mixedColorImage.color = JuiceColor.singleColor(rightCup.color);
        }
        else if (rightFruit.childCount == 0)
        {
            //�������� ��������� ���ʰɷ�
            mixedColorImage.color = JuiceColor.singleColor(leftCup.color);

        }
        else
        {
            //�Ѵ� �Ⱥ�������� ���λ�����
            mixedColorImage.color = JuiceColor.mixColor(leftCup.color, rightCup.color);
            saveJuiceColor = mixedColorImage.color;
        }

        if (selectFruit == maxScore)   //�����κ���
        {
            StartCoroutine(GameSetImage());
        }
    }

    // �÷ο� 1�� ���� ���� �ڷ�ƾ
    IEnumerator GameSetImage()
    {
        Get_SelectFruitUiManager.SelectEndAnimation();
        // ���� �Ҹ�
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

    // ��ź �ڸ��� ����
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
        // ���� �Ҹ�
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
