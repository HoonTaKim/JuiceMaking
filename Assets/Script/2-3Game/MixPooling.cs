using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixPooling : Singleton<MixPooling>
{
    [SerializeField] private MixImageMove mixImage_Prefab = null;

    Queue<MixImageMove> queue = new Queue<MixImageMove>();

    private MixImageMove Create()
    {
        MixImageMove mixImage = Instantiate(mixImage_Prefab);
        return mixImage;
    }

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

    public void In(MixImageMove _mixImage)
    {
        _mixImage.gameObject.SetActive(false);
        queue.Enqueue(_mixImage);
    }
}
