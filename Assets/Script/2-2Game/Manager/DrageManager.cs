using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrageManager : MonoBehaviour
{
    [Header("TouchInfo")]
    private Camera cam;         // ���� ī�޶�
    private Vector3 touchPos;   // ��ġ�� ��ġ
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
        // ��ġ ī��Ʈ�� 0���� ũ�� ��ġ�� ���¿��� �����϶�
        if (Input.touchCount > 0 && touch.phase == TouchPhase.Moved)
        {
            // ī�޶� �������� ��ġ�� ��ġ�� �޾ƿ´�
            touchPos = cam.ScreenToWorldPoint(touch.position);
            touchPos = (Vector2)touchPos;
            // �����Ͱ� �������� �ʴ´ٸ� ����
            if (pointer == null)
                CreatePointer(touchPos);
            else
            {
                // �����Ѵٸ� Ȱ��ȭ �� ��ġ �̵�
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

    // ��ġ ������ ����
    private void CreatePointer(Vector2 pos)
    {
        pointer = Instantiate(pointer_Obj);
        pointer.transform.position = new Vector3(pos.x, pos.y, 0);
        pointer.gameObject.SetActive(true);
    }
}
