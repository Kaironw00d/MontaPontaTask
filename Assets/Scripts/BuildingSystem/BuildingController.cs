using System;
using GridSystem;
using UnityEngine;
using Grid = GridSystem.Grid;

namespace BuildingSystem
{
    public class BuildingController : MonoBehaviour
    {
        [Serializable]
        private struct StartingBuilding
        {
            public BuildingType type;
            public Vector3 position;
        }
        
        [SerializeField] private BuildingsDatabase buildingsDatabase;
        [SerializeField] private StartingBuilding[] startingBuildings;
        [SerializeField] private StartBuildingInvoker[] startBuildingInvokers;
        
        private Grid _grid;
        private IBuildingInputProvider _inputProvider;
        private BuildingType _flyingBuildingType;

        private void Awake()
        {
            _grid = GetComponent<Grid>();
            _inputProvider = GetComponent<IBuildingInputProvider>();

            buildingsDatabase.Init();
            
            for (var i = 0; i < startBuildingInvokers.Length; i++)
            {
                startBuildingInvokers[i].StartBuildingEvent += StartPlacing;
            }
            _inputProvider.PositionCalculatedEvent += position => AddToGrid(_flyingBuildingType, position);
            
            PlaceStartingBuildings();
        }

        private void PlaceStartingBuildings()
        {
            for (var i = 0; i < startingBuildings.Length; i++)
            {
                var startingBuilding = startingBuildings[i];
                AddToGrid(startingBuilding.type, startingBuilding.position);
            }
        }

        private void StartPlacing(BuildingType type)
        {
            if(_flyingBuildingType != BuildingType.None) return;
            _flyingBuildingType = type;
            _inputProvider.CalculatePosition();
        }

        private void AddToGrid(BuildingType type, Vector3 position)
        {
            if (!buildingsDatabase.Get(type, out var building)) return;
            var buildingObject = building.Pool.Get();
            var buildingGridElement = buildingObject.GetComponent<IGridElement>();
            buildingGridElement.OnBuildingDisable += RemoveFromGrid;
            
            position.x = Mathf.RoundToInt(position.x);
            position.z = Mathf.RoundToInt(position.z);

            if (!_grid.AddElement(buildingGridElement, position))
            {
                building.Pool.ReturnToPool(buildingObject);
                _flyingBuildingType = BuildingType.None;
                return;
            }
            buildingObject.transform.position = position;
            buildingObject.SetActive(true);
            _flyingBuildingType = BuildingType.None;
        }

        private void RemoveFromGrid(IGridElement gridElement, Vector3 position)
        {
            _grid.RemoveElement(gridElement, position);
        }
    }
}