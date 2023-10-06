using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyCarMovement
{
    bool IsGrounded();
    Vector3 GetAverageNormal();
 
}
