using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private Transform landerStartPosition;
    [SerializeField] private Transform cameraStartTarget;
    [SerializeField] private float zoomedOutOrthographicSize;
    public int GetLevelNumber()
    {
        return levelNumber;
    }
    public Vector3 GetLanderStartPosition()
    {
        return landerStartPosition.position;
    }
    public Transform GetCameraStartTarget()
    {
        return cameraStartTarget;
    }
    public float GetZoomedOutOrthographicSize()
    {
        return zoomedOutOrthographicSize;
    }
}
