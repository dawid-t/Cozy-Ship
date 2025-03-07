using Critsoft.CozyShip.Gameplay.Controllers;
using Critsoft.CozyShip.Gameplay.Models;
using Critsoft.CozyShip.Gameplay.Player;
using Critsoft.CozyShip.Gameplay.Views;
using System;
using UnityEngine;
using Zenject;

namespace Critsoft.CozyShip.Gameplay.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private LevelsLibrarySO _levelsLibrary;
        [SerializeField] private CollectibleCoin _bronzePrefab;
        [SerializeField] private CollectibleCoin _silverPrefab;
        [SerializeField] private CollectibleCoin _goldPrefab;

        public override void InstallBindings()
        {
            // Models, Views, Controllers
            Container.Bind<GameplayModel>().AsSingle().NonLazy();
            Container.Bind<GameplayView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameplayController>().FromComponentInHierarchy().AsSingle();

            Container.Bind<LevelModel>().AsSingle().NonLazy();
            Container.Bind<LevelView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();

            // Scriptable Objects
            Container.Bind<LevelsLibrarySO>().FromInstance(_levelsLibrary).AsSingle();

            // Actions / Delegates
            Container.Bind<Action<int>>()
                .WithId(GameConfig.CoinCollectedId)
                .FromMethod(ctx => ctx.Container.Resolve<GameplayController>().AddPoints)
                .AsSingle();

            // Factories / Prefabs
            Container.BindFactory<CollectibleCoin, CollectibleCoin.Factory>()
                .WithId(CoinType.Bronze)
                .FromComponentInNewPrefab(_bronzePrefab);

            Container.BindFactory<CollectibleCoin, CollectibleCoin.Factory>()
                .WithId(CoinType.Silver)
                .FromComponentInNewPrefab(_silverPrefab);

            Container.BindFactory<CollectibleCoin, CollectibleCoin.Factory>()
                .WithId(CoinType.Gold)
                .FromComponentInNewPrefab(_goldPrefab);

            Container.BindFactory<GameObject, Obstacle, Obstacle.Factory>()
                .FromFactory<PrefabFactory<Obstacle>>();

            // MemoryPool
            Container.BindMemoryPool<CollectibleCoin, CollectibleCoinPool>()
                .WithInitialSize(GameConfig.MemoryPoolInitialSize)
                .FromComponentInNewPrefab(_bronzePrefab)
                .UnderTransformGroup(GameConfig.CoinMemoryPoolName);
            
            // Other
            Container.Bind<ShipCollisionHandler>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameResultsStorage>().AsSingle();
            Container.Bind<SFXAudioManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}
