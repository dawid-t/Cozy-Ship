using UnityEngine;

namespace Critsoft.CozyShip
{
    [CreateAssetMenu(fileName = "LevelsLibrary", menuName = "ScriptableObjects/LevelsLibrary", order = 1)]
    public class LevelsLibrarySO : ScriptableObject
    {
        [SerializeField] private LevelDataSO[] _levels;
        
        public LevelDataSO[] Levels => _levels;
    }
}
