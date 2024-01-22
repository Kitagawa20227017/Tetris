// ---------------------------------------------------------  
// MoveMino.cs
// 
// プレイヤーに入力処理及びそれに伴うミノの挙動変化
// 
// 作成日:  2023/11/2
// 作成者:  北川稔明
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class MoveMino : MonoBehaviour
{

    #region 変数  

    #region const定数

    // 右方向の移動
    private const string RIGHT_MOVE_MINO = "Right";

    // 左方向の移動
    private const string LEFT_MOVE_MINO = "Left";

    // マップ(外壁含む)の横の最大
    private const int MAP_SIDE_MAX = 11;

    // マップ(外壁含む)の横の最小
    private const int MAP_SIDE_MIN = 0;

    // マップ(外壁含まない)の横の最大
    private const int MINO_SIDE_MAX = 10;

    // マップ(外壁含まない)の横の最大
    private const int MINO_SIDE_MIN = 1;

    // ミノの移動処理
    private const int MOVE_MINO = 1;

    // Iミノの移動処理
    private const int IMINO_MOVE = 2;

    // スコアの増加量
    private const int SCORE_ADD = 100;

    #endregion

    // Mapオブジェクト取得用
    private GameObject _mapObj = default;

    // UpdateMinoMapスクリプト取得用
    private UpdateMinoMap _updateMap = default;

    // 
    private Score _score = default;

    // 子オブジェクト取得用
    private Transform _parentTransform = default;

    // 右回転
    readonly private Quaternion _rightRotetion = Quaternion.Euler(0, 0, 90);

    // 左回転
    readonly private Quaternion _leftRotetion = Quaternion.Euler(0, 0, -90);

    // ミノの移動距離
    private int _moveMino = 1;

    // タイマー制限
    private float _minoTimer = 1f;

    // ミノの形
    private string _minoCondition = default;

    // ミノの落ちてくる時間を測るタイマー
    private float _downMinoTimer = 0;



    // ミノの形の識別
    private enum Mino
    {
        OMino,
        TMino,
        IMino,
        LMino,
        JMino,
        SMino,
        ZMino
    }

    // 選択できるようにする
    [SerializeField] private Mino _minoType;

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary>  
    private void Start()
    {
        // オブジェクト、スクリプトの取得、格納
        _mapObj = GameObject.Find("Map").gameObject;
        _updateMap = GameObject.Find("Map").GetComponent<UpdateMinoMap>();
        _score = GameObject.Find("PlayerMap").GetComponent<Score>();
        _parentTransform = this.gameObject.transform;
        _minoCondition = _minoType.ToString();
        if(_minoCondition == "IMino")
        {
            _moveMino= 2;
        }
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    void Update()
    {
        // ハードドロップ
        if (Input.GetButtonDown("HardDrop"))
        {
            HardDorp();
        }

        // 高速落下
        if (Input.GetButtonDown("UpSpeed"))
        {
            _minoTimer = 0.1f;
        }
        else
        {
            // 消したラインが多くなるほど落下が早くなる
            switch (_updateMap.LineDeleted / 10)
            {
                case 0:
                    _minoTimer = 1f;
                    break;

                case 1:
                    _minoTimer = 0.9f;
                    break;

                case 2:
                    _minoTimer = 0.8f;
                    break;

                case 3:
                    _minoTimer = 0.7f;
                    break;

                case 4:
                    _minoTimer = 0.6f;
                    break;

                case 5:
                    _minoTimer = 0.6f;
                    break;

                case 6:
                    _minoTimer = 0.5f;
                    break;

                case 7:
                    _minoTimer = 0.4f;
                    break;

                case 8:
                    _minoTimer = 0.3f;
                    break;

                case 9:
                    _minoTimer = 0.2f;
                    break;
            }
        }

        // 左右移動
        if (Input.GetButtonDown("RightMove") && RotationMino(RIGHT_MOVE_MINO))
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + MOVE_MINO, gameObject.transform.localPosition.y);
        }
        else if (Input.GetButtonDown("LeftMove") && RotationMino(LEFT_MOVE_MINO))
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - MOVE_MINO, gameObject.transform.localPosition.y);
        }

        // 回転
        if (Input.GetButtonDown("LeftRotation"))
        {
            MinoRevolution(LEFT_MOVE_MINO);
        }
        else if (Input.GetButtonDown("RightRotation"))
        {
            MinoRevolution(RIGHT_MOVE_MINO);
        }
    }

    private void FixedUpdate()
    {
        // 時間を測る
        _downMinoTimer += Time.deltaTime;
        if (_downMinoTimer >= _minoTimer)
        {
            if (DownMove())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - MOVE_MINO);
            }
            else
            {
                StopMino();
            }
            _downMinoTimer = 0;
        }
    }

    /// <summary>
    /// ミノの左右移動処理
    /// </summary>
    /// <param name="moveDirection">左右の移動方向</param>
    /// <returns>左右移動判定結果</returns>
    private bool RotationMino(string moveDirection)
    {
        bool isMinoMove = false;
        foreach (Transform chlid in _parentTransform)
        {
            isMinoMove = false;
            Vector3 localMinoPos = transform.root.gameObject.transform.InverseTransformPoint(chlid.gameObject.transform.position);

            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.CeilToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.CeilToInt(localMinoPos.x);
            // 左右どちらか判定
            if (moveDirection == RIGHT_MOVE_MINO)
            {
                // 1マス横に判定
                if (_updateMap.Map[verticalAxis, horizontalAxis + 1] == 0)
                {
                    isMinoMove = true;
                }
                else
                {
                    isMinoMove = false;
                    break;
                }
            }
            else if (moveDirection == LEFT_MOVE_MINO)
            {
                // 1マス横に判定
                if (_updateMap.Map[verticalAxis, horizontalAxis - 1] == 0)
                {
                    isMinoMove = true;
                }
                else
                {
                    isMinoMove = false;
                    break;
                }
            }
        }
        return isMinoMove;
    }

    /// <summary>
    /// ミノの落下判定処理
    /// </summary>
    /// <returns>落下判定結果</returns>
    private bool DownMove()
    {
        bool isMinoMove = false;
        foreach (Transform chlid in _parentTransform)
        {
            isMinoMove = false;
            Vector3 localMinoPos = transform.root.gameObject.transform.InverseTransformPoint(chlid.gameObject.transform.position);
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.CeilToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.CeilToInt(localMinoPos.x);
            // 1マス下の判定
            if (_updateMap.Map[verticalAxis + 1, horizontalAxis] == 0)
            {
                isMinoMove = true;
            }
            else
            {
                break;
            }
        }
        return isMinoMove;
    }

    /// <summary>
    /// ハードドロップの処理
    /// </summary>
    private void HardDorp()
    {
        bool isMinoMove = false;
        foreach (Transform chlid in _parentTransform)
        {
            isMinoMove = false;
            Vector3 localMinoPos = transform.root.gameObject.transform.InverseTransformPoint(chlid.gameObject.transform.position);
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.CeilToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.CeilToInt(localMinoPos.x);
            // 1マス下が何か判定
            if (_updateMap.Map[verticalAxis + 1, horizontalAxis] == 0)
            {
                isMinoMove = true;
            }
            else
            {
                isMinoMove = false;
                break;
            }
        }

        if (isMinoMove)
        {
            // 何もなかったら1マス下に移動しもう一回呼ぶ
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - MOVE_MINO);
            HardDorp();
        }
        else
        {
            // あったら止める
            StopMino();
        }
    }

    /// <summary>
    /// ミノの重複判定
    /// </summary>
    /// <returns>ミノが重複しているかどうか</returns>
    private bool MinodUplication()
    {
        bool isMinoMove = default;
        foreach (Transform chlid in _parentTransform)
        {            
            Vector3 localMinoPos = transform.root.gameObject.transform.InverseTransformPoint(chlid.gameObject.transform.position);
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.CeilToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.CeilToInt(localMinoPos.x);
            // 範囲外または何かあるかを判定
            if(horizontalAxis >= MAP_SIDE_MAX || horizontalAxis <= MAP_SIDE_MIN || _updateMap.Map[verticalAxis, horizontalAxis] != 0)
            {
                // 条件にあったら抜けてreturn
                isMinoMove = false;
                return isMinoMove;
            }
            else if(_updateMap.Map[verticalAxis, horizontalAxis] == 0)
            {
                isMinoMove = true;
            }
        }
        return isMinoMove;
    }

    /// <summary>
    /// ミノの回転処理
    /// </summary>
    /// <param name="moveDirection">ミノの回転方向</param>
    private void MinoRevolution(string moveDirection)
    {
        int n = 0;
        bool isMinoMove = true;

        // 回転させる
        if (transform.localPosition.y < -0.5) 
        {
            if (moveDirection == RIGHT_MOVE_MINO)
            {
                gameObject.transform.localRotation = gameObject.transform.localRotation * _rightRotetion;
            }
            else if (moveDirection == LEFT_MOVE_MINO)
            {
                gameObject.transform.localRotation = gameObject.transform.localRotation * _leftRotetion;
            }
        }
        else // 上にはみ出たらなにもせず返す
        {
            return;
        }

        // ミノの回転した先に別のオブジェクトがある場合
        if(!MinodUplication())
        {
            // Iミノの動かす距離の変動
            if ((_minoCondition == "IMino" && transform.localPosition.x == 9.5f) || (_minoCondition == "IMino" && transform.localPosition.x == 1.5f))
            {
                _moveMino = MOVE_MINO;
            }
            else if ((_minoCondition == "IMino" && transform.localPosition.x == 10.5f) || (_minoCondition == "IMino" && transform.localPosition.x == 0.5f))
            {
                _moveMino = IMINO_MOVE;
            }

            // 重複orはみ出たのが盤面の左右どちら側なのか判定
            foreach (Transform chlid in _parentTransform)
            {
                Vector3 localMinoPos = transform.root.gameObject.transform.InverseTransformPoint(chlid.gameObject.transform.position);
                // 見つけた子オブジェクトのローカル座標を保存
                int verticalAxis = Mathf.CeilToInt(-localMinoPos.y);
                int horizontalAxis = Mathf.CeilToInt(localMinoPos.x);
                if (horizontalAxis >= MAP_SIDE_MAX || horizontalAxis <= MAP_SIDE_MIN || _updateMap.Map[verticalAxis, horizontalAxis] != 0)
                {
                    n = horizontalAxis;
                    break;
                }
            }

            // 重複orはみ出た方向により移動方向の変動
            if (n <= 5)
            {
                transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
            }
            else if(n >= 6)
            {
                transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
            }

            // 移動させてまた重複したら回転できないので最初の回転前まで戻す
            if(!MinodUplication())
            {
                // 左右の位置を処理前に変更
                if (n <= 5)
                {
                    gameObject.transform.localPosition = new Vector2(transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
                }
                else if (n >= 6)
                {
                    gameObject.transform.localPosition = new Vector2(transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
                }
                isMinoMove = false;
            }


            if (!isMinoMove)
            {
                // 回転させる前に変更
                if (moveDirection == RIGHT_MOVE_MINO)
                {
                    gameObject.transform.localRotation = gameObject.transform.localRotation * _leftRotetion;
                }
                else if (moveDirection == LEFT_MOVE_MINO)
                {
                    gameObject.transform.localRotation = gameObject.transform.localRotation * _rightRotetion;
                }
            }
        }
    }

    /// <summary>
    /// ミノの落下を止める処理
    /// </summary>
    private void StopMino()
    {
        while (gameObject.transform.childCount != 0)
        {
            foreach (Transform chlid in _parentTransform)
            {
                chlid.gameObject.transform.parent = _mapObj.transform;
            }
        }
        // オブジェクトの移動
        gameObject.transform.position = new Vector2(100, 100);
        gameObject.transform.parent = _mapObj.transform;
        gameObject.SetActive(false);
        // スコアの増加
        _score.ScoreCount += SCORE_ADD;
    }

    #endregion

}