using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixImageMove : MonoBehaviour
{
    private MixImage2Move moveObj = null;   // 이동하는 주스의 이미지
    private UiManager uiManager = null;     // UiManager
    private bool createOn = false;          // 한오브젝트당 하나씩 생성시킬 수 있도록 조절하는 bool값

    private void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
        moveObj = GameManager.Inst.Get_MixerStart.movePos.GetComponent<MixImage2Move>();
    }

    // 활성화시 게임 매니저에 저장되어있는 주스의 색으로 변경
    private void OnEnable()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        createOn = false;
    }

    private void Update()
    {
        // 우측으로 이동하는 주스의 이미지가 특정 위치에 도달했을때
        // 새로운 주스의 이미지를 활성화
        if (transform.position.x >= 12f && !createOn)
        {
            MixPooling.Inst.Out(transform.position.y);
            createOn = true;
        }

        // 우측으로 이동하는 주스의 이미지가 특정 위치에 도달했을때
        // 현재 오브젝트를 오브젝트 풀 안으로 이동
        if (transform.position.x > 15f) MixPooling.Inst.In(this);  // 풀안에 집어넣음

        if (uiManager.Get_MixButtonOn && transform.position.y < 0.5f)
        {
            // 버튼이 눌렸을때 그리고 일정 높이 이상으로 올라갔을때
            // 주스의 이미지가 좌측으로 이동하며 점점 상승
            transform.position = new Vector3(transform.position.x + (Time.deltaTime * 10f), transform.position.y + (Time.deltaTime * 5.5f), -2);
        }
        else
        {
            // 이외에 우측으로 이동하는 주스 이미지는 좌측으로 이동하며 점점 아래쪽으로 이동
            if (transform.position.y > moveObj.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x + (Time.deltaTime * 70f), transform.position.y - (Time.deltaTime * 7f), -2);
            }
            if (transform.position.y < moveObj.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, moveObj.transform.position.y, -2);
            }
        }
    }
}
