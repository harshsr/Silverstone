using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsUI : MonoBehaviour
{
    [SerializeField] GameObject LevelSelectPanel;
    [SerializeField] GameObject MainMenuPanel;

    private void Start()
    {
        if (MainMenuPanel && LevelSelectPanel)
        {
            MainMenuPanel.SetActive(true);
            LevelSelectPanel.SetActive(false);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        GameObject.FindWithTag("MatchManager").GetComponent<MatchManager>().RestartTimer();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
    public void Resume()
    {
        Time.timeScale = 0;
        GameObject.FindWithTag("MatchManager").GetComponent<MatchManager>().Resume();
    }
    
    public void SelectLevel()
    {
       
    }
    
    public void PlaySinglePlayer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    
    public void PlaySplitScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    
    public void Back()
    {
        MainMenuPanel.SetActive(true);
        LevelSelectPanel.SetActive(false);
    }
    
    public void Play()
    {
        MainMenuPanel.SetActive(false);
        LevelSelectPanel.SetActive(true);
    }
}
