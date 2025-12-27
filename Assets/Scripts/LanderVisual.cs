using System;
using UnityEngine;

public class LanderVisual : MonoBehaviour
{
   [SerializeField] private ParticleSystem leftThrusterParticleSystem;
   [SerializeField] private ParticleSystem rightThrusterParticleSystem;
   [SerializeField] private ParticleSystem middleThrusterParticleSystem;
   [SerializeField] private GameObject landerExplosionVFX;
   private Lander lander;
    private void Awake()
    {
         lander = GetComponent<Lander>();
         lander.OnUpForce += Lander_OnUpForce;
         lander.OnLeftForce += Lander_OnLeftForce;
         lander.OnRightForce += Lander_OnRightForce;
         lander.OnBeforeForce += Lander_OnBeforeForce;

        SetEnableThruster(leftThrusterParticleSystem, false);
        SetEnableThruster(middleThrusterParticleSystem, false);
        SetEnableThruster(rightThrusterParticleSystem, false);
    }
    private void Start()
    {
        lander.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.Crashed:
            case Lander.LandingType.TooFast:
            case Lander.LandingType.TooSteepAngle:
                Instantiate(landerExplosionVFX, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
        }
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        SetEnableThruster(leftThrusterParticleSystem, false);
        SetEnableThruster(middleThrusterParticleSystem, false);
        SetEnableThruster(rightThrusterParticleSystem, false);
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        SetEnableThruster(leftThrusterParticleSystem, true);
        SetEnableThruster(middleThrusterParticleSystem, false);
        SetEnableThruster(rightThrusterParticleSystem, false);
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        SetEnableThruster(leftThrusterParticleSystem, false);
        SetEnableThruster(middleThrusterParticleSystem, false);
        SetEnableThruster(rightThrusterParticleSystem, true);
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        SetEnableThruster(leftThrusterParticleSystem, true);
        SetEnableThruster(middleThrusterParticleSystem, true);
        SetEnableThruster(rightThrusterParticleSystem, true);
    }

    private void SetEnableThruster(ParticleSystem particleSystem, bool enabled)
   {
       ParticleSystem.EmissionModule emission = particleSystem.emission;
       emission.enabled = enabled;
   }
}
