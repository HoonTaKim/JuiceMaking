using System.Collections.Generic;
using UnityEngine;

public class MixPooling : Singleton<MixPooling>
{
    [SerializeField] private MixImageMove mixImage_Prefab = null;
    Queue<MixImageMove> queue = new Queue<MixImageMove>();

    // Queue에 존재하지 않는다면 생성
    private MixImageMove Create()
    {
        MixImageMove mixImage = Instantiate(mixImage_Prefab);
        return mixImage;
    }

    // 호출시 프리팹을 Queue에서 활성화
    public MixImageMove Out(float _y)
    {
        SoundManager.Inst.PlaySFX("Mixer_Sound01");

        MixImageMove mixImage = null;

        if (queue.Count == 0)
            mixImage = Create();
        else
            mixImage = queue.Dequeue();

        mixImage.transform.position = new Vector3(-9f, _y / 1.5f, 0);
        mixImage.transform.rotation = Quaternion.identity;
        mixImage.gameObject.SetActive(true);

        return mixImage;
    }

    // 호출시 프리팹을 Queue에 비활성화 후 저장
    public void In(MixImageMove _mixImage)
    {
        _mixImage.gameObject.SetActive(false);
        queue.Enqueue(_mixImage);
    }
}
