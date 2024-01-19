// ---------------------------------------------------------  
// MoveScene.cs  
//   
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MoveScene : MonoBehaviour
{

    #region 変数

    private GameOver _gameOver = default;
    private SceneManager _sceneManager;
    private string qqq;

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary>  
    void Awake()
    {
        _gameOver = GameObject.Find("PlayerMap").GetComponent<GameOver>();
        qqq = SceneManager.GetActiveScene().name;
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
        if(_gameOver.IsFin && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(qqq);
        }
    }

    #endregion

}
