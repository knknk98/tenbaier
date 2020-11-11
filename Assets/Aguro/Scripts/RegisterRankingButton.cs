using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterRankingButton : MonoBehaviour
{
    public void RegisterRankingButtonClicked()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking (highScore);
    }
}
