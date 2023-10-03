using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpManager : MonoBehaviour, IPowerUp
{
    private PowerUpType CurrentPowerUp;
    
    [SerializeField] InputAction UsePowerUpAction;
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
    }

    public void UsePowerUp()
    {
        switch (CurrentPowerUp)
        {
            case PowerUpType.None:
                break;
            case PowerUpType.Dash:
                gameObject.GetComponent<ICarMovement>().Dash();
                break;
            case PowerUpType.Health:
                break;
            case PowerUpType.SpeedBoost:
                break;
            case PowerUpType.Spin:
                break;
        }
        CurrentPowerUp = PowerUpType.None;
        Debug.Log("UsePowerUp");
    }

    public void UpdatePowerUp(PowerUpType powerUpType)
    {
        CurrentPowerUp = powerUpType;
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
