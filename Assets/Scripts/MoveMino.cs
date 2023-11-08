// ---------------------------------------------------------  
// PlayerInput.cs  
//   
// 作成日:  2023/11/2
// 作成者:  北川稔明
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class MoveMino : MonoBehaviour
{

    #region 変数  

    private GameObject _mapObj;
    private UpdateMinoMap _updateMap;
    private Transform _parentTransform; // 子オブジェクト取得用

    private float _time = 0;

    public enum Mino
    {
        OMino,
        TMino,
        IMimo,
        LMino,
        JMino,
        SMino,
        ZMino
    }

    [SerializeField] private Mino _mino;

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

    private void FixedUpdate()
    {
        _time += Time.deltaTime;
        if(_time >= 1)
        {
            if(DownMove())
            {
                gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - 1);
            }
            _time = 0;
        }
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    void Update ()
    {
        if (!DownMove())
        { 
            while (gameObject.transform.childCount != 0)
            {
                foreach (Transform chlid in _parentTransform)
                {
                    chlid.gameObject.transform.parent = _mapObj.transform;
                }
            }
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && RightMove())
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + 1, gameObject.transform.localPosition.y);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && LeftMove())
        {
            gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 1, gameObject.transform.localPosition.y);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {

        }

    }

    private bool RightMove()
    {
        bool isMinoMove = false;
        foreach (Transform chlid in _parentTransform)
        {
            isMinoMove = false;
            Vector3 localMinoPos = transform.root.gameObject.transform.InverseTransformPoint(chlid.gameObject.transform.position);
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.FloorToInt(localMinoPos.x);

            if(_updateMap.Map[verticalAxis,horizontalAxis + 1] == 0)
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

    private bool LeftMove()
    {
        bool isMinoMove = false;
        foreach (Transform chlid in _parentTransform)
        {
            isMinoMove = false;
            Vector3 localMinoPos = transform.root.gameObject.transform.InverseTransformPoint(chlid.gameObject.transform.position);
            // 見つけた子オブジェクトのローカル座標を保存
            int verticalAxis = Mathf.FloorToInt(-localMinoPos.y);
            int horizontalAxis = Mathf.FloorToInt(localMinoPos.x);

            if (_updateMap.Map[verticalAxis, horizontalAxis -1] == 0)
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

    #endregion

}
