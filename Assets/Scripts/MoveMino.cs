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

    private const string _RIGHTMOVEMINO = "Right"; // 右方向の移動指標
    private const string _LEFTMOVEMINO = "Left";   // 左方向の移動指標
    private const int MAPSIDEMAX = 11;
    private const int MAPSIDEMIN = 0;
    private const int MINOSIDEMAX = 10;
    private const int MINOSIDEMIN = 1;
    private const int IMINOMOVE = 2;


    private GameObject _mapObj = default;          // Mapオブジェクト取得用
    private UpdateMinoMap _updateMap = default;    // UpdateMinoMapスクリプト取得用
    private Transform _parentTransform = default;  // 子オブジェクト取得用

    private int _moveMino = 1;
    private string _minoCondition = default;
    private float _downMinoTimer = 0;　　　　　　　// ミノの落ちてくる時間を測るタイマー

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

    [SerializeField] private Mino _minoType; // 選択できるようにする

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
        _downMinoTimer += Time.deltaTime; // 時間を測る
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

        if (Input.GetKeyDown(KeyCode.RightArrow) && RotationMino(_RIGHTMOVEMINO))
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && RotationMino(_LEFTMOVEMINO))
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TestMe(_LEFTMOVEMINO);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            TestMe(_RIGHTMOVEMINO);
        }
    }

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

            if(moveDirection == _RIGHTMOVEMINO)
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
            else if(moveDirection == _LEFTMOVEMINO)
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

    private void TestMe(string moveDirection)
    {
        int n = 0;
        bool isMinoMove = false;


        if (moveDirection == _RIGHTMOVEMINO)
        {
            gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
        }
        else if (moveDirection == _LEFTMOVEMINO)
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
            if (moveDirection == _LEFTMOVEMINO)
            {
                if (horizontalAxis >= MINOSIDEMIN && horizontalAxis <= MINOSIDEMAX &&  _updateMap.Map[verticalAxis, horizontalAxis] == 0 
                    && _minoCondition != "IMino")
                {
                    isMinoMove = true;
                }
                else if (horizontalAxis >= MAPSIDEMIN && horizontalAxis <= MAPSIDEMAX && _updateMap.Map[verticalAxis, horizontalAxis] != 0
                         && _minoCondition != "IMino")
                {
                    _moveMino = 1;
                    n = horizontalAxis;
                    isMinoMove = false;
                    break;
                }
                else if (horizontalAxis >= MAPSIDEMIN && horizontalAxis <= MAPSIDEMAX && verticalAxis >= 2 && _updateMap.Map[verticalAxis, horizontalAxis] == 0
                         && _minoCondition == "IMino")
                {
                    Debug.Log(verticalAxis);
                    isMinoMove = true;
                }
                else if (horizontalAxis >= MAPSIDEMIN && horizontalAxis <= MAPSIDEMAX && verticalAxis >= 2 && _updateMap.Map[verticalAxis, horizontalAxis] != 0
                         && _minoCondition == "IMino")
                {
                    Debug.Log(verticalAxis);
                    _moveMino = 2;
                    n = horizontalAxis;
                    isMinoMove = false;
                    break;
                }
            }
            else if (moveDirection == _RIGHTMOVEMINO)
            {
                if (horizontalAxis >= MINOSIDEMIN && horizontalAxis <= MAPSIDEMAX && _updateMap.Map[verticalAxis, horizontalAxis] == 0)
                {
                    isMinoMove = true;
                }
                else if (horizontalAxis >= MAPSIDEMIN && horizontalAxis <= MAPSIDEMAX && _updateMap.Map[verticalAxis, horizontalAxis] != 0)
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
        else if (!isMinoMove && moveDirection == _LEFTMOVEMINO && n <= MAPSIDEMIN)
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
            if (!MinoRevolution())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
            }
        }
        else if (!isMinoMove && moveDirection == _LEFTMOVEMINO && n >= MAPSIDEMAX)
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
            if (!MinoRevolution())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
            }
        }
        else if (!isMinoMove && moveDirection == _RIGHTMOVEMINO && n <= MAPSIDEMIN)
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
            if (!MinoRevolution())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, -90);
            }
        }
        else if (!isMinoMove && moveDirection == _RIGHTMOVEMINO && n >= MAPSIDEMAX)
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - _moveMino, gameObject.transform.localPosition.y);
            if (!MinoRevolution())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + _moveMino, gameObject.transform.localPosition.y);
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, -90);
            }
        }
    }

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
