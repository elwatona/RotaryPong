using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RotaryPong.UI
{
    public class CursorUI : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<CursorUI, UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        public CursorUI()
        {
            VisualElement cursor = new();
            cursor.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>("07_Images/cursor"));
            Add(cursor);
            style.position = Position.Absolute;
            style.left = 0;
            style.top = 0;
            style.height = 64;
            style.width = 64;
            cursor.style.flexGrow = 1;
            style.transformOrigin = new TransformOrigin(Length.Percent(50), Length.Percent(50), 0);
            this.focusable = true;
        }
    }
}
