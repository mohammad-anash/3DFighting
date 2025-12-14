using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Bullet
{
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    private Transform parent;

    public ObjectPool(T prefab, int initialCount, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        // Create initial bullets
        for (int i = 0; i < initialCount; i++)
        {
            AddObjectToPool();
        }
    }

    private void AddObjectToPool()
    {
        T obj = GameObject.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    public T Get()
    {
        if (pool.Count <= 0)
        {
            AddObjectToPool(); // expand pool if needed
        }

        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}