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

    private const string _RIGHTMOVEMINO = "Right";
    private const string _LEFTMOVEMINO = "Left";

    private GameObject _mapObj = default;
    private UpdateMinoMap _updateMap = default;
    private Transform _parentTransform = default; // 子オブジェクト取得用

    private float _downMinoTimer = 0;

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
        _mapObj = GameObject.Find("Map").gameObject;
        _updateMap = GameObject.Find("Map").GetComponent<UpdateMinoMap>();
        _parentTransform = this.gameObject.transform;
    }
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    void Start ()
    {

    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    void Update ()
    {
        _downMinoTimer += Time.deltaTime;
        if (_downMinoTimer >= 1)
        {
            if (DownMove())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - 1);
            }
            else
            {
                StopMino();
            }
            _downMinoTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && RotationMino(_RIGHTMOVEMINO))
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + 1, gameObject.transform.localPosition.y);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && RotationMino(_LEFTMOVEMINO))
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 1, gameObject.transform.localPosition.y);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0,0,-90);
            if (!MinoRevolution())
            {
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
            }
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, 90);
            if (!MinoRevolution())
            {
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(0, 0, -90);
            }
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
                if (horizontalAxis <= 23 && _updateMap.Map[verticalAxis, horizontalAxis + 1] == 0)
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
                if (horizontalAxis >= 1 && _updateMap.Map[verticalAxis, horizontalAxis - 1] == 0)
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
            if (horizontalAxis >= 1 && horizontalAxis <= 10 && verticalAxis >= 1 && verticalAxis <= 22 &&_updateMap.Map[verticalAxis , horizontalAxis] == 0)
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

    private void StopMino()
    {
        while (gameObject.transform.childCount != 0)
        {
            foreach (Transform chlid in _parentTransform)
            {
                chlid.gameObject.transform.parent = _mapObj.transform;
            }
        }
        gameObject.transform.parent = _mapObj.transform;
        gameObject.SetActive(false);
    }

    #endregion

}
