using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUI
{ 
    void UpdatePowerUp(PowerUpType powerUpType);
    void LapUp(int lapCount);
}

