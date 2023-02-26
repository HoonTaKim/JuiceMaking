using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomInfo : MonoBehaviour
{
    [SerializeField] BoomFogInfo fog_Obj = null;
    [SerializeField] GameObject sliceEff = null;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WallInfo>()) return;

        GameManager.Inst.BoomScore();
        SoundManager.Inst.PlaySFX("rotten fruit");

        BoomFogInfo fog = Instantiate(fog_Obj);

        // ����ٰ� ����Ʈ ����־�ߴ�
        GameObject eff = Instantiate(sliceEff, transform.position, transform.rotation);
        eff.transform.localScale = new Vector2(3, 3);
        eff.SetActive(true);
        Destroy(eff, 3f);

        fog.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y, -1);
        fog.transform.rotation = Quaternion.identity;

        Destroy(this.gameObject);
    }
}