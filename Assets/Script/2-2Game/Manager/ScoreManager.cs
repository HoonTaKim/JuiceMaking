using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int fruit_Score = 0;    // 과일을 통해 얻는 점수
    private int boom_Score = 0;     // 썩은 과일을 통해 잃는 점수

    private int score = 0;          // 현재 스코어

    private void Start()
    {
        fruit_Score = GameDatas.Inst.fruitScore;
        boom_Score = GameDatas.Inst.boomScore;
    }

    // 점수 상승
    public int PlusScore()
    {
        score += fruit_Score;
        GameManager.Inst.score = this.score;
        return score;
    }

    // 콤보를 통한 점수 상승
    public int PlusComboScore(int _combo)
    {
        score += _combo;
        GameManager.Inst.score = this.score;
        return score;
    }

    // 점수 하락
    public int MinusScore(int _boomScore)
    {
        if (score - boom_Score <= 0) score = 0;
        else score -= _boomScore;

        GameManager.Inst.score = this.score;

        return score;
    }
}
