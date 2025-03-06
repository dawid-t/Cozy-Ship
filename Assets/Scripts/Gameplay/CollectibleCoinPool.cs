using UnityEngine;
using Zenject;

namespace Critsoft.CozyShip.Gameplay
{
    public class CollectibleCoinPool : MonoMemoryPool<Vector3, CollectibleCoin>
    {
        protected override void Reinitialize(Vector3 position, CollectibleCoin coin)
        {
            coin.transform.position = position;
            coin.gameObject.SetActive(true);
        }
    }
}
