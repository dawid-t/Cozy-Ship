using UnityEngine;

namespace Critsoft.CozyShip.Gameplay
{
    public class LevelInitiatedEventArgs
    {
        public int LevelId { get; protected set; }
        public int PointsLimit { get; }
        public float SceneScrollSpeed { get; }
        
        public int CoinMinSpawnTime { get; }
        public int CoinMaxSpawnTime { get; }
        public GameObject[] CoinsToSpawn { get; }
        
        public int ObstacleMinSpawnTime { get; }
        public int ObstacleMaxSpawnTime { get; }
        public GameObject[] ObstaclesToSpawn { get; }

        public LevelInitiatedEventArgs(int levelId, int pointsLimit, float sceneScrollSpeed, int coinMinSpawnTime, int coinMaxSpawnTime,
                                     GameObject[] coinsToSpawn, int obstacleMinSpawnTime, int obstacleMaxSpawnTime, GameObject[] obstaclesToSpawn)
        {
            LevelId = levelId;
            PointsLimit = pointsLimit;
            SceneScrollSpeed = sceneScrollSpeed;
            
            CoinMinSpawnTime = coinMinSpawnTime;
            CoinMaxSpawnTime = coinMaxSpawnTime;
            CoinsToSpawn = coinsToSpawn;
            
            ObstacleMinSpawnTime = obstacleMinSpawnTime;
            ObstacleMaxSpawnTime = obstacleMaxSpawnTime;
            ObstaclesToSpawn = obstaclesToSpawn;
        }
    }
}
