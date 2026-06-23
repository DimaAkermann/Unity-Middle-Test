using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace TestTask.UI.Characters
{
    public sealed class CharactersView : MonoBehaviour
    {
        [SerializeField] private List<Transform> _characters;
        [SerializeField] private TMP_Text _label;
        [SerializeField, Min(1)] private int _updateEveryNFrames = 10;

        private readonly List<Character> _cache = new(64);
        private readonly StringBuilder _sb = new(128);
        private int _frame;

        private void Awake()
        {
            if (_label == null)
                _label = GetComponent<TMP_Text>();

            RebuildCache();
        }

        private void OnValidate()
        {
            if (_updateEveryNFrames < 1)
                _updateEveryNFrames = 1;
        }

        private void Update()
        {
            _frame++;
            if (_frame % _updateEveryNFrames != 0)
                return;

            Refresh();
        }

        private void RebuildCache()
        {
            _cache.Clear();
            if (_characters == null)
                return;

            for (var i = 0; i < _characters.Count; i++)
            {
                var t = _characters[i];
                if (t == null)
                    continue;

                if (t.TryGetComponent<Character>(out var c) && c != null)
                    _cache.Add(c);
            }
        }

        private void Refresh()
        {
            if (_label == null)
                return;

            if (_cache.Count == 0)
                RebuildCache();

            var total = 0f;
            var activeCount = 0;

            for (var i = 0; i < _cache.Count; i++)
            {
                var c = _cache[i];
                if (c == null)
                    continue;
                if (!c.isActiveAndEnabled)
                    continue;

                total += c.Value;
                activeCount++;
            }

            var avg = activeCount > 0 ? total / activeCount : 0f;

            _sb.Clear();
            _sb.Append("Characters: ").Append(activeCount).Append(" Avg value: ").Append(avg.ToString("0.##"));
            _label.SetText(_sb);
        }
    }
}

