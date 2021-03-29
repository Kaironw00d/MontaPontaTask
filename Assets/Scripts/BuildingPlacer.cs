using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [Serializable]
    private struct StartingBuilding
    {
        public Vector3 Position;
        public BuildingType Type;
    }
    [SerializeField] private BuildingsDatabase buildingsDatabase;
    [SerializeField] private List<BuildingGridElement> buildings;
    [SerializeField] private StartingBuilding[] startingBuildings;
    [SerializeField] private BuildingPlacingInvoker[] placingInvokers;
    [SerializeField] private BuildingGrid[] grids;
    public LayerMask layer;

    private BuildingGridElement _placingBuilding;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        SubscribeToInvokers();
        BuildingsPoolInit();
        PlaceStartingBuildings();
    }

    private void PlaceStartingBuildings()
    {
        for (var i = 0; i < startingBuildings.Length; i++)
        {
            if(_placingBuilding != null) return;
            if (buildingsDatabase.Get(startingBuildings[i].Type, out var building))
            {
                _placingBuilding = building.pool.Get().GetComponent<BuildingGridElement>();
                _placingBuilding.OnBuildingDisable += RemoveFromGrid;
                if (SnapToGrid(_placingBuilding, startingBuildings[i].Position))
                {
                    _placingBuilding.gameObject.SetActive(true);
                    _placingBuilding = null;
                }
            }
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
            StartCoroutine(PlaceBuilding());
        }
    }

    private IEnumerator PlaceBuilding()
    {
        while (true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, layer))
                {
                    if (SnapToGrid(_placingBuilding, hitInfo.point))
                    {
                        _placingBuilding.gameObject.SetActive(true);
                        _placingBuilding = null;
                        yield break;
                    }
                }
            }
            yield return null;
        }
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