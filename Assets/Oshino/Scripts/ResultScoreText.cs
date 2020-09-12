using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ResultScoreText リザルト画面でスコアとハイスコア表示
public class ResultScoreText : MonoBehaviour
{
    // スコア表示テキスト
    public Text highScoreText;
    public Text scoreText;

    //　スコア情報保存するための変数
    private int highScore;
    private int score;
    private string highScoreKey = "HighScore";   //ハイスコアの保存先キー
    private string scoreKey = "Score"; //スコアの保存キー

    // Start is called before the first frame update
    void Start()
    {
        // スコア情報取得
        highScore = PlayerPrefs.GetInt(highScoreKey,0);
        score = PlayerPrefs.GetInt(scoreKey,0);

        // ハイスコア更新
        if (score>highScore){
            highScore = score;
            PlayerPrefs.SetInt(highScoreKey, highScore);
        }

        // スコア表示
        scoreText.text = "score:" + score.ToString();
        highScoreText.text = "HighScore:" + highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
