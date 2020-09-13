using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour
{
    private int itemId;
    private ScoreDisplay scoreDisplay;

    private void Awake()
    {
        var itemCount = ItemScriptalbleObject.SingletonInstance.ItemDataList.Count;
        itemId = UnityEngine.Random.Range(0, itemCount);
        GetComponent<SpriteRenderer>().sprite = ItemScriptalbleObject.SingletonInstance.ItemDataList[itemId].thumbnail;
        scoreDisplay = GameObject.FindWithTag("ScoreDisplay").GetComponent<ScoreDisplay>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            scoreDisplay.AddScore(itemId);
            ScoreManager.SingletonInstance.AddItem(itemId, 1);
            Destroy(this.gameObject);
        }
    }
}
