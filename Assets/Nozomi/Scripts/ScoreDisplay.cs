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
        scoreText = GetComponent<Text>();
        Display();
    }

    public void AddScore(int itemId)
    {
        score += ItemScriptalbleObject.SingletonInstance.ItemDataList[itemId].price;
        Display();
    }

    private void Display()
    {
        scoreText.text = "売却額 ￥ " + score.ToString("D" + scoreDigit);
    }
}
