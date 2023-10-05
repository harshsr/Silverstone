using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarEffects
{
    void EmmitDriftTrail();
    void StopDriftTrail();
    
    void EmmitSpeedBoostParticles();
    void StopSpeedBoostParticles();
    
    void EmmitDashParticles();
}
