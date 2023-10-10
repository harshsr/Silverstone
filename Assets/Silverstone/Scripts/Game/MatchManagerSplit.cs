using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchManagerSplit : MonoBehaviour
{
    [SerializeField] public int MaxLaps = 3;
    [SerializeField] private int StartTimer = 4;
    [SerializeField] private GameObject StartTimerText;
    
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject Player1WinImage;
    [SerializeField] GameObject Player2WinImage;
    [SerializeField] GameObject ComputerWinImage;
    
    [SerializeField] GameObject[] PlayerHuds;
    
    [SerializeField] GameObject PausePanel;
    
    [SerializeField] InputAction PauseAction;
    
    float timer = 0;
    public static bool bIsMatchStarted = false;
    
    
    void Awake()
    {
        RestartTimer();
    }
    void Start()
    {
        StartTimerText.SetActive(true);
        timer = StartTimer;
        PauseAction.Enable();
        PausePanel.SetActive(false);
        WinPanel.SetActive(false);
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
        
        if (PauseAction.triggered)
        {
            if (PausePanel.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void PlayerWin(string playerName, bool bIsAI)
    {
        if (bIsAI)
        {
            WinPanel.SetActive(true);
            ComputerWinImage.SetActive(true);
            Player1WinImage.SetActive(false);
            if (Player2WinImage != null)
            {
                Player2WinImage.SetActive(false);
            }
           
        }
        else if (playerName == "Player A")
        {
            WinPanel.SetActive(true);
            Player1WinImage.SetActive(true);
            if (Player2WinImage != null)
            {
                Player2WinImage.SetActive(false);
            }
            ComputerWinImage.SetActive(false);
        }
        else if (playerName == "Player B")
        {
            WinPanel.SetActive(true);
            Player1WinImage.SetActive(false);
            if (Player2WinImage != null)
            {
                Player2WinImage.SetActive(true);
            }
            ComputerWinImage.SetActive(false);
        }
        Pause();
    }
    
    void Pause()
    {
        PausePanel.SetActive(true);
        foreach (GameObject playerHud in PlayerHuds)
        {
            playerHud.SetActive(false);
        }
        Time.timeScale = 0;
    }
    
    public void Resume()
    {
        PausePanel.SetActive(false);
        foreach (GameObject playerHud in PlayerHuds)
        {
            playerHud.SetActive(true);
        }
        Time.timeScale = 1;
    }
    
    public void RestartTimer()
    {
        timer = StartTimer;
        StartTimerText.SetActive(true);
        bIsMatchStarted = false;
    }
    
    public int GetMaxLaps()
    {
        return MaxLaps;
    }
}
