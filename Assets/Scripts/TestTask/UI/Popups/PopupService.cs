using UnityEngine;

namespace TestTask.UI.Popups
{
    public sealed class PopupService : MonoBehaviour
    {
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private string _resourcesPath = "UI/PopupView";

        private PopupView _instance;

        public void Show(PopupRequest request)
        {
            if (_instance == null)
                _instance = Instantiate(LoadPrefab(), GetRoot());

            _instance.Show(request);
        }

        public void Hide()
        {
            if (_instance != null)
                _instance.Hide();
        }

        private Transform GetRoot()
        {
            if (_uiRoot != null)
                return _uiRoot;

            var canvas = FindAnyObjectByType<Canvas>();
            if (canvas != null)
                return canvas.transform;

            return transform;
        }

        private PopupView LoadPrefab()
        {
            var prefab = Resources.Load<PopupView>(_resourcesPath);
            if (prefab == null)
                throw new System.InvalidOperationException("PopupView prefab not found in Resources.");
            return prefab;
        }
    }
}

