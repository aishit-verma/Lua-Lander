using UnityEngine;
using UnityEngine.Events;

public class ZoneIdentifier : MonoBehaviour
{
    [Header("Zone Settings")]
    [SerializeField] private string acceptedCargoID = "Regular";
    [SerializeField] private GameObject cargoPrefab;
    
    [Header("Visual Feedback")]
    [SerializeField] private SpriteRenderer zoneIndicator;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color matchColor = Color.green;
    [SerializeField] private Color mismatchColor = Color.red;
    
    [Header("Events")]
    public UnityEvent OnCargoPickedUp;
    public UnityEvent OnCargoDropped;
    public UnityEvent OnWrongCargoType;
    
    public string AcceptedCargoID => acceptedCargoID;
    public GameObject CargoPrefab => cargoPrefab;
    
    private void Start()
    {
        ResetVisualFeedback();
    }
    
    public bool AcceptsCargo(string cargoID)
    {
        return string.Equals(acceptedCargoID, cargoID, System.StringComparison.OrdinalIgnoreCase);
    }
    
    public void ShowMatchFeedback()
    {
        if (zoneIndicator != null)
        {
            zoneIndicator.color = matchColor;
        }
    }
    
    public void ShowMismatchFeedback()
    {
        if (zoneIndicator != null)
        {
            zoneIndicator.color = mismatchColor;
        }
        OnWrongCargoType?.Invoke();
    }
    
    public void ResetVisualFeedback()
    {
        if (zoneIndicator != null)
        {
            zoneIndicator.color = defaultColor;
        }
    }
}