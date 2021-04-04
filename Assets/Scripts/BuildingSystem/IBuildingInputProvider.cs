using System;
using UnityEngine;

namespace BuildingSystem
{
    internal interface IBuildingInputProvider
    {
        event Action<Vector3> PositionCalculatedEvent;
        void CalculatePosition();
    }
}