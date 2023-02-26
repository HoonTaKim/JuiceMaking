using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public void Click_Easy()
    {
        Debug.Log("Easy");
        GameDatas.Inst.difficulty = DIFFICULTY.EASY;
        SoundManager.Inst.PlaySFX("collect_item_04");
        GameDatas.Inst.SelectIdx(0);
        SceneManager.LoadScene("2-1.GameScene");
    }

    public void Click_Normal()
    {
        Debug.Log("Normal");
        GameDatas.Inst.difficulty = DIFFICULTY.NORMAL;
        SoundManager.Inst.PlaySFX("collect_item_04");
        GameDatas.Inst.SelectIdx(1);
        SceneManager.LoadScene("2-1.GameScene");
    }

    public void Click_Hard()
    {

        Debug.Log("Hard");
        GameDatas.Inst.difficulty = DIFFICULTY.HARD;
        SoundManager.Inst.PlaySFX("collect_item_04");
        GameDatas.Inst.SelectIdx(2);
        SceneManager.LoadScene("2-1.GameScene");
    }

    public void Click_Master()
    {
        Debug.Log("Master");
        GameDatas.Inst.difficulty = DIFFICULTY.MASTER;
        SoundManager.Inst.PlaySFX("collect_item_04");
        GameDatas.Inst.SelectIdx(3);
        SceneManager.LoadScene("2-1.GameScene");
    }
}