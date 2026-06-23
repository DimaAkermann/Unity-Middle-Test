using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.UI.Popups
{
    public sealed class PopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _body;
        [SerializeField] private Transform _buttonsRoot;
        [SerializeField] private Button _buttonPrefab;
        [SerializeField] private CanvasGroup _canvasGroup;

        private readonly List<Button> _spawnedButtons = new(5);
        private bool _initialized;

        public void InitializeIfNeeded()
        {
            if (_initialized)
                return;

            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            _initialized = true;
        }

        public void Show(PopupRequest request)
        {
            InitializeIfNeeded();

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (_title != null)
                _title.SetText(request.Title ?? string.Empty);
            if (_body != null)
                _body.SetText(request.Body ?? string.Empty);

            RebuildButtons(request.Buttons);
            SetVisible(true);
        }

        public void Hide()
        {
            SetVisible(false);
        }

        private void SetVisible(bool visible)
        {
            if (_canvasGroup == null)
            {
                gameObject.SetActive(visible);
                return;
            }

            _canvasGroup.alpha = visible ? 1f : 0f;
            _canvasGroup.interactable = visible;
            _canvasGroup.blocksRaycasts = visible;
        }

        private void RebuildButtons(IReadOnlyList<PopupButton> buttons)
        {
            if (_buttonsRoot == null || _buttonPrefab == null)
                return;

            for (var i = 0; i < _spawnedButtons.Count; i++)
            {
                if (_spawnedButtons[i] != null)
                    Destroy(_spawnedButtons[i].gameObject);
            }
            _spawnedButtons.Clear();

            var count = buttons?.Count ?? 0;
            if (count < 1)
                count = 1;
            if (count > 5)
                count = 5;

            for (var i = 0; i < count; i++)
            {
                var model = buttons != null && i < buttons.Count
                    ? buttons[i]
                    : new PopupButton("OK", callback: null);

                var btn = Instantiate(_buttonPrefab, _buttonsRoot);
                _spawnedButtons.Add(btn);

                var label = btn.GetComponentInChildren<TMP_Text>(includeInactive: true);
                if (label != null)
                    label.SetText(model.Label ?? string.Empty);

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    try { model.Callback?.Invoke(); }
                    finally { Hide(); }
                });
            }
        }
    }
}

