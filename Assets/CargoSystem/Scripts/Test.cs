using UnityEngine;

public class TestDoor : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<DoorController>().OpenDoor();
        }
    }
}