using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Critsoft.CozyShip.Gameplay.Controllers;
using Zenject;

namespace Critsoft.CozyShip.Gameplay.Spawners
{
    public class ObjectsSpawner : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Init Spawn Points")]
        [Tooltip("If this option is enabled, a number of objects equal to _poolSize will be created during initialization.")]
        [SerializeField] protected bool _useInitSpawn;
        [SerializeField] protected Transform _initPointA;
        [SerializeField] protected Transform _initPointB;

        [Header("Spawn Points")]
        [SerializeField] protected Transform _pointA;
        [SerializeField] protected Transform _pointB;

        [SerializeField] protected GameObject[] _spawnPrefabs;

        [Header("Spawn Timing")]
        [SerializeField] protected float _minSpawnTime = 2f;
        [SerializeField] protected float _maxSpawnTime = 10f;

        [Header("Object Pooling")]
        [SerializeField] protected int _poolSize = 10;
        [SerializeField] protected int _maxPoolSize = 50;
        [SerializeField] protected float _deactivationZ = -50f;

        #endregion

        #region Fields

        protected LevelController _levelController;
        private List<GameObject> _objectPool = new List<GameObject>();
        private WaitForSeconds _disableObjectsWaitForSeconds = new WaitForSeconds(5);

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(LevelController levelController)
        {
            _levelController = levelController;
            _levelController.LevelInitiated += OnLevelInitiated;
        }
        
        public void ChangeSpawnTimes(float minSpawnTime, float maxSpawnTime)
        {
            _minSpawnTime = minSpawnTime;
            _maxSpawnTime = maxSpawnTime;
        }

        public void DisableAllObjects()
        {
            for (int i = _objectPool.Count - 1; i >= 0; i--)
            {
                GameObject obj = _objectPool[i];
                obj.SetActive(false);
            }
        }

        #endregion

        #region Protected Methods

        protected virtual GameObject InstantiateObject(GameObject prefabToSpawn)
        {
            return Instantiate(prefabToSpawn);
        }

        protected virtual void OnLevelInitiated(LevelInitiatedEventArgs eventArgs)
        {
            DisableAllObjects();
            
            if (_useInitSpawn && eventArgs.LevelId > GameConfig.FirstLevelId) // Skip this on the first level.
            {
                for (int i = 0; i < _poolSize; i++)
                {
                    SpawnObject(true);
                }
            }
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            InitializePool();
            StartCoroutine(StartSpawnObjects());
            StartCoroutine(DisableOffscreenObjects());
        }
        
        private void OnDestroy()
        {
            if (_levelController != null)
            {
                _levelController.LevelInitiated -= OnLevelInitiated;
            }
        }

        private void InitializePool()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                CreateNewPooledObject();
                
                if (_useInitSpawn)
                {
                    SpawnObject(true);
                }
            }
        }
        
        private GameObject GetPooledObject()
        {
            foreach (var obj in _objectPool)
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }

            if (_objectPool.Count < _maxPoolSize)
            {
                return CreateNewPooledObject();
            }

            return null;
        }

        private IEnumerator StartSpawnObjects()
        {
            while (true)
            {
                float waitTime = Random.Range(_minSpawnTime, _maxSpawnTime);
                yield return new WaitForSeconds(waitTime);

                SpawnObject(false);
            }
        }

        private void SpawnObject(bool isInitialization)
        {
            GameObject obj = GetPooledObject();
            if (obj == null)
                return;

            Vector3 spawnPosition = isInitialization ? GetRandomPositionBetweenPoints(_initPointA, _initPointB) : GetRandomPositionBetweenPoints(_pointA, _pointB);
            obj.transform.position = spawnPosition;
            obj.SetActive(true);

            SetParentToObject();

            // Local method
            void SetParentToObject()
            {
                Vector3 raycastOriginOffset = new Vector3(0, 10, 0);
                Vector3 raycastOrigin = spawnPosition + raycastOriginOffset;
                if (Physics.Raycast(raycastOrigin, Vector3.down, out RaycastHit hit)) // Use raycast to find scrolling ground.
                {
                    obj.transform.SetParent(hit.collider.transform);
                }
                else
                {
                    obj.transform.SetParent(transform); // Default parent.
                }
            }
        }

        private GameObject CreateNewPooledObject()
        {
            if (_objectPool.Count >= _maxPoolSize)
                return null;

            GameObject prefabToSpawn = _spawnPrefabs[Random.Range(0, _spawnPrefabs.Length)];
            GameObject newObj = InstantiateObject(prefabToSpawn);

            newObj.SetActive(false);
            newObj.transform.SetParent(transform);
            _objectPool.Add(newObj);

            return newObj;
        }

        private Vector3 GetRandomPositionBetweenPoints(Transform pointA, Transform pointB)
        {
            float minX = Mathf.Min(pointA.position.x, pointB.position.x);
            float maxX = Mathf.Max(pointA.position.x, pointB.position.x);
            float minZ = Mathf.Min(pointA.position.z, pointB.position.z);
            float maxZ = Mathf.Max(pointA.position.z, pointB.position.z);

            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            return new Vector3(randomX, pointA.position.y, randomZ);
        }

        private IEnumerator DisableOffscreenObjects()
        {
            while (true)
            {
                for (int i = _objectPool.Count - 1; i >= 0; i--)
                {
                    GameObject obj = _objectPool[i];
                    if (obj.activeInHierarchy && obj.transform.position.z < _deactivationZ)
                    {
                        obj.SetActive(false);
                    }
                }
                
                yield return _disableObjectsWaitForSeconds;
            }
        }

        #endregion
    }
}
