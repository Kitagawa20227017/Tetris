// ---------------------------------------------------------  
// SortingMino.cs  
//   
// 作成日:  2023/11/16
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class SortingMino : MonoBehaviour
{

    #region 変数  

    [SerializeField] private GameObject[] _minoObjs = default;
    private Transform _parentTransform; // 子オブジェクト取得用
    int a = 0;

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary>  
    void Awake()
    {
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
        if(gameObject.transform.childCount == 0)
        {
            Debug.Log("A");
        }
        else
        {
            Debug.Log(gameObject.transform.childCount);
        }
    }

    #endregion

}
