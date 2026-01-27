using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float respawnDelay = 1f;

    private Vector2 direction;
    private float speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, float speed)
    {
        this.direction = direction.normalized;
        this.speed = speed;

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = this.direction * this.speed;
        }

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Invoke(nameof(ReloadScene), respawnDelay);
        }

        // Destroy on hitting terrain/obstacles
        if (collision.CompareTag("Ground") || collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}