// ---------------------------------------------------------  
// GameControl.cs  
//   
// 作成日:  2023/10/24
// 作成者:  北川稔明
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{

    #region 変数  

    private UpdateMinoMap _updateMap;

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary>  
    void Awake()
    {
        _updateMap = GameObject.Find("Map").GetComponent<UpdateMinoMap>();
    }
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    void Start ()
    {
        _updateMap.SearchMino();
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    void Update ()
    {
        _updateMap.SearchMino();
    }

    #endregion

}
