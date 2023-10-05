using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarGameState : MonoBehaviour, ICarGameState
{
    CheckpointInfo LastCheckpointInfo;
    int NumberOfCheckpoints;
    int LapCount;
    int MaxLaps = 3;

    [SerializeField] private InputAction ResetCar;
    
    void Start()
    {
        ResetCar.Enable();
        LastCheckpointInfo.CheckpointIndex = -1;
        LastCheckpointInfo.CheckpointPosition = transform.position;
        LastCheckpointInfo.CheckpointForward = transform.forward;
        
        NumberOfCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
    }
    
    void Update()
    {
        if (ResetCar.triggered)
        {
            ResetToLastCheckpoint();
        }
    }

    public void UpdateCheckpoint(CheckpointInfo CheckpointInfo)
    {
        if (CheckpointInfo.CheckpointIndex == 0 && LastCheckpointInfo.CheckpointIndex == NumberOfCheckpoints-1)
        {
            //Lap completed
            LapCount++;
            LastCheckpointInfo = CheckpointInfo;
            if (LapCount == MaxLaps)
            {
                PlayerWins();
            }

            foreach (var PowerUp in LastCheckpointInfo.PowerUpsToReset)
            {
                PowerUp.SetActive(true);
            }
        }
        else if (CheckpointInfo.CheckpointIndex == LastCheckpointInfo.CheckpointIndex+1)
        {
            LastCheckpointInfo = CheckpointInfo;
            foreach (var PowerUp in LastCheckpointInfo.PowerUpsToReset)
            {
                PowerUp.SetActive(true);
            }
        }
        else
        {
            
        }
    }
    
    public void ResetToLastCheckpoint()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = LastCheckpointInfo.CheckpointPosition;
        transform.forward = LastCheckpointInfo.CheckpointForward;
    }
    
    void PlayerWins()
    {
        Debug.Log("PlayerWins");
    }
}
