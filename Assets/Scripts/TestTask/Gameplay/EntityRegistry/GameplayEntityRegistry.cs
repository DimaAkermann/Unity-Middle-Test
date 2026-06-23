using System.Collections.Generic;
using UnityEngine;

namespace TestTask.Gameplay.EntityRegistry
{
    public sealed class GameplayEntityRegistry
    {
        private readonly HashSet<GameObject> _active = new();

        public void Register(GameObject go)
        {
            if (go != null)
                _active.Add(go);
        }

        public void Unregister(GameObject go)
        {
            if (go != null)
                _active.Remove(go);
        }

        public void GetActive(List<GameObject> buffer)
        {
            buffer.Clear();
            foreach (var go in _active)
            {
                if (go == null)
                    continue;
                if (!go.activeInHierarchy)
                    continue;
                buffer.Add(go);
            }
        }
    }
}

