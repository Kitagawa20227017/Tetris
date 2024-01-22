// ---------------------------------------------------------  
// MoveGameScene.cs  
//   
// 作成日:  
// 作成者:  北川 稔明
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveGameScene : MonoBehaviour
{

    #region 変数  

    // 矢印の位置
    readonly private Vector2 _play = new Vector2(-2.5f, -0.5f);
    readonly private Vector2 _quit = new Vector2(-2.5f, -3f);

    // 矢印オブジェクト
    [SerializeField] private GameObject _arrow;

    // Playに矢印があるかどうか
    private bool _isPlay = true;

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    void Start ()
    {
        PlayUI();
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    void Update ()
    {
        if(Input.GetButtonDown("DownMove"))
        {
            _isPlay = false;
            QiutUI();
        }
        else if(Input.GetButtonDown("UpMove"))
        {
            _isPlay = true;
            PlayUI();
        }

        if(Input.GetButtonDown("Enter"))
        {
            if(_isPlay)
            {
                SceneManager.LoadScene("GameScene");
            }
            else if(!_isPlay)
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;        
                #else
                Application.Quit();
                #endif
            }
        }
    }

    private void PlayUI()
    {
        _arrow.transform.position = _play;
    }

    private void QiutUI()
    {
        _arrow.transform.position = _quit;
    }

    #endregion

}
