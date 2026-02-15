using UnityEngine;
using Unity.Cinemachine;

public class CoinPickup : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
        CameraShakeManager.instance.CameraShake(impulseSource);
    }
}
