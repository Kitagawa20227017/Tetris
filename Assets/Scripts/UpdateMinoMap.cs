// ---------------------------------------------------------  
// UpdateMinoMap.cs  
//   
// 作成日: 2023/10/26 
// 作成者:  北川稔明
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class UpdateMinoMap : MonoBehaviour
{

    #region 変数  

    // 削除するオブジェクトの一時保存場所
    [SerializeField] private GameObject _destoyObj = default;

    #region const定数

    // ミノオブジェクトタグ
    private const string MINO_OBJ_TAG = "Mino";

    // ミノオブジェクト番号
    private const int MINO_OBJ = 1;

    // 外壁オブジェクトタグ
    private const string WALL_OBJ_TAG = "Wall";

    // 外壁オブジェクト番号
    private const int WALL_OBJ = -1;

    // 非アクティブにするオブジェクトがない
    private const int NULL_DESTORY_OBJ = -1;

    // ミノを下げる列数
    private const int DOWN_MINO_LINE = 1;

    // 削除するミノ数の増加量
    private const int DELETE_MINO_QTY = 10;

    // クリアライン数
    private const int CLEARLINE = 30;

    #endregion

    // 非アクティブのミノの一時避難場所
    readonly private Vector2 _evacuation = new Vector2(100f, 100f);

    // スクリプトの取得
    private GameOver _gameOver = default;
    private DeletionObj _deletionObj = default;

    // 子オブジェクト取得用
    private Transform _parentTransform = default;

    // ミノ削除用の要素数格納
    private int _destoyIndex = default;

    // 盤面の格納用
    private int[,] _map = new int[24, 12];

    // 消したライン数の記憶
    private int _lineDeleted = 0;

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
        _gameOver = GameObject.Find("PlayerMap").GetComponent<GameOver>();
        _parentTransform = this.gameObject.transform;

        // マップの初期設定
        UpdateMino();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // マップの探索処理
        UpdateMino();

        

        // ゲームオーバーの判定処理
        GameOver();
    }

    /// <summary>
    ///  ミノの更新
    /// </summary>
    private void UpdateMino()
    {
        foreach (Transform chlid in _parentTransform)
        {
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-chlid.localPosition.y);
            int horizontalAxis = Mathf.FloorToInt(chlid.localPosition.x);

            // タグでオブジェクトを判断して配列に格納
            switch (chlid.tag)
            {
                case WALL_OBJ_TAG:
                    Map[verticalAxis, horizontalAxis] = WALL_OBJ;
                    break;

                case MINO_OBJ_TAG:
                    Map[verticalAxis, horizontalAxis] = MINO_OBJ;
                    break;
            }
        }
        DestoyMinoSearch();
    }

    /// <summary>
    /// 削除する列を探す処理
    /// </summary>
    private void DestoyMinoSearch()
    {
        // 削除する列を格納
        int[] destoryMinoLine = {NULL_DESTORY_OBJ, NULL_DESTORY_OBJ, NULL_DESTORY_OBJ, NULL_DESTORY_OBJ };
        
        // destoryMinoLineのインデックス
        int countDestoryMino = 0;
        
        // 削除できるかどうかのフラグ
        bool isDestory = false;

        // 削除出来る列を探索する処理
        // 一番下の列は、外壁なのでその一つ上から始める
        for (int i = Map.GetLength(0) - 1; i > 0; i--){
            // 一番右のミノ(1)から左から2番のミノ(10)まで探索
            for (int j = 1; j < Map.GetLength(1) - 2; j++)
            {
                // 現在の位置から一つ右を見てミノだったら次を調べる
                if (Map[i, j] == MINO_OBJ && Map[i, j] == Map[i, j + 1])
                {
                    isDestory = true;
                }
                // 違ったらfor文を抜けてフラグをoffにする
                else
                {
                    isDestory = false;
                    break;
                }
            }

            // 削除出来る列があったときその列を記録する
            if (isDestory)
            {
                _lineDeleted++;
                _destoyIndex += DELETE_MINO_QTY;           
                destoryMinoLine[countDestoryMino] = i; 
                countDestoryMino++;                    
            }
        }

        // 削除出来る列があるときだけ非アクティブ処理を呼ぶ
        if (destoryMinoLine[0] != NULL_DESTORY_OBJ)
        {
            DeletionMino(destoryMinoLine);
            DownMino(destoryMinoLine);
        }
    }

    /// <summary>
    /// ミノの非アクティブ処理
    /// </summary>
    /// <param name="destoryMinoLine">ミノの消すラインの列数</param>
    private void DeletionMino(int[] destoryMinoLine)
    {
        // 削除する分だけの配列をつくる
        GameObject[] destoryMino = new GameObject[_destoyIndex];

        // 配列の指標
        int index = 0;
        
        foreach (Transform chlid in _parentTransform) // 子オブジェクトを取得
        {
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-chlid.localPosition.y);
            int horizontalAxis = Mathf.FloorToInt(chlid.localPosition.x);

            for (int i = 0; i < destoryMinoLine.Length; i++) 
            {
                // 削除する列と等しい高さのミノを別の場所に移動して非アクティブにする
                if (chlid.transform.localPosition.y == -destoryMinoLine[i] && chlid.tag == MINO_OBJ_TAG) 
                {
                    destoryMino[index] = chlid.gameObject;
                    chlid.gameObject.SetActive(false);
                    chlid.position = _evacuation;
                    index++;
                    Map[verticalAxis, horizontalAxis] = 0;
                }
            }
            _destoyIndex = 0;
        }

        // 別オブジェクトを親に設定する
        for(int i = 0; i < destoryMino.Length; i++)
        {
            destoryMino[i].transform.parent = _destoyObj.transform;
            destoryMino[i] = default;
        }

        // ミノの削除処理
        _deletionObj.DestyoyObj();
    }

    /// <summary>
    /// ミノを下げる処理
    /// </summary>
    /// <param name="downMinoLine">ミノの段下げの列数</param>
    private void DownMino(int[] downMinoLine)
    {
        foreach (Transform chlid in _parentTransform)
        {
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-chlid.localPosition.y);
            int horizontalAxis = Mathf.FloorToInt(chlid.localPosition.x);

            for (int i = 0; i < downMinoLine.Length; i++)
            {
                // 削除する列より低いミノの段を下げる
                if (chlid.tag == MINO_OBJ_TAG && downMinoLine[i] > verticalAxis)
                {
                    chlid.transform.localPosition = new Vector2(chlid.localPosition.x, chlid.localPosition.y - DOWN_MINO_LINE);
                    Map[verticalAxis, horizontalAxis] = 0;
                }
            }
        }
    }
    
    /// <summary>
    /// ゲームオーバー判定処理
    /// </summary>
    private void GameOver()
    {
        if(Map[0,4] == MINO_OBJ || Map[0,5] == MINO_OBJ || Map[0,6] == MINO_OBJ || Map[0,7] == MINO_OBJ)
        {
            _gameOver.GameOverJudgment();
        }
    }

    private void GameClear()
    {
        if(_lineDeleted >= CLEARLINE)
        {
            Debug.Log("Clear");
        }
    }

    #endregion

}
