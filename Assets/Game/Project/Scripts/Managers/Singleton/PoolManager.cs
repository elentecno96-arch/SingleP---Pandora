using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Utillity.Generic;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Core.Projectile.Interface;
using Game.Project.Scripts.Core.Projectile.SO;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 모든 풀을 관리하는 매니저
    /// </summary>
    public class PoolManager : Singleton<PoolManager>
    {
        // 모든 풀을 관리하는 딕셔너리
        private Dictionary<string, object> _pools = new Dictionary<string, object>();

        private Transform _projectileRoot;
        private Transform _effectRoot;
        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;

            _projectileRoot = CreateRoot("Projectile_Pool");
            _effectRoot = CreateRoot("Effect_Pool");
            _isInitialized = true;
        }

        private Transform CreateRoot(string name)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(this.transform);
            return obj.transform;
        }
        private CustomPoolT<T> CreatePool<T>(GameObject prefab, Transform root) where T : Component
        {
            string key = prefab.name;

            if (_pools.ContainsKey(key) == false)
            {
                var newPool = new CustomPoolT<T>(
                    createFunc: () => CreateNewI<T>(prefab, root),
                    onGet: (item) => item.gameObject.SetActive(true),
                    onRelease: (item) => {
                        item.gameObject.SetActive(false);
                        item.transform.SetParent(root);
                    },
                    initialCount: 10
                );

                _pools.Add(key, newPool);
            }

            return (CustomPoolT<T>)_pools[key];
        }
        private T CreateNewI<T>(GameObject prefab, Transform root) where T : Component
        {
            GameObject obj = Instantiate(prefab, root);
            obj.name = prefab.name;
            obj.gameObject.SetActive(false);

            T component = obj.GetComponent<T>();
            if (component == null) component = obj.AddComponent<T>();

            return component;
        }
        public Projectile GetProjectile(GameObject prefab)
        {
            CustomPoolT<Projectile> pool = CreatePool<Projectile>(prefab, _projectileRoot);
            Projectile proj = pool.Get();
            proj.OnReturnToPool = (item) => pool.Release(proj);
            return proj;
        }

        public GameObject GetObject(GameObject prefab, Transform parent = null)
        {
            CustomPoolT<Transform> pool = CreatePool<Transform>(prefab, _effectRoot);
            Transform instance = pool.Get();

            if (parent != null) instance.SetParent(parent);
            return instance.gameObject;
        }

        public void ReturnObject(GameObject instance)
        {
            if (instance == null) return;

            string key = instance.name;
            if (_pools.TryGetValue(key, out object pool))
            {
                var targetPool = (CustomPoolT<Transform>)pool;
                targetPool.Release(instance.transform);
            }
        }
    }
}