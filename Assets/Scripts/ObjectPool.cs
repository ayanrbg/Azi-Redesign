using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Stack<T> pool = new();

    public ObjectPool(T prefab, Transform parent, int preload = 0)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < preload; i++)
            Create();
    }

    private T Create()
    {
        T obj = Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        pool.Push(obj);
        return obj;
    }

    public T Get()
    {
        if (pool.Count == 0)
            Create();

        T obj = pool.Pop();
        obj.gameObject.SetActive(true);
        obj.transform.SetAsLastSibling();
        return obj;
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Push(obj);
    }
}
