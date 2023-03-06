using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrageManager : MonoBehaviour
{
    [Header("TouchInfo")]
    private Camera cam;         // 메인 카메라
    private Vector3 touchPos;   // 터치한 위치
    private Touch touch;        

    [SerializeField] SliserInfo pointer_Obj = null;
    SliserInfo pointer;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount == 0) return;
        TouchSystem();   
    }

    private void TouchSystem()
    {
        touch = Input.GetTouch(0);
        // 터치 카운트가 0보다 크고 터치된 상태에서 움직일때
        if (Input.touchCount > 0 && touch.phase == TouchPhase.Moved)
        {
            // 카메라를 기준으로 터치한 위치를 받아온다
            touchPos = cam.ScreenToWorldPoint(touch.position);
            touchPos = (Vector2)touchPos;
            // 포인터가 존재하지 않는다면 생성
            if (pointer == null)
                CreatePointer(touchPos);
            else
            {
                // 존재한다면 활성화 후 위치 이동
                pointer.gameObject.SetActive(true);
                pointer.gameObject.transform.position = touchPos;
                pointer.destination = touchPos;
            }
        }
        else
        {
            if (pointer == null) return;
            pointer.gameObject.SetActive(false);
        }
    }

    // 터치 포인터 생성
    private void CreatePointer(Vector2 pos)
    {
        pointer = Instantiate(pointer_Obj);
        pointer.transform.position = new Vector3(pos.x, pos.y, 0);
        pointer.gameObject.SetActive(true);
    }
}
