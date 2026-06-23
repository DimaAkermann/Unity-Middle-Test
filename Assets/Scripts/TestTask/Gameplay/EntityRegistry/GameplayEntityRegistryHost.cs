using UnityEngine;

namespace TestTask.Gameplay.EntityRegistry
{
    public sealed class GameplayEntityRegistryHost : MonoBehaviour
    {
        public GameplayEntityRegistry Registry { get; } = new();
    }
}

