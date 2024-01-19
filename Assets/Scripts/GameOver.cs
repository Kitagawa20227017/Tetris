// ---------------------------------------------------------  
// GameOver.cs  
//   
// 作成日:  2023/12/5
// 作成者:  北川 稔明
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{

    #region 変数

    private bool _isFin = false; 

    #endregion

    #region プロパティ  

    public bool IsFin
    {
        get => _isFin;
    }


    #endregion


    public void GameOverJudgment()
    {
        gameObject.SetActive(false);
        _isFin = true;
    }
}
