using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple object pooling system for frequently spawned objects
/// </summary>
/// <typeparam name="T">Component type to pool</typeparam>
public class SimpleObjectPool<T> where T : Component
{
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly Queue<T> availableObjects = new Queue<T>();
    private readonly HashSet<T> activeObjects = new HashSet<T>();
    private readonly int initialSize;
    
    public SimpleObjectPool(GameObject prefab, Transform parent = null, int initialSize = 10)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.initialSize = initialSize;
        
        // Pre-instantiate initial pool objects
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }
    
    /// <summary>
    /// Get an object from the pool
    /// </summary>
    public T Get()
    {
        T obj;
        
        // Reuse from pool if available
        if (availableObjects.Count > 0)
        {
            obj = availableObjects.Dequeue();
        }
        else
        {
            // Create new if pool is empty
            obj = CreateNewObject();
        }
        
        obj.gameObject.SetActive(true);
        activeObjects.Add(obj);
        return obj;
    }
    
    /// <summary>
    /// Return an object to the pool
    /// </summary>
    public void Return(T obj)
    {
        if (obj == null) return;
        
        obj.gameObject.SetActive(false);
        activeObjects.Remove(obj);
        availableObjects.Enqueue(obj);
        
        // Reset position and parent
        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
    }
    
    /// <summary>
    /// Return all active objects to the pool
    /// </summary>
    public void ReturnAll()
    {
        var objsToReturn = new List<T>(activeObjects);
        foreach (var obj in objsToReturn)
        {
            Return(obj);
        }
    }
    
    private T CreateNewObject()
    {
        GameObject newObj = Object.Instantiate(prefab, parent);
        newObj.SetActive(false);
        T component = newObj.GetComponent<T>();
        availableObjects.Enqueue(component);
        return component;
    }
    
    public int ActiveCount => activeObjects.Count;
    public int AvailableCount => availableObjects.Count;
    public int TotalCount => ActiveCount + AvailableCount;
}
