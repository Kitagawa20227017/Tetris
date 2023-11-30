// ---------------------------------------------------------  
// UpdateMinoMap.cs  
//   
// 作成日: 2023/10/26 
// 作成者:  北川稔明
// ---------------------------------------------------------  
using UnityEngine;
using System;
using System.Collections;

public class UpdateMinoMap : MonoBehaviour
{

    #region 変数  

    [SerializeField] private GameObject _destoyObj;

    private const string MINOOBJTAG = "Mino";
    private const string WALLOBJTAG = "Wall";

    private DeletionObj _deletionObj;
    private Transform _parentTransform = default; // 子オブジェクト取得用

    private int _desIndex = default;
    private int[,] _map = new int[24, 12]; // 盤面の格納用

    string[,] a = new string[24, 1];

    #endregion

    #region プロパティ 
    public int[,] Map 
    { 
        get => _map; 
        set => _map = value; 
    }

    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary>  
    private void Awake()
    {
        // オブジェクト、スクリプトの取得、格納
        _deletionObj = GameObject.Find("DeleteObjs").GetComponent<DeletionObj>();
        _parentTransform = this.gameObject.transform;

        // マップの初期設定
        SearchMino();
    }

    private void Update()
    {
        SearchMino();
        for(int i = 0; i < Map.GetLength(0); i++){
            string b = default;
            for(int j = 0; j < Map.GetLength(1); j++)
            {
                b = b + Map[i, j].ToString();
            }
            Debug.Log(i + "番目 : " + b);
        }
    }

    // ミノの更新
    private void SearchMino()
    {
        foreach (Transform chlid in _parentTransform)
        {
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-chlid.localPosition.y);
            int horizontalAxis = Mathf.FloorToInt(chlid.localPosition.x);

            // タグでオブジェクトを判断して配列に格納
            switch (chlid.tag)
            {
                case WALLOBJTAG:
                    Map[verticalAxis, horizontalAxis] = -1;
                    break;

                case MINOOBJTAG:
                    Map[verticalAxis, horizontalAxis] = 1;
                    break;
            }
        }
        Search();
    }

    /// <summary>
    /// 削除する列を探す処理
    /// </summary>
    private void Search()
    {
        int[] destoryMinoLine = {-1,-1,-1,-1}; // 削除する列を格納、Iミノが4マスなので1回で消せる最大は4列
        int countDestoryMino = 0; // destoryMinoLineのインデックス 
        bool isDestory = false; // 削除できるかどうかのフラグ

        // 削除出来る列を探索する処理
        for (int i = Map.GetLength(0) - 1; i > 0; i--){ // 一番下の列は、外壁なのでその一つ上から始める
            // 一つ右を見るので外壁+一番左のミノ分引いた位置まで見る。また一番右は、外壁なので１スタート
            for (int j = 1; j < Map.GetLength(1) - 2; j++)
            {
                // 現在の位置から一つ右を見てミノだったら次を調べる
                if (Map[i, j] == 1 && Map[i, j] == Map[i, j + 1])
                {
                    isDestory = true;
                }
                else // 違ったらfor文を抜けてフラグをoffにする
                {
                    isDestory = false;
                    break;
                }
            }
            if (isDestory) // 削除出来る列があったときその列を記録する
            {
                _desIndex += 10;
                destoryMinoLine[countDestoryMino] = i; // 列の記録
                countDestoryMino++; // 配列を1つ進める
            }
        }

        if (destoryMinoLine[0] != -1) // 削除出来る列があるときだけ削除処理を呼ぶ
        {
            Debug.Log(_desIndex);
            DeletionMino(destoryMinoLine);
            DownMino(destoryMinoLine);
        }
    }

    /// <summary>
    /// ミノの削除処理
    /// </summary>
    /// <param name="destoryMinoLine"></param>
    private void DeletionMino(int[] destoryMinoLine)
    {
        GameObject[] _des = new GameObject[_desIndex];
        int n = 0;
        foreach (Transform chlid in _parentTransform) // 子オブジェクトを取得
        {
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-chlid.localPosition.y);
            int horizontalAxis = Mathf.FloorToInt(chlid.localPosition.x);

            for (int i = 0; i < destoryMinoLine.Length; i++) 
            {
                // 削除する列と等しい高さのミノを非アクティブにする
                if (chlid.transform.localPosition.y == -destoryMinoLine[i] && chlid.tag == "Mino") 
                {
                    _des[n] = chlid.gameObject;
                    chlid.gameObject.SetActive(false);
                    chlid.position = new Vector2(100, 100);
                    n++;
                    Map[verticalAxis, horizontalAxis] = 0; // 配列を更新する
                }
            }
            _desIndex = 0;
        }

        // 別オブジェクトを親に設定する
        for(int i = 0; i < _des.Length; i++)
        {
            _des[i].transform.parent = _destoyObj.transform;
            _des[i] = default;
        }

        _deletionObj.DestyoyObj();
    }

    /// <summary>
    /// ミノを下げる処理
    /// </summary>
    /// <param name="destoryMinoLine"></param>
    private void DownMino(int[] destoryMinoLine)
    {
        foreach (Transform chlid in _parentTransform)
        {
            int verticalAxis = Mathf.FloorToInt(-chlid.localPosition.y);
            int horizontalAxis = Mathf.FloorToInt(chlid.localPosition.x);
            for (int i = 0; i < destoryMinoLine.Length; i++)
            {
                if (chlid.tag == "Mino" && destoryMinoLine[i] > verticalAxis)
                {
                    chlid.transform.localPosition = new Vector2(chlid.localPosition.x, chlid.localPosition.y - 1);
                    Map[verticalAxis, horizontalAxis] = 0;
                }
            }
        }
    }
    
    #endregion

}
