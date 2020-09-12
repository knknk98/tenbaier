using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Item : MonoBehaviour
{
    [SerializeField] private int itemId;
    [SerializeField] private ScoreDisplay scoreDisplay;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO
            scoreDisplay.AddScore(itemId);
            Destroy(this.gameObject);
        }
    }
}
