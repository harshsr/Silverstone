using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{ 
    [SerializeField] CheckpointInfo CheckpointInfo;
    
    void Start()
    {
        CheckpointInfo.CheckpointPosition = transform.position;
        CheckpointInfo.CheckpointForward = transform.forward;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<ICarGameState>().UpdateCheckpoint(CheckpointInfo);
        }
    }
}

[Serializable]
public struct CheckpointInfo
{
    public int CheckpointIndex;
    public Vector3 CheckpointPosition;
    public Vector3 CheckpointForward;
}