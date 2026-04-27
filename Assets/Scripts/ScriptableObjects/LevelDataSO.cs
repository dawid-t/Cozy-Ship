using UnityEngine;

namespace Critsoft.CozyShip
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 2)]
    public class LevelDataSO : ScriptableObject
    {
        [SerializeField] private int _levelId;
        [SerializeField] private int _pointsLimit;
        [SerializeField] private float _sceneScrollSpeed;
        [Space]
        [SerializeField] private int _coinMinSpawnTime;
        [SerializeField] private int _coinMaxSpawnTime;
        [SerializeField] private GameObject[] _coinsToSpawn;
        [Space]
        [SerializeField] private int _obstacleMinSpawnTime;
        [SerializeField] private int _obstacleMaxSpawnTime;
        [SerializeField] private GameObject[] _obstaclesToSpawn;
        
        public int LevelId => _levelId;
        public int PointsLimit => _pointsLimit;
        public float SceneScrollSpeed => _sceneScrollSpeed;
        
        public int CoinMinSpawnTime => _coinMinSpawnTime;
        public int CoinMaxSpawnTime => _coinMaxSpawnTime;
        public GameObject[] CoinsToSpawn => _coinsToSpawn;
        
        public int ObstacleMinSpawnTime => _obstacleMinSpawnTime;
        public int ObstacleMaxSpawnTime => _obstacleMaxSpawnTime;
        public GameObject[] ObstaclesToSpawn => _obstaclesToSpawn;
    }
}
