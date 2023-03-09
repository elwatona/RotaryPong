using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Watona.Variables;

namespace RotaryPong.UI
{
    public enum ButtonType
    {
		Menu = 0,
        Back = 1,
        Next = 2,
		Preset = 3,
		Icon = 4
    }
    public class AspectRatioButton : Button
    {
        #region USS names
            private const string styleSheet = "SquareButton";
            private const string ussBack = "button__back";
            private const string ussNext = "button__next";
			private const string ussMenu = "button__menu";
			private const string ussPreset = "button__preset";
			private const string ussIcon = "button__icon";
        #endregion
        
		[UnityEngine.Scripting.Preserve]
		public new class UxmlFactory : UxmlFactory<AspectRatioButton, UxmlTraits> { }

		[UnityEngine.Scripting.Preserve]
		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			readonly UxmlIntAttributeDescription aspectRatioX = new() { name = "aspect-ratio-x", defaultValue = 16, restriction = new UxmlValueBounds { min = "1" } };
			readonly UxmlIntAttributeDescription aspectRatioY = new() { name = "aspect-ratio-y", defaultValue = 9, restriction = new UxmlValueBounds { min = "1" } };
			readonly UxmlIntAttributeDescription balanceX = new() { name = "balance-x", defaultValue = 50, restriction = new UxmlValueBounds { min = "0", max = "100" } };
			readonly UxmlIntAttributeDescription balanceY = new() { name = "balance-y", defaultValue = 50, restriction = new UxmlValueBounds { min = "0", max = "100" } };
			// readonly UxmlEnumAttributeDescription<ButtonType> type = new() {name = "type"}; 


			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get { yield break; }
			}


			public override void Init(VisualElement visualElement, IUxmlAttributes attributes, CreationContext creationContext)
			{
				base.Init(visualElement, attributes, creationContext);
				var element = visualElement as AspectRatioButton;
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

		public AspectRatioButton() {}

		public AspectRatioButton(ButtonType type, string label = null)
		{
			ButtonType = type;
			text = label;
			styleSheets.Add(Resources.Load<StyleSheet>(string.Format("08_USS/{0}", styleSheet)));
			BaseStyle();
			RemoveFromClassList("unity-button");
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanelEvent);
            UpdateVisuals();
		}

		private void BaseStyle()
		{
			style.position = Position.Absolute;
			style.left = 0;
			style.top = 0;
			style.right = StyleKeyword.Undefined;
			style.bottom = StyleKeyword.Undefined;
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

			BaseStyle();

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
			switch(ButtonType)
			{
				case ButtonType.Back: this.AddToClassList(ussBack); break;
				case ButtonType.Next: this.AddToClassList(ussNext); break;
				case ButtonType.Menu: this.AddToClassList(ussMenu); break;
				case ButtonType.Preset: this.AddToClassList(ussPreset); break;
				case ButtonType.Icon: this.AddToClassList(ussIcon); break;
			}
        }
	}
}
