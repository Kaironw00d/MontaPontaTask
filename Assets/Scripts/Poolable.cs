using UnityEngine;

public class Poolable : MonoBehaviour, IPoolable
{
    public GameObjectPool Pool { get; set; }
}