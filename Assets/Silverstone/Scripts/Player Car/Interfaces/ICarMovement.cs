
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarMovement
{
    void Dash(float DashImpulse);
    void Spin();
    void SpeedBoost(float BoostedAcceleration);
    void EndSpeedBoost();
    
    float GetSpeed();
}
