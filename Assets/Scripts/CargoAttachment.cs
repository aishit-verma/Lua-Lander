using System.Collections;
using UnityEngine;

public class CargoAttachment : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cargoPrefab;
    [SerializeField] private Transform attachPoint;
    [SerializeField] private string anchorTag = "RopeAnchor";

    [Header("Settings")]
    [SerializeField] private bool destroyOnDetach = false;
    [SerializeField] private float waitTime = 2f;

    [Header("State")]
    public bool hasCargo = false;
    private bool isInPickupZone = false;
    private bool isInDropZone = false;

    private Rigidbody2D landerRb;
    private GameObject currentCargo;
    private HingeJoint2D anchorJoint;
    private Coroutine currentCoroutine;
    private ZoneTimerUI currentZoneTimer;

    private void Awake()
    {
        landerRb = GetComponent<Rigidbody2D>();
    }

    public void AttachCargo()
    {
        if (hasCargo || cargoPrefab == null) return;

        currentCargo = Instantiate(cargoPrefab, Vector3.zero, Quaternion.identity);
        Transform anchor = FindAnchorInCargo(currentCargo);

        if (anchor != null)
        {
            Vector3 offset = attachPoint.position - anchor.position;
            currentCargo.transform.position += offset;

            anchorJoint = anchor.GetComponent<HingeJoint2D>();

            if (anchorJoint != null)
            {
                anchorJoint.connectedBody = landerRb;
                anchorJoint.autoConfigureConnectedAnchor = false;
                anchorJoint.anchor = Vector2.zero;
                anchorJoint.connectedAnchor = transform.InverseTransformPoint(attachPoint.position);
                hasCargo = true;
            }
        }
    }

    public void DetachCargo()
    {
        if (!hasCargo || currentCargo == null) return;

        if (anchorJoint != null)
        {
            anchorJoint.enabled = false;
            anchorJoint.connectedBody = null;
        }

        
            Destroy(currentCargo);
            GameManager.Instance.AddScore(100);
      
        currentCargo = null;
        anchorJoint = null;
        hasCargo = false;
    }

    private Transform FindAnchorInCargo(GameObject cargo)
    {
        Transform[] allChildren = cargo.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.CompareTag(anchorTag))
            {
                return child;
            }
        }
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle Pickup Zone
        if (collision.CompareTag("Pick") && !hasCargo)
        {
            isInPickupZone = true;
            
            if (currentCoroutine != null) 
            {
                StopCoroutine(currentCoroutine);
            }
            
            // Find and start timer UI
            currentZoneTimer = collision.GetComponentInChildren<ZoneTimerUI>();
            if (currentZoneTimer != null)
            {
                currentZoneTimer.StartFill(waitTime);
            }
            
            currentCoroutine = StartCoroutine(PickupCoroutine(collision.gameObject));
        }

        // Handle Drop Zone
        if (collision.CompareTag("Drop") && hasCargo)
        {
            isInDropZone = true;
            
            if (currentCoroutine != null) 
            {
                StopCoroutine(currentCoroutine);
            }
            
            currentZoneTimer = collision.GetComponentInChildren<ZoneTimerUI>();
            if (currentZoneTimer != null)
            {
                currentZoneTimer.StartFill(waitTime);
            }
            
            currentCoroutine = StartCoroutine(DropCoroutine(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Handle leaving Pickup Zone
        if (collision.CompareTag("Pick"))
        {
            isInPickupZone = false;
            
            if (currentCoroutine != null) 
            {
                StopCoroutine(currentCoroutine);
            }
            
            if (currentZoneTimer != null)
            {
                currentZoneTimer.StopFill();
                currentZoneTimer = null;
            }
        }

        // Handle leaving Drop Zone
        if (collision.CompareTag("Drop"))
        {
            isInDropZone = false;
            
            if (currentCoroutine != null) 
            {
                StopCoroutine(currentCoroutine);
            }
            
            if (currentZoneTimer != null)
            {
                currentZoneTimer.StopFill();
                currentZoneTimer = null;
            }
        }
    }

    private IEnumerator PickupCoroutine(GameObject zone)
    {
        yield return new WaitForSeconds(waitTime);
        
        if (isInPickupZone && !hasCargo)
        {
            AttachCargo();
            Destroy(zone);
        }
        
        currentZoneTimer = null;
    }

    private IEnumerator DropCoroutine(GameObject zone)
    {
        yield return new WaitForSeconds(waitTime);
        
        if (isInDropZone && hasCargo)
        {
            DetachCargo();
            Destroy(zone);
        }
        
        currentZoneTimer = null;
    }
}