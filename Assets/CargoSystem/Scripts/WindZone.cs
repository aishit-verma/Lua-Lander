using UnityEngine;

public class WindZone : MonoBehaviour
{
    [Header("Wind Settings")]
    [SerializeField] private WindDirection windDirection = WindDirection.Right;
    [SerializeField] private float windStrength = 3f;
    
    [Header("References")]
    [SerializeField] private string landerTag = "Player";
    
    [Header("Visual")]
    [SerializeField] private SpriteRenderer arrowIndicator;
    
    public enum WindDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    
    private Vector2 forceDirection;
    
    private void Start()
    {
        SetupWindDirection();
    }
    
    private void SetupWindDirection()
    {
        switch (windDirection)
        {
            case WindDirection.Up:
                forceDirection = Vector2.up;
                RotateIndicator(0f);
                break;
            case WindDirection.Down:
                forceDirection = Vector2.down;
                RotateIndicator(180f);
                break;
            case WindDirection.Left:
                forceDirection = Vector2.left;
                RotateIndicator(90f);
                break;
            case WindDirection.Right:
                forceDirection = Vector2.right;
                RotateIndicator(-90f);
                break;
        }
    }
    
    private void RotateIndicator(float angle)
    {
        if (arrowIndicator != null)
        {
            arrowIndicator.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag(landerTag)) return;
        
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(forceDirection * windStrength);
        }
    }
    
    private void OnValidate()
    {
        // Update direction in editor when changed
        SetupWindDirection();
    }
    
    private void OnDrawGizmos()
    {
        // Show wind direction in Scene view
        Gizmos.color = Color.cyan;
        
        Vector3 center = transform.position;
        Vector3 direction = Vector3.zero;
        
        switch (windDirection)
        {
            case WindDirection.Up:
                direction = Vector3.up;
                break;
            case WindDirection.Down:
                direction = Vector3.down;
                break;
            case WindDirection.Left:
                direction = Vector3.left;
                break;
            case WindDirection.Right:
                direction = Vector3.right;
                break;
        }
        
        Gizmos.DrawLine(center, center + direction * 2f);
        Gizmos.DrawWireSphere(center + direction * 2f, 0.2f);
    }
}