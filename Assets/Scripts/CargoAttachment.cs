using System.Collections;
using UnityEngine;

public class CargoAttachment : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform attachPoint;
    [SerializeField] private string anchorTag = "RopeAnchor";

    [Header("Settings")]
    [SerializeField] private float waitTime = 2f;

    // Public state
    public bool HasCargo => hasCargo;
    public string CurrentCargoID => currentCargoID;
    
    // Private state
    private bool hasCargo = false;
    private string currentCargoID = "";
    private bool isInPickupZone = false;
    private bool isInDropZone = false;

    // References
    private Rigidbody2D landerRb;
    private GameObject currentCargo;
    private HingeJoint2D anchorJoint;
    private Coroutine currentCoroutine;
    private ZoneTimerUI currentZoneTimer;

    private void Awake()
    {
        landerRb = GetComponent<Rigidbody2D>();
    }

    public void AttachCargo(GameObject cargoPrefab, string cargoID)
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
                currentCargoID = cargoID;
                
                Debug.Log($"Picked up: {cargoID}");
            }
        }
    }

    public void DetachCargo(bool destroyCargo = false)
    {
        if (!hasCargo || currentCargo == null) return;

        if (anchorJoint != null)
        {
            anchorJoint.enabled = false;
            anchorJoint.connectedBody = null;
        }

        if (destroyCargo && currentCargo != null)
        {
            Destroy(currentCargo);
        }

        Debug.Log($"Dropped: {currentCargoID}");

        currentCargo = null;
        anchorJoint = null;
        hasCargo = false;
        currentCargoID = "";
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
        ZoneIdentifier zone = collision.GetComponent<ZoneIdentifier>();
        if (zone == null) return;

        // Pickup Zone
        if (collision.CompareTag("Pick") && !hasCargo)
        {
            isInPickupZone = true;
            
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            
            zone.ShowMatchFeedback();
            
            currentZoneTimer = collision.GetComponentInChildren<ZoneTimerUI>();
            if (currentZoneTimer != null)
            {
                currentZoneTimer.StartFill(waitTime);
            }
            
            currentCoroutine = StartCoroutine(PickupCoroutine(collision.gameObject, zone));
        }

        // Drop Zone
        if (collision.CompareTag("Drop") && hasCargo)
        {
            // Check if zone accepts our cargo
            if (!zone.AcceptsCargo(currentCargoID))
            {
                zone.ShowMismatchFeedback();
                Debug.Log($"Wrong cargo! Zone wants: {zone.AcceptedCargoID}, You have: {currentCargoID}");
                return;
            }
            
            isInDropZone = true;
            
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            
            zone.ShowMatchFeedback();
            
            currentZoneTimer = collision.GetComponentInChildren<ZoneTimerUI>();
            if (currentZoneTimer != null)
            {
                currentZoneTimer.StartFill(waitTime);
            }
            
            currentCoroutine = StartCoroutine(DropCoroutine(collision.gameObject, zone));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ZoneIdentifier zone = collision.GetComponent<ZoneIdentifier>();

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
            
            if (zone != null)
            {
                zone.ResetVisualFeedback();
            }
        }

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
            
            if (zone != null)
            {
                zone.ResetVisualFeedback();
            }
        }
    }

    private IEnumerator PickupCoroutine(GameObject zoneObject, ZoneIdentifier zone)
    {
        yield return new WaitForSeconds(waitTime);
        
        if (isInPickupZone && !hasCargo)
        {
            AttachCargo(zone.CargoPrefab, zone.AcceptedCargoID);
            zone.OnCargoPickedUp?.Invoke();
            Destroy(zoneObject);
        }
        
        currentZoneTimer = null;
    }

    private IEnumerator DropCoroutine(GameObject zoneObject, ZoneIdentifier zone)
    {
        yield return new WaitForSeconds(waitTime);
        
        if (isInDropZone && hasCargo)
        {
            DetachCargo(true);
            zone.OnCargoDropped?.Invoke();
            Destroy(zoneObject);
        }
        
        currentZoneTimer = null;
    }
}