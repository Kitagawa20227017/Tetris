// ---------------------------------------------------------  
// Sortingmino.cs  
//   
// 作成日:  2023/11/16
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections.Generic;

public class SortingMino : MonoBehaviour
{

    #region 変数  

    #region const変数

    // ランダム用の初期化数値
    private const int BEFORE_NUMBER = 3;
    private const int BEHIND_NUMBER = 11;
    private const int BEFORE_NUMBER_START = 7;
    private const int BEHIND_NUMBER_START = 0;

    // ストック用の判定
    private const int ARRAY_MINO_LAST = 13;
    private const int INITIAL_VALUE = -1;

    // ミノの判定用
    private const int OMINO = 0;
    private const int IMINO = 1;
    private const int TMINO = 2;

    // 配列外判定
    private const int ARRAY_OVER = 14;

    // 表示するミノの初期化用
    private const float INITIAL_POS_X = 21f;
    private const float INITIAL_POS_Y = 5f;

    // 表示するミノの位置の間隔
    private const float NEXT_MINO_MOVE_FEW = 2.5f;
    private const float NEXT_MINO_MOVE_GREAT = 3f;

    // 配列の最後から１つ前
    private const int ARRAY_NUMBER_BEFORE = 2;

    #endregion

    // 各ミノの親オブジェクト
    [SerializeField] private GameObject[] _minoObjs = default;
    [SerializeField] private GameObject[] _mino_Objs_model = default;
    [SerializeField] private GameObject _modelObjs = default;
    [SerializeField] private GameObject _keepMino = default;

    // 削除するオブジェクトの一時保存場所
    [SerializeField] private GameObject _destoyObj = default;

    // 非アクティブのミノの一時避難場所
    readonly private Vector2 _evacuation = new Vector2(100f, 100f);

    // 子オブジェクト取得用
    private Transform _parentTransform = default;

    // 出現させるミノ格納
    private int[] mino = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // ランダム用のリスト
    private List<int> _randomList = new List<int>();

    // 各ミノの出現位置
    readonly private Vector2 _evenNumberSidePos = new Vector2(10.5f, 8.5f);
    readonly private Vector2 _oddNumberSidePos = new Vector2(10f, 8);
    readonly private Vector2 _tMinoPos = new Vector2(10f, 9f);

    // ストックのミノの位置
    readonly private Vector2 _keepMinoPos = new Vector2(0f, 4f);

    // ストックするミノ
    private int _keepMinoNumber = -1;

    // ストック管理
    bool _isKeep = default;

    // 次にくるミノの位置の初期位置
    private float _nextMinoPosX = 21f, _nextMinoPosY = 5f;

    // 表示するミノの格納
    private int[] _nextMino = new int[4];
    private GameObject[] _nextMinoObj = new GameObject[4];

    // 出現させるミノの指標
    private int _indexArray = 0;

    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start()
    {
        _parentTransform = _modelObjs.gameObject.transform;
        RandomNunber();
    }

    private void Update()
    {
        if(Input.GetButtonDown("KeepMino") && _isKeep)
        {
            _isKeep = false;
            KeepMino();
        }
    }

    /// <summary>
    /// ミノが出現可能かの判定
    /// </summary>
    public void IsAdvent()
    {
        if (gameObject.transform.childCount == 0)
        {
            _isKeep = true;
            Advent();
        }
    }

    /// <summary>
    /// ミノの順番をランダムに決める
    /// </summary>
    private void RandomNunber()
    {
        int conut = _indexArray;
        if(conut == BEFORE_NUMBER)
        {
            conut = BEFORE_NUMBER_START;
        }
        else if(conut == BEHIND_NUMBER)
        {
            conut = BEHIND_NUMBER_START;
        }

        for (int i = 0; i <= 6; i++)
        {
            _randomList.Add(i);
        }
        while (_randomList.Count > 0)
        {
            int indexNunber = Random.Range(0, _randomList.Count);
            mino[conut] = _randomList[indexNunber];
            _randomList.RemoveAt(indexNunber);
            conut++;
        }
    }

    /// <summary>
    /// ミノのストック処理
    /// </summary>
    private void KeepMino()
    {
        int onePrevious = _indexArray - 1;

        // 配列外にならないようにする
        if (onePrevious <= INITIAL_VALUE)
        {
            onePrevious = ARRAY_MINO_LAST;
        }

        // ミノを消す
        gameObject.transform.GetChild(0).gameObject.transform.position = _evacuation;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.transform.parent = _destoyObj.transform;

        // 何もストックしてない時
        if (_keepMinoNumber == INITIAL_VALUE)
        {
            _keepMinoNumber = mino[onePrevious];
        }
        else // 何かストックしている時
        {
            // ミノの番号の入れ替え
            int tem = _keepMinoNumber;
            _keepMinoNumber = mino[onePrevious];
            onePrevious = tem;

            // ストックしてたミノを出現させる
            if (onePrevious == OMINO || onePrevious == IMINO)
            {
                Instantiate(_minoObjs[onePrevious], _evenNumberSidePos, Quaternion.identity, this.transform);
            }
            else if (onePrevious == TMINO)
            {
                Instantiate(_minoObjs[onePrevious], _tMinoPos, Quaternion.identity, this.transform);
            }
            else
            {
                Instantiate(_minoObjs[onePrevious], _oddNumberSidePos, Quaternion.identity, this.transform);
            }
        }

        // ストックしているミノを表示する
        if (_keepMino.gameObject.transform.childCount == 0)
        {
            Instantiate(_mino_Objs_model[mino[onePrevious]], _keepMinoPos, Quaternion.identity, _keepMino.transform);
        }
        else
        {
            _keepMino.gameObject.transform.GetChild(0).gameObject.transform.position = _evacuation;
            _keepMino.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            _keepMino.gameObject.transform.GetChild(0).gameObject.transform.parent = _destoyObj.transform;
            Instantiate(_mino_Objs_model[_keepMinoNumber], _keepMinoPos, Quaternion.identity, _keepMino.transform);
        }
        _isKeep = false;
    }


    /// <summary>
    /// ミノの出現処理
    /// </summary>
    private void Advent()
    {
        if (_nextMinoObj[0] == null)
        {
            // 何もしない
        }
        else
        {
            for (int i = 0; i < _nextMino.Length; i++)
            {
                _nextMinoObj[i].transform.position = _evacuation;
                _nextMinoObj[i].SetActive(false);
                _nextMinoObj[i].transform.parent = _destoyObj.transform;
                _nextMinoObj[i] = default;
            }
        }

        // ミノの出現
        if (mino[_indexArray] == OMINO || mino[_indexArray] == IMINO)
        {
            Instantiate(_minoObjs[mino[_indexArray]], _evenNumberSidePos, Quaternion.identity, this.transform);
            _indexArray++;
            if(_indexArray >= ARRAY_OVER)
            {
                _indexArray = 0;
            }
            if (_indexArray == BEFORE_NUMBER || _indexArray == BEHIND_NUMBER)
            {
                RandomNunber();
            }
        }
        else if (mino[_indexArray] == TMINO)
        {
            Instantiate(_minoObjs[mino[_indexArray]], _tMinoPos, Quaternion.identity, this.transform);
            _indexArray++;
            if (_indexArray >= ARRAY_OVER)
            {
                _indexArray = 0;
            }
            if (_indexArray == BEFORE_NUMBER || _indexArray == BEHIND_NUMBER)
            {
                RandomNunber();
            }
        }
        else
        {
            Instantiate(_minoObjs[mino[_indexArray]], _oddNumberSidePos, Quaternion.identity, this.transform);
            _indexArray++;
            if (_indexArray >= ARRAY_OVER)
            {
                _indexArray = 0;
            }
            if (_indexArray == BEFORE_NUMBER || _indexArray == BEHIND_NUMBER)
            {
                RandomNunber();
            }
        }

        // 初期化
        _nextMinoPosX = INITIAL_POS_X;
         _nextMinoPosY = INITIAL_POS_Y;
        int nowNumber = _indexArray;
        // ミノの出現予測表示
        for (int i = 0; i < _nextMino.Length; i++)
        {
            if(nowNumber >= ARRAY_OVER)
            {
                nowNumber = 0;
            }
            Instantiate(_mino_Objs_model[mino[nowNumber]], new Vector2(_nextMinoPosX, _nextMinoPosY), Quaternion.identity,_modelObjs.transform);
            if (nowNumber+1 >= ARRAY_OVER)
            {
                nowNumber = 0;
            }
            if (i <= ARRAY_NUMBER_BEFORE && mino[nowNumber + 1] == 1)
            { 
                 _nextMinoPosY =  _nextMinoPosY - NEXT_MINO_MOVE_FEW;
            }
            else if(i <= ARRAY_NUMBER_BEFORE && mino[nowNumber + 1] != 1)
            {
                 _nextMinoPosY =  _nextMinoPosY - NEXT_MINO_MOVE_GREAT;
            }
            nowNumber++;
        }

        int n = 0;
        if (transform.childCount != 0)
        {
            foreach (Transform chlid in _parentTransform)
            {
                _nextMinoObj[n] = chlid.gameObject;
                n++;
            }
        }
    }

    #endregion

}