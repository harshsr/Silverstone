using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManage : MonoBehaviour,IUI
{
    [SerializeField] GameObject NoPowerupImage;
    [SerializeField] GameObject SpeedUpImage;
    [SerializeField] GameObject DashImage;
    [SerializeField] TextMeshProUGUI PowerUpText;
    [SerializeField] TextMeshProUGUI LapText;
    
    void Start()
    {
        NoPowerupImage.SetActive(true);
        SpeedUpImage.SetActive(false);
        DashImage.SetActive(false);
    }
    
    void Update()
    {
        
    }
    
    void IUI.UpdatePowerUp(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.None:
                NoPowerupImage.SetActive(true);
                SpeedUpImage.SetActive(false);
                DashImage.SetActive(false);
                PowerUpText.text = "No Power Up";
                break;
            case PowerUpType.SpeedBoost:
                NoPowerupImage.SetActive(false);
                SpeedUpImage.SetActive(true);
                DashImage.SetActive(false);
                PowerUpText.text = "Speed Boost";
                break;
            case PowerUpType.Dash:
                NoPowerupImage.SetActive(false);
                SpeedUpImage.SetActive(false);
                DashImage.SetActive(true);
                PowerUpText.text = "Dash";
                break;
        }
    }
    
    void IUI.LapUp(int lapCount)
    {
        LapText.text = lapCount.ToString() + "/" + GameObject.FindWithTag("MatchManager").GetComponent<MatchManagerSplit>().GetMaxLaps().ToString();
    }
}
