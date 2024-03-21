// ---------------------------------------------------------  
// Score.cs  
//   
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour
{

    #region 変数  

    private int scoreCount = 0;
    private Text text;

    #endregion

    #region プロパティ
    public int ScoreCount
    {
        get => scoreCount;
        set => scoreCount = value;
    }
    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary>  
    void Awake()
    {
        text = GameObject.Find("Text").GetComponent<Text>();
    }
     
    /// <summary>  
    /// 更新処理  
    /// </summary>  
    void Update ()
    {
        text.text = scoreCount.ToString();
    }

    #endregion

}
