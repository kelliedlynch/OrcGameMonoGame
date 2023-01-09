using System.Collections.Generic;

namespace OrcGame.Utility;

public class ObjectPool<T> where T : IPoolable, new()
{
    private readonly Queue<T> _pool = new();
    private readonly int _maxSize;
    private int _count;

    public ObjectPool(int maxSize)
    {
        _maxSize = maxSize;
        _count = maxSize;
        for (var i = 0; i < maxSize; i++)
        {
            _pool.Enqueue(new T());
        }
    }

    public T Request()
    {
        if (_count > 0)
        {
            _count--;
            return _pool.Dequeue();
        }

        return new T();
    }

    public void Dispose(T item)
    {
        if (_count < _maxSize)
        {
            item.Reset();
            _pool.Enqueue(item);
            _count++;
        }

    }
    
}

public interface IPoolable
{
    public void Reset();
}