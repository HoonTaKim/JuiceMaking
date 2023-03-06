using UnityEngine;

public class MoveFruit : MonoBehaviour
{
    // 믹서기 게임 시작시 믹서기 내부로 떨어지는 과일들의 스크립트

    [SerializeField] private UiManager uiManager = null;
    private Rigidbody2D rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 활성화시 랜덤한 회전값 부여
    private void OnEnable()
    {
        this.transform.rotation = new Quaternion(0, 0, Random.Range(-30f, 30f), 0);
        this.GetComponent<SpriteRenderer>().sprite = GameManager.Inst.Get_SliceSpriteList[Random.Range(0, GameManager.Inst.Get_SliceSpriteList.Count)];
    }

    // 믹서기의 버튼 입력시 좌, 우측으로 이동 (흔들림 표현)
    private void Update()
    {
        if (uiManager.Get_MixButtonOn)
        {
            if (Random.Range(0, 2) == 0)
            {
                rb.AddForce(Vector3.left * 40f);
            }
            else
                rb.AddForce(Vector3.right * 40f);
        }
    }
}
