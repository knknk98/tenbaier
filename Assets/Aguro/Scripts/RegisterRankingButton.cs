using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterRankingButton : MonoBehaviour
{
    public void RegisterRankingButtonClicked()
    {
        int score = PlayerPrefs.GetInt("Score");
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking (score);
    }
}
