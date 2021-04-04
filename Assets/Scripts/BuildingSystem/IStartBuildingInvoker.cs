using System;

namespace BuildingSystem
{
    internal interface IStartBuildingInvoker
    {
        event Action<BuildingType> StartBuildingEvent;
    }
}