using UnityEngine;
using UnityEngine.UI;

public class ZoneTimerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image fillImage;
    
    [Header("Settings")]
    [SerializeField] private bool fillUp = true;
    [SerializeField] private bool hideWhenInactive = true;
    
    private float fillDuration;
    private float currentTime;
    private bool isActive = false;
    
    private void Awake()
    {
        if (fillImage == null)
        {
            fillImage = GetComponent<Image>();
        }
        
        ResetFill();
        
        if (hideWhenInactive)
        {
            SetVisibility(false);
        }
    }
    
    private void Update()
    {
        if (!isActive) return;
        
        currentTime += Time.deltaTime;
        float progress = Mathf.Clamp01(currentTime / fillDuration);
        
        if (fillUp)
        {
            fillImage.fillAmount = progress;
        }
        else
        {
            fillImage.fillAmount = 1f - progress;
        }
    }
    
    public void StartFill(float duration)
    {
        fillDuration = duration;
        currentTime = 0f;
        isActive = true;
        
        fillImage.fillAmount = fillUp ? 0f : 1f;
        
        if (hideWhenInactive)
        {
            SetVisibility(true);
        }
    }
    
    public void StopFill()
    {
        isActive = false;
        ResetFill();
        
        if (hideWhenInactive)
        {
            SetVisibility(false);
        }
    }
    
    private void ResetFill()
    {
        currentTime = 0f;
        if (fillImage != null)
        {
            fillImage.fillAmount = fillUp ? 0f : 1f;
        }
    }
    
    private void SetVisibility(bool visible)
    {
        if (fillImage != null)
        {
            fillImage.enabled = visible;
        }
    }
    
    public bool IsActive => isActive;
}