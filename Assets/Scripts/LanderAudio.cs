using System;
using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrustAudioSource;
    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();
        
    }
    private void Start()
    {
        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        thrustAudioSource.Pause();
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        if(!thrustAudioSource.isPlaying)
        thrustAudioSource.Play();
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        if(!thrustAudioSource.isPlaying)
        thrustAudioSource.Play();
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        if(!thrustAudioSource.isPlaying)
        thrustAudioSource.Play();
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        thrustAudioSource.Pause();
    }
}
