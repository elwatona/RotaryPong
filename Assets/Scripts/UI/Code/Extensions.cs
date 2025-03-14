using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RotaryPong.UI
{
    public static class Extensions
    {
        public static VisualElement SetTooltip(this VisualElement element, string tooltipText)
        {
            if(string.IsNullOrEmpty(tooltipText)) return element;
            element.RegisterCallback<MouseEnterEvent>(evt =>
            {
                var tooltip = new Label(tooltipText);
                tooltip.styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/Tooltip")));
                tooltip.AddToClassList("tooltip");
                tooltip.name = "tooltip";
                tooltip.style.width = new StyleLength(element.layout.width);
                // tooltip.style.height = new StyleLength(20);

                float tooltipWidth = tooltip.layout.width;
                float tooltipHeight = tooltip.layout.height;
                float elementTop = Mathf.Abs(element.ChangeCoordinatesTo(element.parent, new Vector2(0, 0)).y);
                float elementHeight = element.layout.height;
                float elementWidth = element.layout.width;
                float screenHeight = Screen.height;
                float spaceAboveElement = elementTop;
                float spaceBelowElement = screenHeight - (elementTop + elementHeight);

                if (spaceAboveElement > tooltipHeight && spaceAboveElement > spaceBelowElement)
                {
                    // Show tooltip below element
                    tooltip.style.top = new StyleLength(elementHeight + element.style.borderTopWidth.value);
                    tooltip.style.bottom = new StyleLength(Mathf.Max(0, elementTop + elementHeight));
                }
                else
                {
                    // Show tooltip above element
                    tooltip.style.top = new StyleLength(Mathf.Max(0, elementTop - tooltipHeight));
                    tooltip.style.bottom = new StyleLength(elementHeight - element.style.borderBottomWidth.value);
                }

                // Center the tooltip horizontally
                tooltip.style.left = new StyleLength((elementWidth - tooltipWidth) / 2);
                if(element.enabledSelf) element.Add(tooltip);

                tooltip.RegisterCallback<MouseEnterEvent>(evt =>
                {
                    element.Query<VisualElement>("tooltip").ForEach(x => x.RemoveFromHierarchy());
                });
            });

            element.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                element.Query<VisualElement>("tooltip").ForEach(x => x.RemoveFromHierarchy());
            });

            return element;
        }

    }
}
