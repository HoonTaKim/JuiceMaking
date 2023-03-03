using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTMP;
    [SerializeField] Text scoreText;
    [SerializeField] Text rewardStarText;

    //highscore일때만 보여주는거
    public GameObject HighScoreImage;


    public int rewardStar;


    private void Start()
    {
        SoundManager.Inst.StopBGM();
        GetComponent<Canvas>().worldCamera = Camera.main;
        GetComponent<Canvas>().sortingLayerName = "UI";
        GetComponent<Canvas>().sortingLayerName = "EndScene";
        scoreText.text = GameDatas.Inst.score.ToString();
        rewardStarText.text = GameDatas.Inst.reward.ToString();
    }

    public void LoadScene()
    {
        Destroy(FindObjectOfType<GameManager>().gameObject);
        SceneManager.LoadScene("1.StartSCene");
    }
}
