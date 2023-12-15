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
    
    // Iミノの移動処理
    private const int IMINO_MOVE = 2;

    #endregion

    // Mapオブジェクト取得用
    private GameObject _mapObj = default;

    // UpdateMinoMapスクリプト取得用
    private UpdateMinoMap _updateMap = default;

    // 子オブジェクト取得用
    private Transform _parentTransform = default;


    private int _moveMino = 1;


    private string _minoCondition = default;

    // ミノの落ちてくる時間を測るタイマー
    private float _downMinoTimer = 0;　　　　　

    // ミノの形の識別
    private enum Mino
    {
        OMino,
        TMino,
        IMimo,
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
    void Awake()
    {
        // オブジェクト、スクリプトの取得、格納
        _mapObj = GameObject.Find("Map").gameObject;
        _updateMap = GameObject.Find("Map").GetComponent<UpdateMinoMap>();
        _parentTransform = this.gameObject.transform;
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    void Update ()
    {
        // 時間を測る
        _downMinoTimer += Time.deltaTime;
        if (_downMinoTimer >= 1)
        {
            if (DownMove())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - _moveMino);
            }
            else
            {
                StopMino();
            }
            _downMinoTimer = 0;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            HardDorp();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && RotationMino(RIGHT_MOVE_MINO))
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && RotationMino(LEFT_MOVE_MINO))
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TestMe(LEFT_MOVE_MINO);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            TestMe(RIGHT_MOVE_MINO);
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
            int verticalAxis = Mathf.FloorToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.FloorToInt(localMinoPos.x);

            if(moveDirection == RIGHT_MOVE_MINO)
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
            else if(moveDirection == LEFT_MOVE_MINO)
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
            int verticalAxis = Mathf.FloorToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.FloorToInt(localMinoPos.x);
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
            int verticalAxis = Mathf.FloorToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.FloorToInt(localMinoPos.x);
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

        if(isMinoMove)
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - 1);
            HardDorp();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool MinoRevolution()
    {
        bool isMinoMove = false;
        foreach (Transform chlid in _parentTransform)
        {
            isMinoMove = false;
            Vector3 localMinoPos = transform.root.gameObject.transform.InverseTransformPoint(chlid.gameObject.transform.position);
            
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.FloorToInt(localMinoPos.x);

            if(_updateMap.Map[verticalAxis, horizontalAxis] == 0)
            {
                isMinoMove = true;
            }
            else
            {
                isMinoMove = false;
                break;
            }
        }
        return isMinoMove;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moveDirection"></param>
    private void TestMe(string moveDirection)
    {
        int n = 0;
        bool isMinoMove = false;

        // 移動方向
        if (moveDirection == RIGHT_MOVE_MINO)
        {
            gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
        }
        else if (moveDirection == LEFT_MOVE_MINO)
        {
            gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, -90);
        }

        foreach (Transform chlid in _parentTransform)
        {
            isMinoMove = false;
            Vector3 localMinoPos = transform.root.gameObject.transform.InverseTransformPoint(chlid.gameObject.transform.position);
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.FloorToInt(localMinoPos.x);
            if (moveDirection == LEFT_MOVE_MINO)
            {
                if (horizontalAxis >= MINO_SIDE_MIN && horizontalAxis <= MINO_SIDE_MAX &&  _updateMap.Map[verticalAxis, horizontalAxis] == 0 
                    && _minoCondition != "IMino")
                {
                    isMinoMove = true;
                }
                else if (horizontalAxis >= MAP_SIDE_MIN && horizontalAxis <= MAP_SIDE_MAX && _updateMap.Map[verticalAxis, horizontalAxis] != 0
                         && _minoCondition != "IMino")
                {
                    _moveMino = 1;
                    n = horizontalAxis;
                    isMinoMove = false;
                    break;
                }
                else if (horizontalAxis >= MAP_SIDE_MIN && horizontalAxis <= MAP_SIDE_MAX && 
                        verticalAxis >= 2 && _updateMap.Map[verticalAxis, horizontalAxis] == 0
                         && _minoCondition == "IMino")
                {
                    Debug.Log(verticalAxis);
                    isMinoMove = true;
                }
                else if (horizontalAxis >= MAP_SIDE_MIN && horizontalAxis <= MAP_SIDE_MAX && 
                        verticalAxis >= 2 && _updateMap.Map[verticalAxis, horizontalAxis] != 0
                         && _minoCondition == "IMino")
                {
                    Debug.Log(verticalAxis);
                    _moveMino = 2;
                    n = horizontalAxis;
                    isMinoMove = false;
                    break;
                }
            }
            else if (moveDirection == RIGHT_MOVE_MINO)
            {
                if (horizontalAxis >= MINO_SIDE_MIN && horizontalAxis <= MAP_SIDE_MAX && _updateMap.Map[verticalAxis, horizontalAxis] == 0)
                {
                    isMinoMove = true;
                }
                else if (horizontalAxis >= MAP_SIDE_MIN && horizontalAxis <= MAP_SIDE_MAX && _updateMap.Map[verticalAxis, horizontalAxis] != 0)
                {
                    n = horizontalAxis;
                    isMinoMove = false;
                    break;
                }
            }
        }

        if (isMinoMove)
        {
            return;
        }
        else if (!isMinoMove && moveDirection == LEFT_MOVE_MINO && n <= MAP_SIDE_MIN)
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
            if (!MinoRevolution())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
            }
        }
        else if (!isMinoMove && moveDirection == LEFT_MOVE_MINO && n >= MAP_SIDE_MAX)
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
            if (!MinoRevolution())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
            }
        }
        else if (!isMinoMove && moveDirection == RIGHT_MOVE_MINO && n <= MAP_SIDE_MIN)
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
            if (!MinoRevolution())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, -90);
            }
        }
        else if (!isMinoMove && moveDirection == RIGHT_MOVE_MINO && n >= MAP_SIDE_MAX)
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
            if (!MinoRevolution())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, -90);
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
