using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrageManager : MonoBehaviour
{
    [Header("TouchInfo")]
    private Camera cam;
    private Vector3 touchPos;
    private Touch touch;

    [SerializeField] GameObject pointer_Obj = null;
    GameObject pointer;

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
        if (Input.touchCount > 0 && touch.phase == TouchPhase.Moved)
        {
            touchPos = cam.ScreenToWorldPoint(touch.position);
            if (pointer == null)
                CreatePointer(touchPos);
            else
            {
                pointer.SetActive(true);
                pointer.transform.position = new Vector3(touchPos.x, touchPos.y, 0);
            }
        }
        else
        {
            if (pointer == null) return;
            pointer.SetActive(false);
        }
    }

    private void CreatePointer(Vector2 pos)
    {
        pointer = Instantiate(pointer_Obj);
        pointer.transform.position = new Vector3(pos.x, pos.y, 0);
        pointer.SetActive(true);
    }
}
