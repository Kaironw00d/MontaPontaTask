using System;
using UnityEngine;

public class Tree : MonoBehaviour, ICollectable, IPoolable
{
    public event Action<ICollectable> OnDestroy;
    public GameObjectPool Pool { get; set; }
    [SerializeField] private Transform targetTransform;
    public Transform Transform => targetTransform;
    [SerializeField] private int maxHealth = 100;
    public int Health { get; private set; }
    public bool IsAlive => Health > 0;

    private void OnEnable() => Health = maxHealth;

    public void Impact(int impactAmount)
    {
        Health -= impactAmount;
        if (!IsAlive)
        {
            Pool.ReturnToPool(gameObject);
        }
    }

    private void OnDisable() => OnDestroy?.Invoke(this);
}