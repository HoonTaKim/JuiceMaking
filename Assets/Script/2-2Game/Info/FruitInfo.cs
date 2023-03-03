using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitInfo : MonoBehaviour
{
    private Rigidbody2D rb = null;
    private float saveTr = 0f;
    private float curTr = 0f;

    [SerializeField] private GameObject sliceEff = null;
    private List<Sprite> fruitSpriteList = new List<Sprite>();
    private SpriteRenderer color = null;
    private SpriteRenderer sprite = null;

    private bool upSlice = false;
    private bool cutOn = false;

    public bool Get_CutIn { get { return cutOn; } private set { } }
    public void Set_CutOn(bool _cut) => cutOn = _cut;
    private void Awake()
    {
        fruitSpriteList = GameManager.Inst.Get_FruitSpriteList;
        rb = GetComponent<Rigidbody2D>();
        color = GetComponent<SpriteRenderer>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        Init();
    }

    private void Update()
    {
        curTr = transform.position.y;
        if (curTr > saveTr) saveTr = curTr;
        if (curTr < saveTr)
        {
            rb.gravityScale = 1.4f;
        }
    }

    private void Init()
    {
        color.color = new Color() { r = 255, g = 255, b = 255, a = 100 };

        int idx = Random.Range(0, fruitSpriteList.Count);
        sprite.sprite = fruitSpriteList[idx];

        curTr = 0f;
        saveTr = -100f;
        rb.gravityScale = 1f;
        cutOn = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.Inst.Get_GameSet) return;

        if (cutOn)
        {
            if (collision.name != "Pointer(Clone)") return;

            if (collision != null && collision.name == "Pointer(Clone)")
            {
                Vector2 pos = collision.transform.position - this.transform.position;
                if (Mathf.Abs(pos.x) > Mathf.Abs(pos.y)) upSlice = true;
            }

            // 여기에 이팩트 집어넣으면댐
            SoundManager.Inst.PlaySFX("Sliceing01");

            GameObject eff = Instantiate(sliceEff, transform.position, transform.rotation);
            eff.transform.localScale = new Vector2(4, 4);
            eff.SetActive(true);
            Destroy(eff, 3f);

            if (upSlice)
            {
                FruitSeting(transform.position.x, transform.position.y + 0.5f, Quaternion.Euler(0, 0, -90f), Vector2.up, false);
                FruitSeting(transform.position.x, transform.position.y - 0.5f, Quaternion.Euler(0, 0, -90f), Vector2.down, true);
            }
            else
            {
                FruitSeting(transform.position.x - 0.5f, transform.position.y, Quaternion.identity, Vector2.left, true);
                FruitSeting(transform.position.x + 0.5f, transform.position.y, Quaternion.identity, Vector2.right, false);
            }

            upSlice = false;
            GameManager.Inst.SetCutGameScore();
            FruitPooling.Inst.In(this); 
        }
    }

    private void FruitSeting(float _x, float _y, Quaternion _r, Vector2 _d, bool _dir)
    {
        SliceInfo slice_obj = SlicePooling.Inst.Out(_x, _y, _r, this.GetComponent<SpriteRenderer>().sprite, _dir);
        slice_obj.GetComponent<Rigidbody2D>().AddForce(_d * 140f);
        slice_obj.GetComponent<Rigidbody2D>().AddTorque(Random.Range(25f, 55f));
    }
}
