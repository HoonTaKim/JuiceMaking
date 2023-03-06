using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DIFFICULTY
{
    EASY,
    NORMAL,
    HARD,
    MASTER
}

public class GameDatas : MonoBehaviour
{
    public static GameDatas Inst;
    public DIFFICULTY difficulty;

    public int score = 0;
    public int reward = 0;
    public TextMeshProUGUI test;

    [Header("게임 정보")]
    public float cutGameTime = 0f;                                  // 자르기게임의 진행 시간
    public int reward_Min = 0;                                      // 최소 리워드
    public int reward_Max = 0;                                      // 최대 리워드
    public int reward_Terms = 0;                                    // 리워드 Max를 받기위한 조건
    public List<float> timeSection_Min = new List<float>();         // 구간을 나누는 최소시간
    public List<float> timeSection_Max = new List<float>();         // 구간을 나누는 최소시간

    [Header("과일 정보")]
    public List<int> creatFruit_MinCount = new List<int>();         // 각 구간별로 생성되는 최소개수
    public List<int> creatFruit_MaxCount = new List<int>();         // 각 구간별로 생성되는 최대개수
    public float fruitTimeSection_Min = 0f;                         // 과일의 생성 최소 시간
    public float fruitTimeSection_Max = 0f;                         // 과일의 최대 생성 시간
    public int fruitScore = 0;                                      // 과일의 점수

    [Header("폭탄 정보")]
    public int boomTimeSection = 0;                                 // 폭탄의 생성 주기
    public int creatBoomCount = 0;                                  // 폭탄의 생성 개수
    public int boomScore = 0;                                       // 폭탄의 점수
    public int boomTime = 0;                                        // 폭탄으로 깎이는 시간
    public List<int> boomAddCreatCount = new List<int>();           // 폭탄의 추가 생성 개수

    [Header("휴식시간 정보")]
    public float refreshTime = 0f;                                  // 휴식시간
    public float refreshCycle = 0f;                                 // 휴식시간 주기

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Inst == null) Inst = this;
        else Destroy(gameObject);

        SoundManager.Inst.PlayBGM("TalkingCuteChiptune");
    }

    // 데이터 입력
    public void DataSeting(List<string> dataList, int idx)
    {
        string[] splitData = dataList[idx].Split(",");

        cutGameTime = float.Parse(splitData[2]);
        reward_Min = int.Parse(splitData[3]);
        reward_Max = int.Parse(splitData[4]);
        reward_Terms = int.Parse(splitData[5]);

        timeSection_Min.Add(float.Parse(splitData[6]));
        timeSection_Min.Add(float.Parse(splitData[8]));
        timeSection_Min.Add(float.Parse(splitData[10]));
        timeSection_Min.Add(float.Parse(splitData[12]));

        timeSection_Max.Add(float.Parse(splitData[7]));
        timeSection_Max.Add(float.Parse(splitData[9]));
        timeSection_Max.Add(float.Parse(splitData[11]));
        timeSection_Max.Add(float.Parse(splitData[13]));

        creatFruit_MinCount.Add(int.Parse(splitData[14]));
        creatFruit_MinCount.Add(int.Parse(splitData[16]));
        creatFruit_MinCount.Add(int.Parse(splitData[18]));
        creatFruit_MinCount.Add(int.Parse(splitData[20]));

        creatFruit_MaxCount.Add(int.Parse(splitData[15]));
        creatFruit_MaxCount.Add(int.Parse(splitData[17]));
        creatFruit_MaxCount.Add(int.Parse(splitData[19]));
        creatFruit_MaxCount.Add(int.Parse(splitData[21]));

        fruitTimeSection_Min = float.Parse(splitData[22]);
        fruitTimeSection_Max = float.Parse(splitData[23]);
        fruitScore = int.Parse(splitData[24]);

        boomTimeSection = int.Parse(splitData[25]);
        creatBoomCount = int.Parse(splitData[26]);
        boomScore = int.Parse(splitData[27]);
        boomTime = int.Parse(splitData[28]);

        boomAddCreatCount.Add(int.Parse(splitData[29]));
        boomAddCreatCount.Add(int.Parse(splitData[30]));
        boomAddCreatCount.Add(int.Parse(splitData[31]));
        boomAddCreatCount.Add(int.Parse(splitData[32]));
        boomAddCreatCount.Add(int.Parse(splitData[33]));


        //---------------------------------------------------------------------------------------
        SoundManager.Inst.StopBGM();
        SoundManager.Inst.PlayBGM("Hoppin_#039J");
    }

    // 플로우2의 과일 스폰 중지 시간 입력
    public void RefreshTimeData(List<string> dataList)
    {
        string[] data = dataList[0].Split(",");

        refreshCycle = float.Parse(data[2]);
        refreshTime = float.Parse(data[3]);
    }

    // 최종 게임 스코어 저장
    public void RewardSeting(int score)
    {
        Debug.Log("최종 스코어 : " + score);

        this.score = score;

        if (score >= reward_Terms)
        {
            reward = reward_Max;
        }
        else
        {
            reward = reward_Min;
        }
    }
}
