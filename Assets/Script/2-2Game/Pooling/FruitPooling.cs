using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitPooling : Singleton<FruitPooling>
{
    [SerializeField] FruitInfo fruit_Obj = null;
    private Queue<FruitInfo> queue = new Queue<FruitInfo>();

    private FruitInfo Create()
    {
        FruitInfo fruit = Instantiate(fruit_Obj);
        return fruit;
    }

    public FruitInfo Out(float _x, float _y)
    {
        SoundManager.Inst.PlaySFX("Fruit Throwing");

        FruitInfo fruit = null;

        if (queue.Count == 0)
            fruit = Create();
        else
            fruit = queue.Dequeue();

        fruit.transform.position = new Vector3(_x, _y, 0);
        fruit.transform.rotation = Quaternion.identity;
        
        fruit.gameObject.SetActive(true);

        return fruit;
    }

    public void In(FruitInfo _fruit)
    {
        _fruit.gameObject.SetActive(false);
        queue.Enqueue(_fruit);
    }
}
