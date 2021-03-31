using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBasedCollectiblesProvider : MonoBehaviour, ICollectiblesProvider
{
    public event Action OnListChanged;
    private readonly List<ICollectable> _collectables = new List<ICollectable>();

    private void OnTriggerEnter(Collider other)
    {
        var collectible = other.GetComponent<ICollectable>();
        if (collectible != null)
        {
            _collectables.Add(collectible);
            collectible.OnDestroy += RemoveCollectable;
            OnListChanged?.Invoke();
        }
    }

    private void RemoveCollectable(ICollectable collectable)
    {
        _collectables.Remove(collectable);
        OnListChanged?.Invoke();
    }

    public List<ICollectable> Get() => _collectables;
}