using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;

namespace RotaryPong.UI
{
    public enum ButtonType
    {
        Back,
        Next
    }
    public class SquareButton : Button
    {
        #region USS names
            private const string styleSheet = "SquareButton";
            private const string ussBack = "square-button__back";
            private const string ussNext = "square-button__next";
        #endregion
        
		[UnityEngine.Scripting.Preserve]
		public new class UxmlFactory : UxmlFactory<SquareButton, UxmlTraits> { }

		[UnityEngine.Scripting.Preserve]
		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			readonly UxmlIntAttributeDescription aspectRatioX = new() { name = "aspect-ratio-x", defaultValue = 16, restriction = new UxmlValueBounds { min = "1" } };
			readonly UxmlIntAttributeDescription aspectRatioY = new() { name = "aspect-ratio-y", defaultValue = 9, restriction = new UxmlValueBounds { min = "1" } };
			readonly UxmlIntAttributeDescription balanceX = new() { name = "balance-x", defaultValue = 50, restriction = new UxmlValueBounds { min = "0", max = "100" } };
			readonly UxmlIntAttributeDescription balanceY = new() { name = "balance-y", defaultValue = 50, restriction = new UxmlValueBounds { min = "0", max = "100" } };
			readonly UxmlEnumAttributeDescription<ButtonType> type = new() {name = "type"}; 


			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get { yield break; }
			}


			public override void Init(VisualElement visualElement, IUxmlAttributes attributes, CreationContext creationContext)
			{
				base.Init(visualElement, attributes, creationContext);
				var element = visualElement as SquareButton;
				if (element != null)
				{
					element.AspectRatioX = Mathf.Max(1, aspectRatioX.GetValueFromBag(attributes, creationContext));
					element.AspectRatioY = Mathf.Max(1, aspectRatioY.GetValueFromBag(attributes, creationContext));
					element.BalanceX = Mathf.Clamp(balanceX.GetValueFromBag(attributes, creationContext), 0, 100);
					element.BalanceY = Mathf.Clamp(balanceY.GetValueFromBag(attributes, creationContext), 0, 100);
                    // element.ButtonType = type.GetValueFromBag(attributes, creationContext);
					element.FitToParent();
				}
			}
		}


		public int AspectRatioX = 16;
		public int AspectRatioY = 9;
		public int BalanceX = 50;
		public int BalanceY = 50;
        public ButtonType ButtonType;

		public SquareButton() {}

		public SquareButton(ButtonType type)
		{
			ButtonType = type;
			styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", styleSheet)));
			style.position = Position.Absolute;
			style.left = 0;
			style.top = 0;
			style.right = StyleKeyword.Undefined;
			style.bottom = StyleKeyword.Undefined;
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanelEvent);
            UpdateVisuals();
		}


		void OnAttachToPanelEvent(AttachToPanelEvent e)
		{
			parent?.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
			FitToParent();
		}


		void OnGeometryChangedEvent(GeometryChangedEvent e)
		{
			FitToParent();
		}


		void FitToParent()
		{
			if (parent == null) return;
			var parentW = parent.resolvedStyle.width;
			var parentH = parent.resolvedStyle.height;
			if (float.IsNaN(parentW) || float.IsNaN(parentH)) return;

			style.position = Position.Absolute;
			style.left = 0;
			style.top = 0;
			style.right = StyleKeyword.Undefined;
			style.bottom = StyleKeyword.Undefined;

			if (AspectRatioX <= 0.0f || AspectRatioY <= 0.0f)
			{
				style.width = parentW;
				style.height = parentH;
				return;
			}

			var ratio = Mathf.Min(parentW / AspectRatioX, parentH / AspectRatioY);
			var targetW = Mathf.Floor(AspectRatioX * ratio);
			var targetH = Mathf.Floor(AspectRatioY * ratio);
			style.width = targetW;
			style.height = targetH;

			var marginX = parentW - targetW;
			var marginY = parentH - targetH;
			style.left = Mathf.Floor(marginX * BalanceX / 100.0f);
			style.top = Mathf.Floor(marginY * BalanceY / 100.0f);
		}
        
        void UpdateVisuals()
        {
            if(ButtonType == ButtonType.Back)
            {
                this.AddToClassList(ussBack);
                return;
            }
            this.AddToClassList(ussNext);
        }
	}
}
