// ---------------------------------------------------------  
// GameUI.cs  
//   
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    #region 変数  

    // 矢印の位置
    readonly private Vector2 _title = new Vector2(8f, -4.5f);
    readonly private Vector2 _quit = new Vector2(8f, -6.5f);

    private Score _score = default;

    // 矢印オブジェクト
    [SerializeField] private GameObject _arrow;

    [SerializeField] private Text _text;

    // Playに矢印があるかどうか
    private bool _isTitle = true;

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    void Start()
    {
        _score = GameObject.Find("PlayerMap").GetComponent<Score>();
        PlayUI();
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    void Update()
    {
        _text.text = _score.ScoreCount.ToString();

        if (Input.GetButtonDown("DownMove"))
        {
            _isTitle = false;
            QiutUI();
        }
        else if (Input.GetButtonDown("UpMove"))
        {
            _isTitle = true;
            PlayUI();
        }

        if (Input.GetButtonDown("Enter"))
        {
            if (_isTitle)
            {
                SceneManager.LoadScene("TitleScene");
            }
            else if (!_isTitle)
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
        _arrow.transform.position = _title;
    }

    private void QiutUI()
    {
        _arrow.transform.position = _quit;
    }

    #endregion

}
