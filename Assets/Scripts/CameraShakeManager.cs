using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    [SerializeField] private float globalShakeForce = 1f;

    private void Awake()
    {
        instance = this;
    }
    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }
}
