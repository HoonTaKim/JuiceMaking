using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    List<string> dataList = new List<string>();
    List<string> refreshTimeDataList = new List<string>();

    private void Start()
    {
        dataList = CSVLoad.LoadData("JuiceMaking_DataTable");
        refreshTimeDataList = CSVLoad.LoadData("JuiceMaking_RefreshTimeData");

        GameDatas.Inst.RefreshTimeData(refreshTimeDataList);
    }

    public void Click_Easy()
    {
        Debug.Log("쉬움 실행");
        Debug.Log("Easy");
        GameDatas.Inst.difficulty = DIFFICULTY.EASY;
        SoundManager.Inst.PlaySFX("collect_item_04");
        GameDatas.Inst.DataSeting(dataList, 1);
        SceneManager.LoadScene("2-1.GameScene");
    }

    public void Click_Normal()
    {
        Debug.Log("보통 실행");
        Debug.Log("Normal");
        GameDatas.Inst.difficulty = DIFFICULTY.NORMAL;
        SoundManager.Inst.PlaySFX("collect_item_04");
        GameDatas.Inst.DataSeting(dataList, 2);
        SceneManager.LoadScene("2-1.GameScene");
    }

    public void Click_Hard()
    {
        Debug.Log("어려움 실행");
        Debug.Log("Hard");
        GameDatas.Inst.difficulty = DIFFICULTY.HARD;
        SoundManager.Inst.PlaySFX("collect_item_04");
        GameDatas.Inst.DataSeting(dataList, 3);
        SceneManager.LoadScene("2-1.GameScene");
    }

    public void Click_Master()
    {
        Debug.Log("매우어려움 실행");
        Debug.Log("Master");
        GameDatas.Inst.difficulty = DIFFICULTY.MASTER;
        SoundManager.Inst.PlaySFX("collect_item_04");
        GameDatas.Inst.DataSeting(dataList, 4);
        SceneManager.LoadScene("2-1.GameScene");
    }
}