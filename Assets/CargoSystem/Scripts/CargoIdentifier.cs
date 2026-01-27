using UnityEngine;

public class CargoIdentifier : MonoBehaviour
{
    [Header("Cargo Identity")]
    [SerializeField] private string cargoID = "Regular";
    
    public string CargoID => cargoID;
}