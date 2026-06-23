using UnityEngine;

namespace TestTask.Gameplay.EntityRegistry
{
    public sealed class GameplayEntity : MonoBehaviour
    {
        [SerializeField] private GameplayEntityRegistryHost _host;

        private void Awake()
        {
            if (_host == null)
                _host = FindAnyObjectByType<GameplayEntityRegistryHost>();
        }

        private void OnEnable()
        {
            _host?.Registry.Register(gameObject);
        }

        private void OnDisable()
        {
            _host?.Registry.Unregister(gameObject);
        }

        private void OnDestroy()
        {
            _host?.Registry.Unregister(gameObject);
        }
    }
}

