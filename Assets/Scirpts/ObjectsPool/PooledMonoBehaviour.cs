using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectsPool
{
    public abstract class PooledMonoBehaviour : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] private int defaultCapacity = 10;
        [SerializeField] private int maxSize = 50;
        [SerializeField] private List<PooledObject> prefabs;

        // Ka¿dy prefab ma swoj¹ w³asn¹ pulê.
        private readonly Dictionary<int, ObjectPool<PooledObject>> pools = new(); 
        private readonly Dictionary<PooledObject, ObjectPool<PooledObject>> activeObjects = new();

        public PooledObject Get(int prefabIndex = 0)
        {
            return GetPool(prefabIndex).Get();
        }

        public int GetActiveCount(int prefabIndex = 0)
        {
            return GetPool(prefabIndex).CountActive;
        }

        protected void ReleaseAllActive()
        {
            // Kopia listy aktualnie pobranych obiektów.
            var objectsToRelease =
                new List<PooledObject>(activeObjects.Keys);

            foreach (PooledObject obj in objectsToRelease)
            {
                if (!activeObjects.TryGetValue(obj, out var pool))
                    continue;

                // Obs³uga obiektów zniszczonych poza pul¹.
                if (obj == null)
                {
                    activeObjects.Remove(obj);
                    continue;
                }

                obj.ReturnToPool();
            }
        }

        private ObjectPool<PooledObject> GetPool(int prefabIndex)
        {
            if (prefabIndex < 0 || prefabIndex >= prefabs.Count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(prefabIndex),
                    $"Nie ma prefabu o indeksie {prefabIndex}."
                );
            }

            if (prefabs[prefabIndex] == null)
            {
                throw new InvalidOperationException(
                    $"Prefab o indeksie {prefabIndex} nie zosta³ przypisany."
                );
            }

            // Jeœli pula dla tego prefabu ju¿ istnieje,
            // zwracamy j¹.
            if (pools.TryGetValue(prefabIndex, out var existingPool))
                return existingPool;

            // Tworzymy pulê dopiero przy pierwszym u¿yciu prefabu.
            PooledObject prefab = prefabs[prefabIndex];

            ObjectPool<PooledObject> newPool = null;

            newPool = new ObjectPool<PooledObject>(
                createFunc: () =>
                {
                    PooledObject instance = Instantiate(prefab, transform);

                    // Obiekt musi wiedzieæ, do której puli ma wróciæ.
                    instance.Setup(newPool);

                    return instance;
                },
                actionOnGet: obj =>
                {
                    activeObjects[obj] = newPool;
                    OnGetPooledObject(obj);
                },
                actionOnRelease: obj =>
                {
                    activeObjects.Remove(obj);
                    OnReleasePooledObject(obj);
                },

                actionOnDestroy: obj =>
                {
                    activeObjects.Remove(obj);
                    OnDestroyPooledObject(obj);
                },
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );

            pools.Add(prefabIndex, newPool);

            return newPool;
        }

        protected virtual void OnGetPooledObject(PooledObject pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
        }

        protected virtual void OnReleasePooledObject(PooledObject pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        protected virtual void OnDestroyPooledObject(PooledObject pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
    }
}