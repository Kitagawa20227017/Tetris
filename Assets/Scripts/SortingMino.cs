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
    [SerializeField] private GameObject[] _mino_Objs_model = default;
    [SerializeField] private GameObject _modelObjs = default;
    // 削除するオブジェクトの一時保存場所
    [SerializeField] private GameObject _destoyObj = default;

    // 非アクティブのミノの一時避難場所
    readonly private Vector2 _evacuation = new Vector2(100f, 100f);
    // 子オブジェクト取得用
    private Transform _parentTransform = default;

    private int[] mino = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    private List<int> _randomList = new List<int>();
    readonly private Vector2 _evenNumberSidePos = new Vector2(10.5f, 8.5f);
    readonly private Vector2 _oddNumberSidePos = new Vector2(10f, 8);
    readonly private Vector2 _tMinoPos = new Vector2(10f, 9f);
    private Vector2 _test = new Vector2(21f, 7f);
    private float qq = 21f,ii = 5f;
    int[] lll = new int[4];
    GameObject[] ppp = new GameObject[4];
    private int indexArray = 0;
    int _keepMino = -1;
    public int IndexArray { get => indexArray; set => indexArray = value; }
    public int[] Mino { get => mino; set => mino = value; }

    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    void Start()
    {
        _parentTransform = _modelObjs.gameObject.transform;
        RandomNunber();
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    void Update()
    {
        //if (Input.GetButton("KeepMino"))
        //{
        //    KeepMIno();
        //}
        if (gameObject.transform.childCount == 0)
        {
            aaa();
        }
    }

    private void RandomNunber()
    {
        int conut = IndexArray;
        if(conut == 3)
        {
            conut = 7;
        }
        else if(conut == 11)
        {
            conut = 0;
        }

        for (int i = 0; i <= 6; i++)
        {
            _randomList.Add(i);
        }
        while (_randomList.Count > 0)
        {
            int indexNunber = Random.Range(0, _randomList.Count);
            Mino[conut] = _randomList[indexNunber];
            _randomList.RemoveAt(indexNunber);
            conut++;
        }
    }

    private void KeepMIno()
    {
        int n = indexArray;
        if(n - 1 <= -1)
        {
            n = 13;
        }
        if(_keepMino == -1)
        {
            _keepMino = Mino[n];
        }
        else
        {
            int tem = _keepMino;
            _keepMino = n;
            n = tem;
            if (n == 1 || Mino[IndexArray] == 0)
            {
                Instantiate(_minoObjs[Mino[IndexArray]], _evenNumberSidePos, Quaternion.identity, this.transform);
               
            }
            else if (n == 2)
            {
                Instantiate(_minoObjs[Mino[IndexArray]], _tMinoPos, Quaternion.identity, this.transform);
                
            }
            else
            {
                Instantiate(_minoObjs[Mino[IndexArray]], _oddNumberSidePos, Quaternion.identity, this.transform);
            }
            gameObject.transform.GetChild(0).gameObject.transform.position = _evacuation;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(0).gameObject.transform.parent = _destoyObj.transform;
        }
        
    }

    private void aaa()
    {
        if (ppp[0] == null)
        {
            
        }
        else
        {
            for (int i = 0; i < lll.Length; i++)
            {
                ppp[i].transform.position = _evacuation;
                ppp[i].SetActive(false);
                ppp[i].transform.parent = _destoyObj.transform;
                ppp[i] = default;
            }
        }

        if (Mino[IndexArray] == 1 || Mino[IndexArray] == 0)
        {
            Instantiate(_minoObjs[Mino[IndexArray]], _evenNumberSidePos, Quaternion.identity, this.transform);
            IndexArray++;
            if(IndexArray >= 14)
            {
                IndexArray = 0;
            }
            if (IndexArray == 3 || IndexArray == 11)
            {
                RandomNunber();
            }
        }
        else if (Mino[IndexArray] == 2)
        {
            Instantiate(_minoObjs[Mino[IndexArray]], _tMinoPos, Quaternion.identity, this.transform);
            IndexArray++;
            if (IndexArray >= 14)
            {
                IndexArray = 0;
            }
            if (IndexArray == 3 || IndexArray == 11)
            {
                RandomNunber();
            }
        }
        else
        {
            Instantiate(_minoObjs[Mino[IndexArray]], _oddNumberSidePos, Quaternion.identity, this.transform);
            IndexArray++;
            if (IndexArray >= 14)
            {
                IndexArray = 0;
            }
            if (IndexArray == 3 || IndexArray == 11)
            {
                RandomNunber();
            }
        }

        qq = 21f;
        ii = 5f;
        int j = IndexArray;
        for (int i = 0; i < lll.Length; i++)
        {
            if(j >= 14)
            {
                j = 0;
            }
            Instantiate(_mino_Objs_model[Mino[j]], new Vector2(qq,ii), Quaternion.identity,_modelObjs.transform);
            if (j+1 >= 14)
            {
                j = 0;
            }
            if (i <= 2 && Mino[j + 1] == 1)
            { 
                ii = ii - 2.5f;
            }
            else if(i <= 2 && Mino[j + 1] != 1)
            {
                ii = ii - 3f;
            }
            j++;
        }

        int n = 0;
        if (transform.childCount != 0)
        {
            foreach (Transform chlid in _parentTransform)
            {
                ppp[n] = chlid.gameObject;
                n++;
            }
        }
    }

    #endregion

}