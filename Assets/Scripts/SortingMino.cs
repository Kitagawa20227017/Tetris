// ---------------------------------------------------------  
// SortingMino.cs  
//   
// 作成日:  2023/11/16
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections.Generic;

public class SortingMino : MonoBehaviour
{

    #region 変数  

    [SerializeField] private GameObject[] _minoObjs = default;
    private Transform _parentTransform = default; // 子オブジェクト取得用
    private int[] _mino = { 0, 0, 0, 0, 0, 0, 0, };
    private List<int> _randomList = new List<int>();
    private Vector2 _evenNumberSidePos = new Vector2(10.5f,8.5f);
    private Vector2 _oddNumberSidePos = new Vector2(10f, 8);
    private Vector2 _tMinoPos = new Vector2(5f, 1);

    int _indexArray = 0;

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary>  
    void Awake()
    {
    }
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    void Start ()
    {
        RandomNunber();
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    void Update ()
    {
        if(gameObject.transform.childCount == 0)
        {
            aaa();
        }
    }

    private void RandomNunber()
    {
        int conut = 0;
        for(int i = 0; i <= 6; i++)
        {
            _randomList.Add(i);
        }
        while(_randomList.Count > 0)
        {
            int indexNunber = Random.Range(0, _randomList.Count);
            _mino[conut] = _randomList[indexNunber];
            _randomList.RemoveAt(indexNunber);
            conut++;
        }
    }

    private void aaa()
    {
        Debug.Log("test :" + _mino[_indexArray]);
        if(_mino[_indexArray] == 1 || _mino[_indexArray] == 0)
        {
            Instantiate(_minoObjs[_mino[_indexArray]],_evenNumberSidePos, Quaternion.identity, this.transform);
            _indexArray++;
            if(_indexArray >= 7)
            {
                _indexArray = 0;
                RandomNunber();
            }
        }
        else if(_mino[_indexArray] == 2)
        {
            Instantiate(_minoObjs[_mino[_indexArray]],_tMinoPos, Quaternion.identity, this.transform);
            if (_indexArray >= 7)
            {
                _indexArray = 0;
                RandomNunber();
            }
        }
        else
        {
            Instantiate(_minoObjs[_mino[_indexArray]], _oddNumberSidePos, Quaternion.identity, this.transform);
            if (_indexArray >= 7)
            {
                _indexArray = 0;
                RandomNunber();
            }
        }
    }

    #endregion

}
