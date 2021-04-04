using System;
using UnityEngine;

namespace GridSystem
{
    public interface IGridElement
    {
        event Action<IGridElement, Vector3> OnBuildingDisable;
        Vector2Int Size { get; }
    }
}