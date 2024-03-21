// ---------------------------------------------------------  
// DeletionObj.cs  
//   
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class DeletionObj : MonoBehaviour
{

    #region 変数

    // 子オブジェクト取得用
    private Transform _parentTransform = default;
    private const int DESTYOY_CONUT = 100;
    
    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary>  
    private void Awake()
    {
        _parentTransform = this.gameObject.transform;
    }

    public void DestyoyObj()
    {
        if(gameObject.transform.childCount >= DESTYOY_CONUT)
        {
            foreach(Transform chlid in _parentTransform)
            {
                Destroy(chlid.gameObject);
            }
        }
    }

    #endregion

}
