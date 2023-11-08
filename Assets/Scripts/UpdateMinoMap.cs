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

    private Transform _parentTransform; // 子オブジェクト取得用
    private int[,] _map = new int[24, 12]; // 盤面の格納用

    #endregion

    #region プロパティ 
    public int[,] Map { get => _map; set => _map = value; }

    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary>  
    void Awake()
    {
        _parentTransform = this.gameObject.transform; 
    }

    // ミノの更新
    public void SearchMino()
    {
        foreach (Transform chlid in _parentTransform)
        {
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-chlid.localPosition.y);
            int horizontalAxis = Mathf.FloorToInt(chlid.localPosition.x);

            // タグでオブジェクトを判断して配列に格納
            switch (chlid.tag)
            {
                case "Wall":
                    Map[verticalAxis, horizontalAxis] = -1;
                    break;

                case "Mino":
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
        for (int i = Map.GetLength(0) - 1; i > 0; i--) // 一番下の列は、外壁なのでその一つ上から始める
        {
            for (int j = 1; j < Map.GetLength(1) - 2; j++) // 一つ右を見るので外壁+一番左のミノ分引いた位置まで見る また一番右は、外壁なので１スタート
            {
                if (Map[i, j] == 1 && Map[i, j] == Map[i, j + 1]) // 現在の位置から一つ右を見てミノだったら次を調べる
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
                destoryMinoLine[countDestoryMino] = i; // 列の記録
                countDestoryMino++; // 配列を1つ進める
            }
        }
        if (destoryMinoLine[0] != -1)
        {
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
        foreach (Transform chlid in _parentTransform)
        {
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-chlid.localPosition.y);
            int horizontalAxis = Mathf.FloorToInt(chlid.localPosition.x);

            for (int j = 0; j < destoryMinoLine.Length; j++)
            {
                if (chlid.transform.localPosition.y == -destoryMinoLine[j] && chlid.tag == "Mino") 
                {
                    Map[verticalAxis, horizontalAxis] = 0;
                    Destroy(chlid.gameObject);
                }
            }
        }
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
            for (int j = 0; j < destoryMinoLine.Length; j++)
            {
                if (chlid.tag == "Mino" && destoryMinoLine[j] > verticalAxis)
                {
                    chlid.transform.localPosition = new Vector2(chlid.localPosition.x, chlid.localPosition.y - 1);
                }
            }
        }
    }
    
    #endregion

}
