using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixImage2Move : MonoBehaviour
{
    private UiManager uiManager = null;
    [SerializeField] private GameObject downWall = null;

    private bool gameEnd = false;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        uiManager = FindObjectOfType<UiManager>();
    }

    private void Update()
    {
        if (uiManager.Get_MixButtonOn)
        {
            if (transform.position.y < -0.25f)
            {
                transform.position = new Vector3(0, transform.position.y + 0.016f, 0);
                downWall.transform.position = new Vector3(0, downWall.transform.position.y - 0.01f, 0);
            }
            else
            {
                transform.position = new Vector3(0, -0.25f, 0);
            }
        }
    }
}
