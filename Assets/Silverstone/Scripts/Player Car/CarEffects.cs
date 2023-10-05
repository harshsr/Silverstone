using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour, ICarEffects
{
    TrailRenderer[] DriftTrails;
    AudioSource EngineAudioSource;
    public float CarSpeedDivisor = 30f;
    
    // Start is called before the first frame update
    void Start()
    {
        DriftTrails = gameObject.GetComponentsInChildren<TrailRenderer>();
        EngineAudioSource = gameObject.GetComponent<AudioSource>();
 
    }

    // Update is called once per frame
    void Update()
    {
        EngineAudio();
    }
    
    void EngineAudio()
    {
        float Pitchfactor = gameObject.GetComponentInParent<ICarMovement>().GetSpeed() / CarSpeedDivisor;
        Pitchfactor = Mathf.Clamp(Pitchfactor, 0.2f, 1.2f);
        EngineAudioSource.pitch = Pitchfactor;
    }
    
    
    public void EmmitDriftTrail()
    {
        foreach (TrailRenderer Trail in DriftTrails)
        {
            Trail.emitting = true;
        }
    }
    
    public void StopDriftTrail()
    {
        foreach (TrailRenderer Trail in DriftTrails)
        {
            Trail.emitting = false;
        }
    }
}
