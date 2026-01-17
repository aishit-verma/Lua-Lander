using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip fuelPickupSound;
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip successfulLandingSound;
    private void Start()
    {
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch(e.landingType)
        {
            case Lander.LandingType.Successful:
                AudioSource.PlayClipAtPoint(successfulLandingSound, Camera.main.transform.position,0.5f);
                break;
            case Lander.LandingType.Crashed:
            case Lander.LandingType.TooFast:
            case Lander.LandingType.TooSteepAngle:
                AudioSource.PlayClipAtPoint(crashSound, Camera.main.transform.position,0.5f);
                break;
        }
    }

    private void Lander_OnCoinPickup(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position,0.5f);
    }

    private void Lander_OnFuelPickup(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickupSound, Camera.main.transform.position,0.5f);
    }
}
