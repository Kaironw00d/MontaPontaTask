using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [Serializable]
    private struct StartingBuilding
    {
        public Vector3 position;
        public BuildingType type;
    }
    [SerializeField] private BuildingsDatabase buildingsDatabase;
    [SerializeField] private List<BuildingGridElement> buildings;
    [SerializeField] private StartingBuilding[] startingBuildings;
    [SerializeField] private BuildingPlacingInvoker[] placingInvokers;
    [SerializeField] private BuildingGrid[] grids;
    public LayerMask layer;
    
    private Camera _camera;
    private BuildingGridElement _placingBuilding;
    private IBuildingGridInputProvider _inputProvider;

    private void Start()
    {
        _camera = Camera.main;
        _inputProvider = GetComponent<IBuildingGridInputProvider>();
        _inputProvider.OnPerformedPointerAction += CalculatePositionFromPointer;
        SubscribeToInvokers();
        BuildingsPoolInit();
        PlaceStartingBuildings();
    }

    private void PlaceStartingBuildings()
    {
        for (var i = 0; i < startingBuildings.Length; i++)
        {
            StartBuildingPlacing(startingBuildings[i].type, startingBuildings[i].position);
        }
    }

    private void BuildingsPoolInit()
    {
        for (var i = 0; i < buildingsDatabase.buildings.Length; i++)
        {
            var building = buildingsDatabase.buildings[i];
            building.pool = new GameObjectPool(building.building.gameObject);
        }
    }

    private void SubscribeToInvokers()
    {
        for (var i = 0; i < placingInvokers.Length; i++)
        {
            placingInvokers[i].StartBuildingPlacingEvent += StartBuildingPlacing;
        }
    }

    private void StartBuildingPlacing(BuildingType buildingType)
    {
        if(_placingBuilding != null) return;
        if (buildingsDatabase.Get(buildingType, out var building))
        {
            _placingBuilding = building.pool.Get().GetComponent<BuildingGridElement>();
            _placingBuilding.OnBuildingDisable += RemoveFromGrid;
            StartCoroutine(_inputProvider.TrackPerformedPointerPosition());
        }
    }

    private void StartBuildingPlacing(BuildingType buildingType, Vector3 position)
    {
        if(_placingBuilding != null) return;
        if (buildingsDatabase.Get(buildingType, out var building))
        {
            _placingBuilding = building.pool.Get().GetComponent<BuildingGridElement>();
            _placingBuilding.OnBuildingDisable += RemoveFromGrid;
            if (PlaceBuilding(_placingBuilding, position))
                _placingBuilding = null;
        }
    }

    private bool PlaceBuilding(BuildingGridElement buildingGridElement, Vector3 position)
    {
        if (!SnapToGrid(buildingGridElement, position)) return false;
        buildingGridElement.gameObject.SetActive(true);
        return true;
    }

    private void CalculatePositionFromPointer(Vector3 pointerPosition)
    {
        var ray = _camera.ScreenPointToRay(pointerPosition);
        if (!Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, layer)) return;
        if (PlaceBuilding(_placingBuilding, hitInfo.point))
            _placingBuilding = null;
    }

    private bool SnapToGrid(BuildingGridElement building, Vector3 position)
    {
        position.x = Mathf.RoundToInt(position.x);
        position.z = Mathf.RoundToInt(position.z);
        for (var i = 0; i < grids.Length; i++)
        {
            if (grids[i].PlaceBuilding(building, position))
            {
                building.transform.position = position;
                buildings.Add(building);
                return true;
            }
        }
        return false;
    }
    
    private void RemoveFromGrid(BuildingGridElement building)
    {
        var position = building.transform.position;
        position.x = Mathf.RoundToInt(position.x);
        position.z = Mathf.RoundToInt(position.z);
        for (var i = 0; i < grids.Length; i++)
        {
            if (grids[i].RemoveBuilding(building, position))
                buildings.Remove(building);
        }
    }
}