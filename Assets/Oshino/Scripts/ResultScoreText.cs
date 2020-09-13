using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ResultScoreText リザルト画面でスコアとハイスコア表示
public class ResultScoreText : MonoBehaviour
{
    // スコア表示テキスト
    public Text highScoreText;
    public Text totalScoreText;

    //　スコア情報保存するための変数
    private int highScore;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.SingletonInstance.PlaySE("result",false,0.3f);
        // スコア情報取得
        highScore = PlayerPrefs.GetInt("HighScore");
        score = PlayerPrefs.GetInt("Score");

        // ハイスコア更新
        if (score>highScore){
            highScore = score;
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = "新記録";
        }
        // スコア表示
        totalScoreText.text = "売却合計￥" + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
