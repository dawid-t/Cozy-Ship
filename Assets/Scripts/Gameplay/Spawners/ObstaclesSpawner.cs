using UnityEngine;
using Zenject;

namespace Critsoft.CozyShip.Gameplay.Spawners
{
    public class ObstaclesSpawner : ObjectsSpawner
    {
        #region Fields

        private Obstacle.Factory _obstacleFactory;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(Obstacle.Factory obstacleFactory)
        {
            _obstacleFactory = obstacleFactory;
        }
        
        #endregion

        #region Protected Methods

        protected override GameObject InstantiateObject(GameObject prefabToSpawn)
        {
            return _obstacleFactory.Create(prefabToSpawn).gameObject;
        }
        
        protected override void OnLevelInitiated(LevelInitiatedEventArgs eventArgs)
        {
            _minSpawnTime = eventArgs.ObstacleMinSpawnTime;
            _maxSpawnTime = eventArgs.ObstacleMaxSpawnTime;
            _spawnPrefabs = eventArgs.ObstaclesToSpawn;

            base.OnLevelInitiated(eventArgs);
        }
        
        #endregion
    }
}
