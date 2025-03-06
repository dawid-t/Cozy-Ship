using UnityEngine;
using Zenject;

namespace Critsoft.CozyShip.Gameplay.Spawners
{
    public class CoinsSpawner : ObjectsSpawner
    {
        #region Fields

        private CollectibleCoin.Factory _bronzeFactory;
        private CollectibleCoin.Factory _silverFactory;
        private CollectibleCoin.Factory _goldFactory;

        #endregion

        #region Public Methods

        [Inject]
        public void Construct(
            [Inject(Id = CoinType.Bronze)] CollectibleCoin.Factory bronzeFactory,
            [Inject(Id = CoinType.Silver)] CollectibleCoin.Factory silverFactory,
            [Inject(Id = CoinType.Gold)] CollectibleCoin.Factory goldFactory)
        {
            _bronzeFactory = bronzeFactory;
            _silverFactory = silverFactory;
            _goldFactory = goldFactory;
        }
        
        #endregion

        #region Protected Methods

        protected override GameObject InstantiateObject(GameObject prefabToSpawn)
        {
            CollectibleCoin collectibleCoin = prefabToSpawn.GetComponent<CollectibleCoin>();

            switch (collectibleCoin.CoinType)
            {
                case CoinType.Bronze:
                    return _bronzeFactory.Create().gameObject;

                case CoinType.Silver:
                    return _silverFactory.Create().gameObject;

                case CoinType.Gold:
                default:
                    return _goldFactory.Create().gameObject;
            }
        }

        protected override void OnLevelInitiated(LevelInitiatedEventArgs eventArgs)
        {
            _minSpawnTime = eventArgs.CoinMinSpawnTime;
            _maxSpawnTime = eventArgs.CoinMaxSpawnTime;
            _spawnPrefabs = eventArgs.CoinsToSpawn;

            base.OnLevelInitiated(eventArgs);
        }
        
        #endregion
    }
}
