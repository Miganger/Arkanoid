using UnityEngine;
using UnityEngine.Pool;

namespace ObjectsPool
{
    public abstract class PooledObject : MonoBehaviour
    {
        private IObjectPool<PooledObject> pool;

        public virtual void Setup(IObjectPool<PooledObject> pool)
        {
            this.pool = pool;
        }

        public virtual void ReturnToPool()
        {
            if (!gameObject.activeSelf)
                return;

            pool?.Release(this);
        }
    }
}