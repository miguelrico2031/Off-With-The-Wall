using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> where T : IPoolObject
{
    public int PoolSize { get; private set; }
    public int ActiveCount { get; private set; }

    private readonly IPoolObject _prototype;
    private readonly bool _allowAddNew;
    private readonly Transform _parent;

    private readonly List<IPoolObject> _pool;


    public ObjectPool (T prototype, int poolSize, bool allowAddNew = false, Transform parent = null)
    {
        _prototype = prototype;
        PoolSize = poolSize;
        _allowAddNew = allowAddNew;
        _parent = parent;
        ActiveCount = 0;

        _pool = new(PoolSize);

        for (int i = 0; i < PoolSize; i++) _pool.Add(_prototype.Clone(_parent));
    }

    public T Get()
    {
        var availableObj = _pool.FirstOrDefault(obj => !obj.Active);
        if (availableObj != null)
        {
            availableObj.Active = true;
            ActiveCount++;
            return (T) availableObj;
        }
        
        if (!_allowAddNew) return default;

        var newObj = _prototype.Clone(_parent, true);
        _pool.Add(newObj);
        ActiveCount++;
        PoolSize++;
        return (T) newObj;
    }

    public bool Return(T obj)
    {
        if(!_pool.Contains(obj)) return false;

        obj.Active = false;
        ActiveCount--;
        obj.Clean();
        return true;
    }


}
