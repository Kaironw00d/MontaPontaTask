using System;
using UnityEngine;

namespace BuildingSystem
{
    public class StartBuildingInvoker : MonoBehaviour, IStartBuildingInvoker
    {
        public event Action<BuildingType> StartBuildingEvent;

        public BuildingType buildingType;

        public void StartBuilding() => StartBuildingEvent?.Invoke(buildingType);
    }
}