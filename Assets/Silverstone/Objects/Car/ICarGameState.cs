using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarGameState
{
  void UpdateCheckpoint(CheckpointInfo CheckpointInfo);
  
  void ResetToLastCheckpoint();
}
