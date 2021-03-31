using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameObjectPool
{
    private readonly GameObject _prefab;
    private readonly Queue<GameObject> _pool;
    
    public GameObjectPool(GameObject prefab)
    {
        _prefab = prefab;
        _pool = new Queue<GameObject>();
    }

    public GameObject Get()
    {
        if (_pool.Count == 0)
            AddToPool(1);
        return _pool.Dequeue();
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        objectToReturn.transform.position = Vector3.zero;
        _pool.Enqueue(objectToReturn);
    }

    private void AddToPool(int count)
    {
        var newObject = Object.Instantiate(_prefab);
        newObject.GetComponent<IPoolable>().Pool = this;
        newObject.SetActive(false);
        _pool.Enqueue(newObject);
    }
}