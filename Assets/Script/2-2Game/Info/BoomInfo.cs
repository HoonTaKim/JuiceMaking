using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomInfo : MonoBehaviour
{
    [SerializeField] BoomFogInfo fog_Obj = null;
    [SerializeField] GameObject sliceEff = null;

    // 썩은과일이 터치포인트와 닿았을때 실행
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WallInfo>()) return;

        GameManager.Inst.CutGameMinus();
        SoundManager.Inst.PlaySFX("rotten fruit");

        BoomFogInfo fog = Instantiate(fog_Obj);

        GameObject eff = Instantiate(sliceEff, transform.position, transform.rotation);
        eff.transform.localScale = new Vector2(2, 2);
        eff.SetActive(true);
        Destroy(eff, 0.9f);

        fog.transform.position = new Vector3(eff.transform.position.x, eff.transform.position.y, -1);
        fog.transform.rotation = Quaternion.identity;

        Destroy(this.gameObject);
    }
}
