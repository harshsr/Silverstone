using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManagerSplit : MonoBehaviour
{
    [SerializeField] public static int MaxLaps = 3;
    [SerializeField] private int StartTimer = 4;
    [SerializeField] private GameObject StartTimerText;
    
    float timer = 0;
    public static bool bIsMatchStarted = false;
    
    
    void Start()
    {
        StartTimerText.SetActive(true);
        timer = StartTimer;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StartTimerText.SetActive(false);
            bIsMatchStarted = true;
        }
        else
        {
            StartTimerText.GetComponent<TMPro.TextMeshProUGUI>().text = ((int)timer).ToString();
        }
    }

    public void PlayerWin(string playerName, bool bIsAI)
    {
        Debug.Log(playerName + " wins!");
    }
}
