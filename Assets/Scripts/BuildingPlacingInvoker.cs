using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlacingInvoker : MonoBehaviour
{
    public event Action<BuildingType> StartBuildingPlacingEvent;
    
    [SerializeField] private BuildingType buildingType;
    [SerializeField] private Button button;

    private void Awake() => button.onClick.AddListener(() => StartBuildingPlacingEvent?.Invoke(buildingType));
}