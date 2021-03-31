using System;
using UnityEngine;

public interface ICollectable
{
    event Action<ICollectable> OnDestroy; 
    Transform Transform { get; }
    int Health { get; }
    bool IsAlive { get; }
    void Impact(int impactAmount);
}