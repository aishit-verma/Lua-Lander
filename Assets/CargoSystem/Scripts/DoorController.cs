using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D doorCollider;
    
    [Header("Animation")]
    [SerializeField] private string openTriggerName = "Open";
    
    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    
    private bool isOpen = false;
    
    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (doorCollider == null)
        {
            doorCollider = GetComponent<Collider2D>();
        }
    }
    
    public void OpenDoor()
    {
        if (isOpen) return;
        isOpen = true;
        
        // Play animation
        if (animator != null)
        {
            animator.SetTrigger(openTriggerName);
        }
        
        // Disable collider so player can pass through
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
        
        // Play sound
        if (openSound != null)
        {
            AudioSource.PlayClipAtPoint(openSound, transform.position);
        }
        
        Debug.Log("Door Opened!");
    }
    
    public bool IsOpen => isOpen;
}