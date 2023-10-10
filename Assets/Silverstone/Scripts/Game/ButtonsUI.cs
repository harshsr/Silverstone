using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsUI : MonoBehaviour
{
    
    public void Restart()
    {
        Time.timeScale = 1;
        GameObject.FindWithTag("MatchManager").GetComponent<MatchManagerSplit>().RestartTimer();
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
        GameObject.FindWithTag("MatchManager").GetComponent<MatchManagerSplit>().Resume();
    }
}
