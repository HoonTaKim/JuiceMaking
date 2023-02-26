using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicePooling : Singleton<SlicePooling>
{
    [SerializeField] private List<Sprite> sliceLeftList = new List<Sprite>();
    [SerializeField] private List<Sprite> sliceRightList = new List<Sprite>();
    private List<Sprite> saveSliceRightList = new List<Sprite>();
    [SerializeField] SliceInfo slice_Obj = null;
    private Queue<SliceInfo> queue = new Queue<SliceInfo>();

    public List<Sprite> Get_SaveSliceRightList { get { return saveSliceRightList; } private set { } }

    private SliceInfo Create()
    {
        SliceInfo slice = Instantiate(slice_Obj);
        return slice;
    }

    public SliceInfo Out(float _x, float _y, Quaternion _r, Sprite _sprite, bool _dir)
    {
        SliceInfo slice = null;

        if (queue.Count == 0)
            slice = Create();
        else
            slice = queue.Dequeue();

        slice.transform.position = new Vector3(_x, _y, 0);
        slice.transform.rotation = _r;
        if (_dir)
            slice.GetComponent<SpriteRenderer>().sprite = FindSprite(_sprite, sliceLeftList, _dir);
        else
            slice.GetComponent<SpriteRenderer>().sprite = FindSprite(_sprite, sliceRightList, _dir);
        slice.gameObject.SetActive(true);

        return slice;
    }

    public Sprite FindSprite(Sprite _sprite, List<Sprite> _checkList, bool _dir)
    {
        string spriteName;

        // _dir이 true라면 왼쪽(왼쪽의 이름은 1이 들어감) 아니라면 오른쪽(오른쪽은 2가 들어감)
        if (_dir)
            spriteName = _sprite.name + "1";
        else
            spriteName = _sprite.name + "2";

        for (int i = 0; i < _checkList.Count; i++)
        {
            if (_checkList[i].name == spriteName)
            {
                saveSliceRightList.Add(_checkList[i]);
                return _checkList[i];
            }
        }

        return null;
    }

    public void In(SliceInfo _slice)
    {
        _slice.gameObject.SetActive(false);
        queue.Enqueue(_slice);
    }
}
