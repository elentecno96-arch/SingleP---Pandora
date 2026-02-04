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
        private Dictionary<string, object> _pools = new();

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
            var root = new GameObject(name).transform;
            root.SetParent(transform);
            return root;
        }
        private CustomPoolT<T> CreatePool<T>(GameObject prefab, Transform root) where T : Component
        {
            string key = prefab.name;
            if (!_pools.ContainsKey(key))
            {
                _pools[key] = new CustomPoolT<T>(
                    createFunc: () =>
                    {
                        var obj = Instantiate(prefab, root);
                        obj.name = key;
                        obj.gameObject.SetActive(false);
                        return obj.GetComponent<T>() ?? obj.AddComponent<T>();
                    },
                    onGet: (item) => item.gameObject.SetActive(true),
                    onRelease: (item) =>
                    {
                        item.gameObject.SetActive(false);
                        item.transform.SetParent(root);
                    },
                    initialCount: 10
                );
            }
            return (CustomPoolT<T>)_pools[key];
        }
        public Projectile GetProjectile(GameObject prefab)
        {
            var pool = CreatePool<Projectile>(prefab, _projectileRoot);
            var proj = pool.Get();
            proj.OnReturnToPool = (target) => pool.Release(proj);
            return proj;
        }
        public GameObject GetGameObject(GameObject prefab, Transform parent = null)
        {
            var pool = CreatePool<Transform>(prefab, _effectRoot);
            var instance = pool.Get().gameObject;

            if (parent != null) instance.transform.SetParent(parent);
            return instance;
        }

        public void ReleaseGameObject(GameObject instance)
        {
            string key = instance.name;
            if (_pools.TryGetValue(key, out var pool))
            {
                ((CustomPoolT<Transform>)pool).Release(instance.transform);
            }
        }
        public void LaunchProjectile(GameObject prefab, Vector3 pos, Vector3 dir, GameObject owner, SkillData data, List<IProjectileStrategy> strategies)
        {
            Projectile proj = GetProjectile(prefab);
            proj.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(dir));
            proj.Init(owner, dir, data, strategies);
        }
    }
}