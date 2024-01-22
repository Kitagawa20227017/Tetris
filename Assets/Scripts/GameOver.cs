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

    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _textUI;
    [SerializeField] private GameObject _map;
    private bool _isFin = false; 

    #endregion

    #region プロパティ  

    public bool IsFin
    {
        get => _isFin;
    }


    #endregion


    private void Start()
    {
        _gameOverUI.SetActive(false);
        _textUI.SetActive(false);
    }

    public void GameOverJudgment()
    {
        gameObject.SetActive(false);
        _map.SetActive(false);
        _gameOverUI.SetActive(true);
        _textUI.SetActive(true);
        _isFin = true;
    }
}
