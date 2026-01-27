using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform gun;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Aiming")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float aimThreshold = 5f; // degrees

    [Header("Firing")]
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float projectileSpeed = 10f;

    private Transform player;
    private bool playerInRange = false;
    private bool canFire = true;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        CheckPlayerInRange();

        if (playerInRange && HasLineOfSight())
        {
            AimAtPlayer();
            TryFire();
        }
    }

    private void CheckPlayerInRange()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        playerInRange = distance <= detectionRadius;
    }

    private bool HasLineOfSight()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;
        float distance = Vector2.Distance(firePoint.position, player.position);

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, distance, obstacleLayer);

        // If we hit nothing on obstacle layer, we have clear line of sight
        return hit.collider == null;
    }

    private void AimAtPlayer()
    {
        Vector2 direction = (player.position - gun.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust if your gun sprite faces a different direction
        // For example, if gun points up by default: targetAngle -= 90f;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        gun.rotation = Quaternion.Lerp(gun.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool IsAimedAtPlayer()
    {
        Vector2 direction = (player.position - gun.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float currentAngle = gun.eulerAngles.z;

        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));
        return angleDifference <= aimThreshold;
    }

    private void TryFire()
    {
        if (canFire && IsAimedAtPlayer())
        {
            Fire();
            StartCoroutine(FireCooldown());
        }
    }

    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projScript = projectile.GetComponent<Projectile>();

        if (projScript != null)
        {
            projScript.Initialize(firePoint.right, projectileSpeed);
        }
    }

    private IEnumerator FireCooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(1f / fireRate);
        canFire = true;
    }

    // Visualize detection radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (firePoint != null && player != null)
        {
            Gizmos.color = HasLineOfSight() ? Color.green : Color.red;
            Gizmos.DrawLine(firePoint.position, player.position);
        }
    }
}