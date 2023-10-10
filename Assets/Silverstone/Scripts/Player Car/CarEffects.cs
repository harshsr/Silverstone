using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour, ICarEffects
{
    TrailRenderer[] DriftTrails;
    AudioSource EngineAudioSource;
    public float CarSpeedDivisor = 30f;
    [SerializeField] ParticleSystem SpeedBoostParticles;
    [SerializeField] ParticleSystem DashParticles;
    
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
        if (Time.timeScale == 0)
        {
            EngineAudioSource.pitch = 0;
        }
        else
        {
            float Pitchfactor = gameObject.GetComponentInParent<ICarMovement>().GetSpeed() / CarSpeedDivisor;
            Pitchfactor = Mathf.Clamp(Pitchfactor, 0.2f, 1.2f);
            EngineAudioSource.pitch = Pitchfactor;
        }
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
    
    public void EmmitSpeedBoostParticles()
    {
        SpeedBoostParticles.Play();
    }
    
    public void StopSpeedBoostParticles()
    {
        SpeedBoostParticles.Stop();
    }
    
    public void EmmitDashParticles()
    {
        DashParticles.Play();
    }
}
