using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameSession : MonoBehaviour
{
    int currentScore = 0;
    int currentHealth;

    private void Awake()
    {
        SingletonForScore();
    }

    private void SingletonForScore()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        { Destroy(gameObject); }
        else
        { DontDestroyOnLoad(gameObject); }
    }

    public int GetScore()
    {
        return currentScore;
    }

    public void AddToScore(int scoreValue)
    {
        currentScore += scoreValue;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

}
