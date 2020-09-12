using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    public Text highScoreText;          //ハイスコアを表示するText
    private int highScore;              //ハイスコア用変数
    private string key = "HighScore";   //ハイスコアの保存先キー

    // Start is called before the first frame update
    void Start()
    {
        // ハイスコア情報取得。情報がなければ0にする
        highScore = PlayerPrefs.GetInt(key,0);
        // ハイスコア表示
        highScoreText.text = "HighScore:" + highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
