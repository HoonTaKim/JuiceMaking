using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DIFFICULTY
{
    EASY,
    NORMAL,
    HARD,
    MASTER
}

[System.Serializable]
public class GameSeting
{
    public float cutGameTime = 0f;      // 자르기게임의 진행 시간
    public int reward_Min = 0;          // 최소 리워드
    public int reward_Max = 0;          // 최대 리워드
    public int reward_Terms = 0;        // 리워드 Max를 받기위한 조건
    public List<float> timeSection_Min = new List<float>();         // 구간을 나누는 최소시간
    public List<float> timeSection_Max = new List<float>();         // 구간을 나누는 최소시간

    [Header("과일 정보")]
    public List<int> creatFruit_MinCount = new List<int>();         // 각 구간별로 생성되는 최소개수
    public List<int> creatFruit_MaxCount = new List<int>();         // 각 구간별로 생성되는 최대개수
    public float fruitTimeSection_Min = 0f;         // 과일의 생성 최소 시간
    public float fruitTimeSection_Max = 0f;         // 과일의 최대 생성 시간
    public int fruitScore = 0;          // 과일의 점수

    [Header("폭탄 정보")]
    public int boomTimeSection = 0;         // 폭탄의 생성 주기
    public int creatBoomCount = 0;          // 폭탄의 생성 개수
    public int boomScore = 0;               // 폭탄의 점수
    public int boomTime = 0;                // 폭탄으로 깎이는 시간
    public List<int> boomAddCreatCount = new List<int>();       // 폭탄의 추가 생성 개수
}

public class GameDatas : MonoBehaviour
{
    public static GameDatas Inst;
    public DIFFICULTY difficulty;

    [SerializeField] private List<GameSeting> setingList = new List<GameSeting>();
    private GameSeting selectSeting = null;
    public int score = 0;
    public int reward = 0;

    public GameSeting Get_SelectSeting { get { return selectSeting; } private set { } }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Inst == null) Inst = this;
        else Destroy(gameObject);

        SoundManager.Inst.PlayBGM("TalkingCuteChiptune");
    }

    public void SelectIdx(int idx)
    {
        selectSeting = setingList[idx];
        //selectSeting.boomScore *= -1;
        //selectSeting.boomTime *= -1;

        if (selectSeting.boomTime == -1)
        {
            selectSeting.boomTime = 0;
        }

        SoundManager.Inst.StopBGM();
        SoundManager.Inst.PlayBGM("Hoppin_#039J");

        Debug.Log("게임의 시간 : " + selectSeting.cutGameTime);
        Debug.Log("리워드 최소 : " + selectSeting.reward_Min);
        Debug.Log("리워드 최대 : " + selectSeting.reward_Max);
        Debug.Log("리워드 기준 : " + selectSeting.reward_Terms);
        Debug.Log("시간 나누는 구간 최소 카운트 : " + selectSeting.timeSection_Min.Count);
        Debug.Log("시간 나누는 구간 최대 카운트 : " + selectSeting.timeSection_Max.Count);
        Debug.Log("과일의 최소 생성시간 : " + selectSeting.fruitTimeSection_Min);
        Debug.Log("과일의 최대 생성시간 : " + selectSeting.fruitTimeSection_Max);
        Debug.Log("과일의 점수 : " + selectSeting.fruitScore);
        Debug.Log("폭탄의 생성 주기 : " + selectSeting.boomTimeSection);
        Debug.Log("폭탄의 생성 개수 : " + selectSeting.creatBoomCount);
        Debug.Log("폭탄의 점수 : " + selectSeting.boomScore);
        Debug.Log("폭탄으로 깎이는 시간 : " + selectSeting.boomTime);
        Debug.Log("폭탄의 추가 생성 개수 : " + selectSeting.boomAddCreatCount.Count);
    }

    public void RewardSeting(int score)
    {
        Debug.Log("최종 스코어 : " + score);

        this.score = score;

        if (score >= selectSeting.reward_Terms)
        {
            reward = selectSeting.reward_Max;
        }
        else
        {
            reward = selectSeting.reward_Min;
        }

        Debug.Log("리워드 최소 : " + selectSeting.reward_Min);
    }
}
