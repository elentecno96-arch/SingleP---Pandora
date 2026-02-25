using Game.Project.Data.Damage;
using Game.Project.Scripts.Core.Projectile;
using Game.Project.Scripts.Enemy;
using Game.Project.Scripts.Managers.UI.ItemPopUp;
using Game.Project.Utility.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Scripts.Managers.Singleton
{
    /// <summary>
    /// 모든 풀을 관리하는 매니저
    /// </summary>
    public class PoolManager : Singleton<PoolManager>
    {
        // 모든 풀을 관리하는 딕셔너리
        private Dictionary<string, CustomPoolT<Projectile>> _projectilePools = new();
        private Dictionary<string, CustomPoolT<Transform>> _effectPools = new();
        private Dictionary<string, CustomPoolT<Game.Project.Scripts.Enemy.Enemy>> _enemyPools = new();
        private Dictionary<string, CustomPoolT<DamageText>> _damageTextPools = new();
        private Dictionary<string, CustomPoolT<DuopItemsInfoPopup>> _itemPopupPools = new();

        private Transform _projectileRoot;
        private Transform _effectRoot;
        private Transform _enemyRoot;
        private Transform _uiRoot;
        private bool _isInitialized = false;

        public void Init()
        {
            if (_isInitialized) return;
            _projectileRoot = CreateRoot("Projectile_Pool");
            _effectRoot = CreateRoot("Effect_Pool");
            _enemyRoot = CreateRoot("Enemy_Pool");
            _uiRoot = CreateRoot("UI_DamageText_Pool");

            _projectilePools.Clear();
            _effectPools.Clear();
            _enemyPools.Clear();
            _damageTextPools.Clear();
            _itemPopupPools.Clear();

            _isInitialized = true;
        }

        private Transform CreateRoot(string name)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(this.transform);
            return obj.transform;
        }

        public Projectile GetProjectile(GameObject prefab)
        {
            string key = prefab.name;
            if (!_projectilePools.TryGetValue(key, out var pool))
            {
                pool = new CustomPoolT<Projectile>(
                    createFunc: () => CreateNewInstance<Projectile>(prefab, _projectileRoot),
                    onGet: (item) => item.gameObject.SetActive(true),
                    onRelease: (item) => {
                        item.gameObject.SetActive(false);
                        item.transform.SetParent(_projectileRoot);
                    },
                    initialCount: 10
                );
                _projectilePools.Add(key, pool);
            }

            Projectile proj = pool.Get();
            proj.OnReturnToPool = (item) => pool.Release(item);
            return proj;
        }

        public Game.Project.Scripts.Enemy.Enemy GetEnemy(GameObject prefab)
        {
            string key = prefab.name;
            if (!_enemyPools.TryGetValue(key, out var pool))
            {
                pool = new CustomPoolT<Game.Project.Scripts.Enemy.Enemy>(
                    createFunc: () => CreateNewInstance<Game.Project.Scripts.Enemy.Enemy>(prefab, _enemyRoot),
                    onGet: (item) => item.gameObject.SetActive(true),
                    onRelease: (item) => {
                        item.gameObject.SetActive(false);
                        item.transform.SetParent(_enemyRoot);
                    },
                    initialCount: 5
                );
                _enemyPools.Add(key, pool);
            }
            return pool.Get();
        }

        public void ReturnEnemy(Game.Project.Scripts.Enemy.Enemy enemy)
        {
            if (enemy == null) return;
            if (_enemyPools.TryGetValue(enemy.gameObject.name, out var pool))
            {
                pool.Release(enemy);
            }
            else
            {
                Destroy(enemy.gameObject);
            }
        }

        public GameObject GetEffect(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            string key = prefab.name;
            if (!_effectPools.TryGetValue(key, out var pool))
            {
                pool = new CustomPoolT<Transform>(
                    createFunc: () => CreateNewInstance<Transform>(prefab, _effectRoot),
                    onGet: (item) => item.gameObject.SetActive(true),
                    onRelease: (item) => {
                        item.gameObject.SetActive(false);
                        item.transform.SetParent(_effectRoot);
                    },
                    initialCount: 10
                );
                _effectPools.Add(key, pool);
            }

            Transform instance = pool.Get();
            instance.SetPositionAndRotation(position, rotation);
            if (parent != null) instance.SetParent(parent);

            return instance.gameObject;
        }

        public void ReturnEffect(GameObject instance)
        {
            if (instance == null) return;

            string key = instance.name;
            if (_effectPools.TryGetValue(key, out var pool))
            {
                pool.Release(instance.transform);
            }
            else
            {
                Destroy(instance);
            }
        }

        //대미지 텍스트 풀
        public DamageText GetDamageText(GameObject prefab, Vector3 position)
        {
            string key = prefab.name;
            if (!_damageTextPools.TryGetValue(key, out var pool))
            {
                pool = new CustomPoolT<DamageText>(
                    createFunc: () => CreateNewInstance<DamageText>(prefab, _uiRoot),
                    onGet: (item) => item.gameObject.SetActive(true),
                    onRelease: (item) => {
                        item.gameObject.SetActive(false);
                        item.transform.SetParent(_uiRoot);
                    },
                    initialCount: 30 
                );
                _damageTextPools.Add(key, pool);
            }

            DamageText dt = pool.Get();
            dt.transform.position = position;
            dt.transform.rotation = Quaternion.identity;
            return dt;
        }

        public DuopItemsInfoPopup GetItemPopup(GameObject prefab, Transform parent)
        {
            string key = prefab.name;
            if (!_itemPopupPools.TryGetValue(key, out var pool))
            {
                pool = new CustomPoolT<DuopItemsInfoPopup>(
                    createFunc: () => CreateNewInstance<DuopItemsInfoPopup>(prefab, _uiRoot),
                    onGet: (item) => item.gameObject.SetActive(true),
                    onRelease: (item) => {
                        item.gameObject.SetActive(false);
                        item.transform.SetParent(_uiRoot);
                    },
                    initialCount: 5
                );
                _itemPopupPools.Add(key, pool);
            }

            DuopItemsInfoPopup popup = pool.Get();

            popup.transform.SetParent(parent, false);

            popup.transform.localPosition = Vector3.zero;
            popup.transform.localScale = Vector3.one;

            RectTransform rect = popup.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero; 
            }

            popup.OnReturnToPool = (item) => pool.Release(item);

            return popup;
        }

        public void ReturnDamageText(DamageText dt)
        {
            if (dt == null) return;

            string key = dt.gameObject.name;
            if (_damageTextPools.TryGetValue(key, out var pool))
            {
                pool.Release(dt);
            }
            else
            {
                Destroy(dt.gameObject);
            }
        }

        public GameObject GetWarningEffect(GameObject prefab, Vector3 position, Quaternion rotation, float duration)
        {
            GameObject indicator = GetEffect(prefab, position, rotation);

            if (indicator.TryGetComponent<WarningIndicator>(out var warning))
            {
                warning.InitAndRelease(duration);
            }
            else
            {
                StartCoroutine(AutoReturnRoutine(indicator, duration));
            }

            return indicator;
        }

        private IEnumerator AutoReturnRoutine(GameObject obj, float duration)
        {
            yield return new WaitForSeconds(duration);
            ReturnEffect(obj);
        }

        private T CreateNewInstance<T>(GameObject prefab, Transform root) where T : Component
        {
            GameObject obj = Instantiate(prefab, root);
            obj.name = prefab.name;
            obj.SetActive(false);

            T component = obj.GetComponent<T>();
            if (component == null) component = obj.AddComponent<T>();

            return component;
        }
    }
}