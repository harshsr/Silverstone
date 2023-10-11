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
    [SerializeField] bool bIsAI = false;
    
    [SerializeField] string PlayerName;

    [SerializeField] private InputAction ResetCar;
    
    MatchManager MatchManager;
    
    void Start()
    {
        MatchManager = GameObject.FindWithTag("MatchManager").GetComponent<MatchManager>();
        MaxLaps = MatchManager.GetMaxLaps();
        ResetCar.Enable();
        LastCheckpointInfo.CheckpointIndex = -1;
        LastCheckpointInfo.CheckpointPosition = transform.position;
        LastCheckpointInfo.CheckpointForward = transform.forward;
        
        NumberOfCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
        if (!bIsAI)
        {
            gameObject.GetComponentInParent<IUI>().LapUp(LapCount);

        }
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
            if (!bIsAI)
            {
                gameObject.GetComponentInParent<IUI>().LapUp(LapCount);
            }
            LastCheckpointInfo = CheckpointInfo;
            if (LapCount == MaxLaps)
            {
                PlayerWins();
            }

            if (!bIsAI)
            {
                foreach (var PowerUp in LastCheckpointInfo.PowerUpsToReset)
                {
                    PowerUp.SetActive(true);
                }
            }
        }
        else if (CheckpointInfo.CheckpointIndex == LastCheckpointInfo.CheckpointIndex+1)
        {
            LastCheckpointInfo = CheckpointInfo;
            if (!bIsAI)
            {
                foreach (var PowerUp in LastCheckpointInfo.PowerUpsToReset)
                {
                    PowerUp.SetActive(true);
                }
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
        if (bIsAI)
        {
            gameObject.GetComponent<IEnemyCarMovement>().ResetCalled(LastCheckpointInfo.NextAIWaypointIndex);
        }
    }

    void PlayerWins()
    {
        MatchManager.PlayerWin(PlayerName, bIsAI);
    }
}
