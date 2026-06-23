using System;
using System.Collections.Generic;

namespace TestTask.UI.Popups
{
    public sealed class PopupRequest
    {
        public string Title;
        public string Body;
        public IReadOnlyList<PopupButton> Buttons;

        public static PopupRequest Simple(string title, string body, params PopupButton[] buttons)
        {
            return new PopupRequest
            {
                Title = title,
                Body = body,
                Buttons = buttons ?? Array.Empty<PopupButton>(),
            };
        }
    }

    public readonly struct PopupButton
    {
        public readonly string Label;
        public readonly Action Callback;

        public PopupButton(string label, Action callback)
        {
            Label = label;
            Callback = callback;
        }
    }
}

