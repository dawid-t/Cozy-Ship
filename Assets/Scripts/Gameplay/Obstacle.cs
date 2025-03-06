using UnityEngine;
using Zenject;

namespace Critsoft.CozyShip.Gameplay
{
    public class Obstacle : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<GameObject, Obstacle> { }
    }
}
