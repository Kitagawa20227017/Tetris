// ---------------------------------------------------------  
// PlayerInput.cs
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
    private const int MOVEMINO = 1;

    // Iミノの移動処理
    private const int IMINO_MOVE = 2;

    #endregion

    // Mapオブジェクト取得用
    private GameObject _mapObj = default;

    // UpdateMinoMapスクリプト取得用
    private UpdateMinoMap _updateMap = default;

    // 子オブジェクト取得用
    private Transform _parentTransform = default;

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
        // 時間を測る
        _downMinoTimer += Time.deltaTime;
        if (_downMinoTimer >= _minoTimer)
        {
            if (DownMove())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - MOVEMINO);
            }
            else
            {
                StopMino();
            }
            _downMinoTimer = 0;
        }

        if (Input.GetButtonDown("HardDrop"))
        {
            HardDorp();
        }

        if (Input.GetKey(KeyCode.S))
        {
            _minoTimer = 0.25f;
        }
        else
        {
            _minoTimer = 1f;
        }

        if (Input.GetButtonDown("RightMove") && RotationMino(RIGHT_MOVE_MINO))
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + MOVEMINO, gameObject.transform.localPosition.y);
        }
        else if (Input.GetButtonDown("LeftMove") && RotationMino(LEFT_MOVE_MINO))
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - MOVEMINO, gameObject.transform.localPosition.y);
        }

        if (Input.GetButtonDown("LeftRotation"))
        {
            MinoRevolution(LEFT_MOVE_MINO);
        }
        else if (Input.GetButtonDown("RightRotation"))
        {
            MinoRevolution(RIGHT_MOVE_MINO);
        }
    }

    /// <summary>
    /// ミノの左右移動処理
    /// </summary>
    /// <param name="moveDirection"></param>
    /// <returns></returns>
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

            if (moveDirection == RIGHT_MOVE_MINO)
            {
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
    /// 
    /// </summary>
    /// <returns></returns>
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
    /// 
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
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - MOVEMINO);
            HardDorp();
        }
        else
        {
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
            if(horizontalAxis >= MAP_SIDE_MAX || horizontalAxis <= MAP_SIDE_MIN || _updateMap.Map[verticalAxis, horizontalAxis] != 0)
            {
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
        if (transform.localPosition.y < -0.5) // 上にはみ出たらなにもせず返す
        {
            if (moveDirection == RIGHT_MOVE_MINO)
            {
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
            }
            else if (moveDirection == LEFT_MOVE_MINO)
            {
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, -90);
            }
        }
        else
        {
            return;
        }

        // ミノの回転した先に別のオブジェクトがある場合
        if(!MinodUplication())
        {
            // Iミノの動かす距離の変動
            if ((_minoCondition == "IMino" && transform.localPosition.x == 9.5f) || (_minoCondition == "IMino" && transform.localPosition.x == 1.5f))
            {
                _moveMino = MOVEMINO;
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
                    gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, -90);
                }
                else if (moveDirection == LEFT_MOVE_MINO)
                {
                    gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
                }
            }
        }
    }

    /// <summary>
    /// 
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
        gameObject.transform.position = new Vector2(100, 100);
        gameObject.transform.parent = _mapObj.transform;
        gameObject.SetActive(false);
    }

    #endregion

}