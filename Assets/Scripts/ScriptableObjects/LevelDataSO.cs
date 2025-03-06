using UnityEngine;

namespace Critsoft.CozyShip
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 2)]
    public class LevelDataSO : ScriptableObject
    {
        public int LevelId;
        public int PointsLimit;
        public float SceneScrollSpeed;
        [Space]
        public int CoinMinSpawnTime;
        public int CoinMaxSpawnTime;
        public GameObject[] CoinsToSpawn;
        [Space]
        public int ObstacleMinSpawnTime;
        public int ObstacleMaxSpawnTime;
        public GameObject[] ObstaclesToSpawn;
    }
}
