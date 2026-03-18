using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardDefense.Core.Pooling
{
    /// <summary>
    /// 매우 빠르고 일관된 글로벌 오브젝트 풀.
    /// EventBus로 통보받은 파티클이나 다가오는 적군의 재사용을 위해 필수적인 기초 단계입니다.
    /// </summary>
    public class SimpleObjectPool<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly Transform _parentTransform;
        private readonly Queue<T> _pool = new Queue<T>();

        public SimpleObjectPool(T prefab, Transform parentTransform, int initialSize = 10)
        {
            _prefab = prefab;
            _parentTransform = parentTransform;

            for (int i = 0; i < initialSize; i++)
            {
                var newObj = CreateNew();
                newObj.gameObject.SetActive(false);
                _pool.Enqueue(newObj);
            }
        }

        private T CreateNew()
        {
            var newObj = UnityEngine.Object.Instantiate(_prefab, _parentTransform);
            return newObj;
        }

        public T Get()
        {
            T obj = _pool.Count > 0 ? _pool.Dequeue() : CreateNew();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Return(T obj)
        {
            if (obj == null) return;
            
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_parentTransform);
            
            if (!_pool.Contains(obj))
            {
                _pool.Enqueue(obj);
            }
        }

        public void ReturnAll(List<T> activeObjects)
        {
            foreach (var active in activeObjects)
            {
                Return(active);
            }
            activeObjects.Clear();
        }
    }
}
