using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScoreText : MonoBehaviour
{
    public Text highScoreText;          //ハイスコアを表示するText
    private int highScore;              //ハイスコア用変数

    // Start is called before the first frame update
    void Start()
    {
        // ハイスコア情報取得。情報がなければ0にする
        highScore = PlayerPrefs.GetInt("HighScore");
        // ハイスコア表示
        highScoreText.text = "最高売却額：￥" + highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
