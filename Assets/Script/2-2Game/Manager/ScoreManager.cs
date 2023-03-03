using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int fruit_Score = 0;
    private int boom_Score = 0;

    private int score = 0;

    private void Start()
    {
        fruit_Score = GameDatas.Inst.fruitScore;
        boom_Score = GameDatas.Inst.boomScore;
    }

    public int PlusScore()
    {
        score += fruit_Score;
        GameManager.Inst.score = this.score;
        return score;
    }

    public int PlusComboScore(int _combo)
    {
        score += _combo;
        GameManager.Inst.score = this.score;
        return score;
    }

    public int MinusScore(int _boomScore)
    {
        if (score - boom_Score <= 0) score = 0;
        else score -= _boomScore;

        GameManager.Inst.score = this.score;

        return score;
    }
}
