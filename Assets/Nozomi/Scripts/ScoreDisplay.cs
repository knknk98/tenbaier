using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private int scoreDigit = 8;
    
    private Text scoreText;

    private int score;

    private void Awake()
    {
        score = 0;
        scoreText.text = score.ToString("D" + scoreDigit);
    }

    public void AddScore(int itemId)
    {
        score += ItemScriptalbleObject.SingletonInstance.ItemDataList[itemId].price;
        scoreText.text = score.ToString("D" + scoreDigit);
    }
}
