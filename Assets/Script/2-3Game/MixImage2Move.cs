using UnityEngine;

public class MixImage2Move : MonoBehaviour
{
    private UiManager uiManager = null;
    [SerializeField] private GameObject downWall = null;
    public Vector3 movePos = Vector3.zero;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        uiManager = FindObjectOfType<UiManager>();
    }

    // 믹서기의 버튼이 눌렸을때 오브젝트가 점점 상승
    private void Update()
    {
        if (uiManager.Get_MixButtonOn)
        {
            if (transform.position.y < -0.25f)
            {
                transform.position = new Vector3(0, transform.position.y + (Time.deltaTime * 2f), 0);
                downWall.transform.position = new Vector3(0, downWall.transform.position.y - (Time.deltaTime * 2f), 0);
            }
            else
            {
                transform.position = new Vector3(0, -0.25f, 0);
            }
        }
    }
}
