using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpManager : MonoBehaviour, IPowerUp
{
    private PowerUpType CurrentPowerUp;
    
    [SerializeField] InputAction UsePowerUpAction;
    [SerializeField] float SpeedBoostAcceleration = 100f;
    [SerializeField] float DashImpulse = 25f;
    [SerializeField] float SpinImpulse = 25f;
    
    bool bSpeedBoost = false;
    float SpeedBoostTimer = 0f;
    [SerializeField] float SpeedBoostDuration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        UsePowerUpAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (UsePowerUpAction.triggered)
        {
            UsePowerUp();
        }

        if (bSpeedBoost)
        {
            SpeedBoostTimer += Time.deltaTime;
            if (SpeedBoostTimer >= SpeedBoostDuration)
            {
                bSpeedBoost = false;
                SpeedBoostTimer = 0f;
                gameObject.GetComponent<ICarMovement>().EndSpeedBoost();
            }
        }
    }

    public void UsePowerUp()
    {
        switch (CurrentPowerUp)
        {
            case PowerUpType.None:
                break;
            case PowerUpType.Dash:
            {
                gameObject.GetComponent<ICarMovement>().Dash(DashImpulse);
                break;
            }
                
            case PowerUpType.Health:
                break;
            case PowerUpType.SpeedBoost:
            {
                Debug.Log("SpeedBoost");
                gameObject.GetComponent<ICarMovement>().SpeedBoost(SpeedBoostAcceleration);
                bSpeedBoost = true;
                break;
            }
            case PowerUpType.Spin:
            {
                gameObject.GetComponent<ICarMovement>().Spin(SpinImpulse);
            }
                break;
        }
        CurrentPowerUp = PowerUpType.None;
        Debug.Log("UsePowerUp");
    }

    public void UpdatePowerUp(PowerUpType powerUpType)
    {
        CurrentPowerUp = powerUpType;
        switch (CurrentPowerUp)
        {
            case PowerUpType.Spin:
                UsePowerUp();
                break;
        }
        Debug.Log("PowerUpType: " + CurrentPowerUp);
    }
}

public enum PowerUpType
{
    None,
    Dash,
    Health,
    SpeedBoost,
    Spin,
}
